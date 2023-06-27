using ShaderMagic.Shaders;
using UnityEngine;

public class SampleBlurController : MonoBehaviour
{
    public BlurEventChannel BlurEventChannel = null;
    public float BlurAmount = 10f;
    public float BlurDuration = 3f;

    private async void Start()
    {
        await BlurEventChannel.RequestBlurAsync(BlurAmount, BlurDuration);
    }
}
