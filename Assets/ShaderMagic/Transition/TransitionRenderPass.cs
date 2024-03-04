using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class TransitionRenderPass : ScriptableRenderPass
{
    [Serializable]
    public class Setting
    {
        public Transitions TransitionType = Transitions.Fade;
        public Shader TransitionShader;
        public RenderPassEvent RenderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    private class PassData
    {
        public Material TransitionMaterial;
        public TextureHandle SourceTexture;
    }

    private readonly Material _transitionMaterial;
    private readonly ProfilingSampler _profilingSampler = new("After Post");

    public TransitionRenderPass(Setting setting)
    {
        _transitionMaterial = CoreUtils.CreateEngineMaterial(setting.TransitionShader);
        renderPassEvent = setting.RenderPassEvent;
    }

    public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
    {
        UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
        UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
        
        RenderTextureDescriptor descriptor = cameraData.cameraTargetDescriptor;
        descriptor.depthBufferBits = 0;
        TextureHandle blitDestination = UniversalRenderer.CreateRenderGraphTexture(renderGraph, descriptor, "_blit", true);
        
        using (var builder = renderGraph.AddRasterRenderPass("transition pass", out PassData passData, _profilingSampler))
        {
            passData.TransitionMaterial = _transitionMaterial;
            passData.SourceTexture = resourceData.activeColorTexture;
            
            builder.UseTexture(blitDestination);
            builder.SetRenderAttachment(passData.SourceTexture, 0);
            builder.SetRenderFunc((PassData data, RasterGraphContext context) => ExecutePass(data, context));
        }
    }
    
    private static void ExecutePass(PassData passData, RasterGraphContext context)
    {
        Blitter.BlitTexture(context.cmd, passData.SourceTexture, new Vector4(1, 1, 0, 0), passData.TransitionMaterial, 0);
    }
}
