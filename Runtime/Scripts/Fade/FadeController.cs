using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CollieMollie.Shaders
{
    public class FadeController : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private FadeFeature _fadeFeature = null;
        [SerializeField] private FadeEventChannel _fadeEventChannel = null;

        [SerializeField] private float _maxValue = 10f;
        [SerializeField] private float _minValue = 0f;
        [SerializeField] private float _defaultFadeValue = 3f;
        [SerializeField] private float _defaultDuration = 1f;
        [SerializeField] private AnimationCurve _fadeCurve = null;

        private IEnumerator _fadeAction = null;
        #endregion

        private void Awake()
        {
            _fadeEventChannel.OnFadeInRequest += ChangeFadeAmount;
        }

        #region Subscribers
        private void ChangeFadeAmount(float fadeAmount, float duration, Action done)
        {
            if (_fadeAction != null)
                StopCoroutine(_fadeAction);

            if (duration == 0)
            {
                _fadeFeature.RenderPass.SetFadeAmount(fadeAmount);
                done?.Invoke();
            }
            else
            {
                _fadeAction = Fade(duration > 0 ? fadeAmount : _defaultFadeValue,
                    duration > 0 ? duration : _defaultDuration, done);
                StartCoroutine(_fadeAction);
            }
        }

        #endregion

        #region Public Functions
        public IEnumerator FadeAmount(float fadeAmount, float duration = 1f, Action done = null)
        {
            fadeAmount = Mathf.Clamp(fadeAmount, _minValue, _maxValue);
            yield return Fade(fadeAmount, duration);
            done?.Invoke();
        }

        public void FadeAmoutInstant(float fadeAmount)
        {
            _fadeFeature.RenderPass.SetFadeAmount(fadeAmount);
        }

        public void SetFadeColor(Color color)
        {
            _fadeFeature.RenderPass.SetFadeColor(color);
        }

        #endregion

        #region Private Functions
        private IEnumerator Fade(float targetValue, float duration = 1f, Action done = null)
        {
            if (_fadeFeature == null) yield break;
            float elapsedTime = 0f;
            float startFadeValue = _fadeFeature.RenderPass.GetFadeAmount();
            while (elapsedTime < duration)
            {
                _fadeFeature.RenderPass.SetFadeAmount(Mathf.Lerp(startFadeValue, targetValue,
                    _fadeCurve.Evaluate(elapsedTime / duration)));

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _fadeFeature.RenderPass.SetFadeAmount(targetValue);
            done?.Invoke();
        }
        #endregion
    }
}
