using System.Collections;
using System.Collections.Generic;
using CollieMollie.Shaders;
using UnityEngine;

public class SampleFadeController : MonoBehaviour
{
    public FadeEventChannel _fadeEventChannel = null;
    public float _fadeAmount = 1f;
    public float _fadeDuration = 3f;
    public Color _fadeColor = Color.black;

    private void Start()
    {
        _fadeEventChannel.RaiseEvent(EffectDir.In, _fadeAmount, _fadeDuration, () =>
        {
            _fadeEventChannel.RaiseEvent(EffectDir.Out, 0, _fadeDuration);
        });
    }
}
