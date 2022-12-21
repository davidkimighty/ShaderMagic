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
        [SerializeField] private float _defaultBlurValue = 3f;
        [SerializeField] private float _defaultDuration = 1f;
        [SerializeField] private AnimationCurve _blurCurve = null;

        private IEnumerator _blurAction = null;
        #endregion

        private void Awake()
        {
            _blurEventChannel.OnBlurInRequest += ChangeBlurAmount;
            _blurEventChannel.OnBlurOutRequest += ChangeBlurAmount;
        }

        #region Subscribers
        private void ChangeBlurAmount(float blurAmount, float duration, Action done)
        {
            if (_blurAction != null)
                StopCoroutine(_blurAction);

            if (duration == 0)
            {
                _blurFeature.FullScreenPass.SetBlurStrength(blurAmount);
                done?.Invoke();
            }
            else
            {
                _blurAction = Blur(duration > 0 ? blurAmount : _defaultBlurValue,
                    duration > 0 ? duration : _defaultDuration, done);
                StartCoroutine(_blurAction);
            }
        }

        #endregion

        #region Public Functions
        public IEnumerator BlurAmount(float targetValue, float duration = 1f, Action done = null)
        {
            targetValue = Mathf.Clamp(targetValue, _minValue, _maxValue);
            yield return Blur(targetValue, duration);
            done?.Invoke();
        }

        public void BlurAmoutInstant(float blurAmout)
        {
            _blurFeature.FullScreenPass.SetBlurStrength(blurAmout);
        }

        #endregion

        #region Private Functions
        private IEnumerator Blur(float targetValue, float duration = 1f, Action done = null)
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
