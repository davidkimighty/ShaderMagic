using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ShaderMagic.Shaders
{
    public class BlurPassFullScreen : ScriptableRenderPass
    {
        private const string s_profilerTag = "Blur Pass";
        private static readonly int s_blurStrengthProperty = Shader.PropertyToID("_BlurStrength");

        private BlurFeature.PassSetting _passSettings = null;
        private RenderTargetIdentifier _colorBuffer;
        private RenderTargetIdentifier _temporaryBuffer;
        private int _temporaryBufferID = Shader.PropertyToID("_TemporaryBuffer");
        private Material _blurMaterial = null;

        public BlurPassFullScreen(BlurFeature.PassSetting passSettings)
        {
            this._passSettings = passSettings;

            renderPassEvent = passSettings.RenderPassEvent;
            _blurMaterial = passSettings.BlurMat;
            _blurMaterial.SetFloat(s_blurStrengthProperty, passSettings.BlurStrength);
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

            descriptor.width /= _passSettings.Downsample;
            descriptor.height /= _passSettings.Downsample;
            descriptor.depthBufferBits = 0;

            ConfigureInput(ScriptableRenderPassInput.Depth);
            ConfigureInput(ScriptableRenderPassInput.Normal);

            _colorBuffer = renderingData.cameraData.renderer.cameraColorTargetHandle;

            cmd.GetTemporaryRT(_temporaryBufferID, descriptor, FilterMode.Bilinear);
            _temporaryBuffer = new RenderTargetIdentifier(_temporaryBufferID);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, new ProfilingSampler(s_profilerTag)))
            {
                cmd.Blit(_colorBuffer, _temporaryBuffer, _blurMaterial, 0);
                cmd.Blit(_temporaryBuffer, _colorBuffer, _blurMaterial, 1);
            }
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            if (cmd == null) throw new ArgumentNullException("cmd");

            cmd.ReleaseTemporaryRT(_temporaryBufferID);
        }

        #region Blur Controls
        public bool IsValid()
        {
            return _blurMaterial != null;
        }

        public void SetBlurStrength(float strength)
        {
            if (_blurMaterial == null) return;
            _blurMaterial.SetFloat(s_blurStrengthProperty, strength);
        }

        public float GetBlurStrength()
        {
            if (_blurMaterial == null) return 0f;
            return _blurMaterial.GetFloat(s_blurStrengthProperty);
        }

        #endregion
    }
}
