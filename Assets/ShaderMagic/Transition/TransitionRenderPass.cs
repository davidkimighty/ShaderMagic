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
        public RenderPassEvent RenderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
    {
        base.RecordRenderGraph(renderGraph, frameData);
    }
}
