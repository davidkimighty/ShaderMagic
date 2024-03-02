using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class InvertBlitRenderPass : ScriptableRenderPass
{
    [Serializable]
    public class Setting
    {
        public Material InvertMat;
        public Material BlitMat;
        public RenderPassEvent RenderPassEvent = RenderPassEvent.AfterRenderingSkybox;
    }
    
    private class PassData
    {
        public Material BlitMat;
        public TextureHandle SourceTexture;
    }

    private readonly Material _invertColorMat;
    private readonly Material _blitMat;
    private readonly ProfilingSampler _profilingSampler = new("After Opaques");

    public InvertBlitRenderPass(Setting setting)
    {
        _invertColorMat = setting.InvertMat;
        _blitMat = setting.BlitMat;
        renderPassEvent = setting.RenderPassEvent;
    }
    
    public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
    {
        UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
        UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
        
        RenderTextureDescriptor descriptor = cameraData.cameraTargetDescriptor;
        descriptor.depthBufferBits = 0;
        TextureHandle destinationTexture = UniversalRenderer.CreateRenderGraphTexture(renderGraph, descriptor, "_rt", true);

        using (var builder = renderGraph.AddRasterRenderPass("invert", out PassData passData, _profilingSampler))
        {
            passData.BlitMat = _invertColorMat;
            passData.SourceTexture = resourceData.activeColorTexture;
            
            builder.UseTexture(passData.SourceTexture);
            builder.SetRenderAttachment(destinationTexture, 0);
            builder.SetRenderFunc((PassData data, RasterGraphContext context) => ExecutePass(data, context));
        }
        
        using (var builder = renderGraph.AddRasterRenderPass("blit", out PassData passData, _profilingSampler))
        {
            passData.BlitMat = _blitMat;
            passData.SourceTexture = resourceData.activeColorTexture;
            
            builder.UseTexture(destinationTexture);
            builder.SetRenderAttachment(passData.SourceTexture, 0);
            builder.SetRenderFunc((PassData data, RasterGraphContext context) => ExecutePass(data, context));
        }
    }

    private static void ExecutePass(PassData data, RasterGraphContext context)
    {
        Blitter.BlitTexture(context.cmd, data.SourceTexture, new Vector4(1, 1, 0, 0), data.BlitMat, 0);
    }
}
