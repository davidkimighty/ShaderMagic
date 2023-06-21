using System.Threading.Tasks;
using ShaderMagic.Shaders;
using UnityEngine;

public class SampleFadeController : MonoBehaviour
{
    public FadeController _fadeController = null;
    public float _fadeAmount = 1f;
    public float _fadeDuration = 3f;
    public Color _fadeColor = Color.black;

    private async Task Start()
    {
        await _fadeController.FadeAsync(_fadeAmount, _fadeDuration);
    }
}
