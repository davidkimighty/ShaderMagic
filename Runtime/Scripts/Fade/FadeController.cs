using System;
using System.Collections;
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

        private IEnumerator _fadeCoroutine = null;

        #endregion
    
        private void OnEnable()
        {
            _fadeEventChannel.OnFadeInRequest += ChangeFadeAmount;
            _fadeEventChannel.OnFadeOutRequest += ChangeFadeAmount;
            _fadeEventChannel.OnColorChangeRequest += ChangeFadeColor;
        }

        private void OnDisable()
        {
            _fadeEventChannel.OnFadeInRequest -= ChangeFadeAmount;
            _fadeEventChannel.OnFadeOutRequest -= ChangeFadeAmount;
            _fadeEventChannel.OnColorChangeRequest -= ChangeFadeColor;
        }

        #region Public Functions
        public void ChangeFadeAmount(float fadeAmount, float duration, Action done = null)
        {
            if (duration <= 0)
            {
                _fadeFeature.RenderPass.SetFadeAmount(fadeAmount);
                done?.Invoke();
            }
            else
            {
                if (_fadeCoroutine != null)
                    StopCoroutine(_fadeCoroutine);

                _fadeCoroutine = Fade(fadeAmount, duration, done);
                StartCoroutine(_fadeCoroutine);
            }
        }

        public void ChangeFadeColor(Color color, Action done = null)
        {
            _fadeFeature.RenderPass.SetFadeColor(color);
            done?.Invoke();
        }

        #endregion

        #region Private Functions
        private IEnumerator Fade(float targetValue, float duration, Action done = null)
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
