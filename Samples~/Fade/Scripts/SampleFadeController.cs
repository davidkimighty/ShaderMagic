using ShaderMagic.Shaders;
using UnityEngine;

public class SampleFadeController : MonoBehaviour
{
    public FadeEventChannel FadeEventChannel = null;
    public float FadeAmount = 1f;
    public float FadeDuration = 3f;
    public Color FadeColor = Color.black;

    private async void Start()
    {
        FadeEventChannel.RequestColorChange(FadeColor);
        await FadeEventChannel.RequestFadeAsync(FadeAmount, FadeDuration);
    }
}
