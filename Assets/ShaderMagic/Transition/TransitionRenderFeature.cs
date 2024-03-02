using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum Transitions { Fade, }

[Serializable]
public class TransitionRenderFeature : ScriptableRendererFeature
{
    [SerializeField] private TransitionEventChannel _eventChannel;
    [SerializeField] private TransitionRenderPass.Setting _setting;
    
    private TransitionRenderPass _transitionRenderPass;
    
    public override void Create()
    {
        _transitionRenderPass = new TransitionRenderPass();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_transitionRenderPass);
    }

    private void SubscribeEvents()
    {
        //_eventChannel.OnTransitionStart += 
    }
}
