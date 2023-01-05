using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

        private CancellationTokenSource _cts = new CancellationTokenSource();

        #endregion
    
        private void OnEnable()
        {
            _fadeEventChannel.OnFadeInRequest += ChangeFadeAmount;
            _fadeEventChannel.OnFadeOutRequest += ChangeFadeAmount;
            _fadeEventChannel.OnColorChangeRequest += SetFadeColor;
        }

        private void OnDisable()
        {
            _fadeEventChannel.OnFadeInRequest -= ChangeFadeAmount;
            _fadeEventChannel.OnFadeOutRequest -= ChangeFadeAmount;
            _fadeEventChannel.OnColorChangeRequest -= SetFadeColor;
        }

        #region Subscribers
        private void ChangeFadeAmount(float fadeAmount, float duration, Action done)
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();

            if (duration <= 0)
            {
                _fadeFeature.RenderPass.SetFadeAmount(fadeAmount);
                done?.Invoke();
            }
            else
            {
                Task fadeTask = FadeAsync(fadeAmount, duration, _cts.Token, done);
            }
        }

        private void SetFadeColor(Color color, Action done)
        {
            _fadeFeature.RenderPass.SetFadeColor(color);
            done?.Invoke();
        }

        #endregion

        #region Private Functions
        private async Task FadeAsync(float targetValue, float duration, CancellationToken token, Action done = null)
        {
            if (_fadeFeature == null) return;
            float elapsedTime = 0f;
            float startFadeValue = _fadeFeature.RenderPass.GetFadeAmount();
            while (elapsedTime < duration)
            {
                token.ThrowIfCancellationRequested();

                _fadeFeature.RenderPass.SetFadeAmount(Mathf.Lerp(startFadeValue, targetValue,
                    _fadeCurve.Evaluate(elapsedTime / duration)));

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            _fadeFeature.RenderPass.SetFadeAmount(targetValue);
            done?.Invoke();
        }

        #endregion
    }
}
