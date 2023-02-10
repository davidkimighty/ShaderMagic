using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CollieMollie.Shaders;
using UnityEngine;

public class SampleBlurController : MonoBehaviour
{
    public BlurController _blurController = null;
    public float _blurAmount = 10f;
    public float _blurDuration = 3f;

    private async Task Start()
    {
        await _blurController.BlurAsync(_blurAmount, _blurDuration);
    }
}
