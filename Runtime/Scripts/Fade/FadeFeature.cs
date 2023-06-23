using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ShaderMagic.Shaders
{
    public class FadeFeature : ScriptableRendererFeature
    {
        public FadePass RenderPass = null;
        public PassSetting Settings = new PassSetting();

        public override void Create()
        {
            Settings.FadeMaterial = CoreUtils.CreateEngineMaterial("ShaderMagic/Fade");
            RenderPass = new FadePass(Settings);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(RenderPass);
        }

        [Serializable]
        public class PassSetting
        {
            [HideInInspector] public string ProfileTag = "Screen Fade";
            [HideInInspector] public Material FadeMaterial = null;

            public Color FadeColor;
            [Range(0, 1)] public float FadeValue = 0f;
            public RenderPassEvent RenderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        }
    }
}
