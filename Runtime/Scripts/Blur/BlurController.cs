using System;
using System.Collections;
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

        private IEnumerator _blurCoroutine = null;

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

        #region Public Functions
        public void ChangeBlurAmount(float blurAmount, float duration, Action done = null)
        {
            if (duration <= 0)
            {
                _blurFeature.FullScreenPass.SetBlurStrength(blurAmount);
                done?.Invoke();
            }
            else
            {
                if (_blurCoroutine != null)
                    StopCoroutine(_blurCoroutine);
                _blurCoroutine = Blur(blurAmount, duration, done);
                StartCoroutine(_blurCoroutine);
            }
        }

        #endregion

        #region Private Functions
        private IEnumerator Blur(float targetValue, float duration, Action done = null)
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

        #endregion
    }
}
