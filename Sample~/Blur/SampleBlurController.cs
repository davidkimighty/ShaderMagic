using System.Collections;
using System.Collections.Generic;
using CollieMollie.Shaders;
using UnityEngine;

public class SampleBlurController : MonoBehaviour
{
    public BlurEventChannel _blurEventChannel = null;
    public float _blurAmount = 10f;
    public float _blurDuration = 3f;

    private void Start()
    {
        _blurEventChannel.RaiseEvent(EffectDir.In, _blurAmount, _blurDuration, () =>
        {
            _blurEventChannel.RaiseEvent(EffectDir.Out, 0, _blurDuration);
        });
    }
}
