using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ShaderMagic.Shaders
{
    public class BlurFeature : ScriptableRendererFeature
    {
        public BlurPassFullScreen FullScreenPass = null;
        public PassSetting Settings = new PassSetting();

        public override void Create()
        {
            Settings.BlurMat = CoreUtils.CreateEngineMaterial("ShaderMagic/BoxBlur");
            FullScreenPass = new BlurPassFullScreen(Settings);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (FullScreenPass.IsValid())
                renderer.EnqueuePass(FullScreenPass);
        }

        [Serializable]
        public class PassSetting
        {
            public RenderPassEvent RenderPassEvent = RenderPassEvent.AfterRenderingTransparents;
            [HideInInspector] public Material BlurMat = null;
            [Range(1, 4)] public int Downsample = 1;
            [Range(0, 100)] public float BlurStrength = 5;
        }
    }
}
