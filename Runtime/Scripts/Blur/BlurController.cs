using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CollieMollie.Shaders
{
    public class BlurController : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private BlurFeature _blurFeature = null;
        [SerializeField] private BlurEventChannel _blurEventChannel = null;

        [SerializeField] private float _maxValue = 100f;
        [SerializeField] private float _minValue = 0f;
        [SerializeField] private AnimationCurve _blurCurve = null;

        private CancellationTokenSource _cts = new CancellationTokenSource();

        #endregion

        private void OnEnable()
        {
            _blurEventChannel.OnBlurInRequest += ChangeBlurAmount;
            _blurEventChannel.OnBlurOutRequest += ChangeBlurAmount;
        }

        private void OnDisable()
        {
            _blurEventChannel.OnBlurInRequest -= ChangeBlurAmount;
            _blurEventChannel.OnBlurOutRequest -= ChangeBlurAmount;
        }

        #region Subscribers
        private void ChangeBlurAmount(float blurAmount, float duration, Action done)
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();

            if (duration <= 0)
            {
                _blurFeature.FullScreenPass.SetBlurStrength(blurAmount);
                done?.Invoke();
            }
            else
            {
                Task blurTask = BlurAsync(blurAmount, duration, _cts.Token, done);
            }
        }

        #endregion

        #region Private Functions
        private async Task BlurAsync(float targetValue, float duration, CancellationToken token, Action done = null)
        {
            if (_blurFeature == null) return;

            float elapsedTime = 0f;
            float startBlurValue = _blurFeature.FullScreenPass.GetBlurStrength();

            while (elapsedTime < duration)
            {
                token.ThrowIfCancellationRequested();

                _blurFeature.FullScreenPass.SetBlurStrength(Mathf.Lerp(startBlurValue, targetValue,
                    _blurCurve.Evaluate(elapsedTime / duration)));

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            _blurFeature.FullScreenPass.SetBlurStrength(targetValue);
            done?.Invoke();
        }

        #endregion
    }
}
