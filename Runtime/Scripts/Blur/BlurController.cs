using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CollieMollie.Shaders
{
    public class BlurController : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private BlurFeature _blurFeature = null;
        [SerializeField] private float _maxValue = 100f;
        [SerializeField] private float _minValue = 0f;
        [SerializeField] private AnimationCurve _blurCurve = null;

        #endregion

        #region Public Functions
        public IEnumerator Blur(float targetValue, float duration, Action done = null)
        {
            if (_blurFeature == null) yield break;

            float elapsedTime = 0f;
            float startBlurValue = _blurFeature.FullScreenPass.GetBlurStrength();

            while (elapsedTime < duration)
            {
                _blurFeature.FullScreenPass.SetBlurStrength(Mathf.Lerp(startBlurValue, targetValue,
                    _blurCurve.Evaluate(elapsedTime / duration)));

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _blurFeature.FullScreenPass.SetBlurStrength(targetValue);
            done?.Invoke();
        }

        public async Task BlurAsync(float targetValue, float duration, Action done = null)
        {
            if (_blurFeature == null) return;

            float elapsedTime = 0f;
            float startBlurValue = _blurFeature.FullScreenPass.GetBlurStrength();

            while (elapsedTime < duration)
            {
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
