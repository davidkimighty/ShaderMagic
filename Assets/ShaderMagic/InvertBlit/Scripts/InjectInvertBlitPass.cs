using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class InjectInvertBlitPass : MonoBehaviour
{
    [SerializeField] private Material _invertMat;
    [SerializeField] private Material _blitMat;
    [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingSkybox;

    private InvertBlitRenderPass _renderPass;

    private void OnEnable()
    {
        CreateRenderPass();
        RenderPipelineManager.beginCameraRendering += InjectPass;
    }

    private void CreateRenderPass()
    {
        if (_invertMat == null || _blitMat == null)
        {
            Debug.Log("One or more materials are null. Pass injection failed.");
            return;
        }

        _renderPass = new InvertBlitRenderPass(new InvertBlitRenderPass.Setting()
        {
            InvertMat = _invertMat,
            BlitMat = _blitMat,
            RenderPassEvent = _renderPassEvent
        });
    }
    
    private void InjectPass(ScriptableRenderContext context, Camera cam)
    {
        if (_renderPass == null)
            CreateRenderPass();

        if (cam.cameraType != CameraType.Game && cam.cameraType != CameraType.VR) return;
        
        UniversalAdditionalCameraData data = cam.GetUniversalAdditionalCameraData();
        data.scriptableRenderer.EnqueuePass(_renderPass);
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= InjectPass;
    }
}
