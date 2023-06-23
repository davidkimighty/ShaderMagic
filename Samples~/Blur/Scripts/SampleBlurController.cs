using System.Threading.Tasks;
using ShaderMagic.Shaders;
using UnityEngine;

public class SampleBlurController : MonoBehaviour
{
    public BlurController BlurController = null;
    public float BlurAmount = 10f;
    public float BlurDuration = 3f;

    private async Task Start()
    {
        await BlurController.BlurAsync(BlurAmount, BlurDuration);
    }
}
