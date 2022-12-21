using System.Collections;
using System.Collections.Generic;
using CollieMollie.Shaders;
using UnityEngine;

public class SampleFadeController : MonoBehaviour
{
    public FadeEventChannel _fadeEventChannel = null;
    public float _fadeAmount = 1f;
    public float _fadeDuration = 3f;

    private void Start()
    {
        _fadeEventChannel.RaiseEvent(EffectDir.In, _fadeAmount, _fadeDuration);
    }
}
