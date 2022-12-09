using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CollieMollie.Shaders
{
    public class BlurController : MonoBehaviour
    {
        #region Variable Field
        private const float MAXVALUE = 30f;
        private const float MINVALUE = 0f;

        [SerializeField] private UniversalRendererData _rendererData = null;
        [SerializeField] private Renderer2DData _rendererData2D = null;
        [SerializeField] private AnimationCurve _blurCurve = null;

        private BlurFeature _blurFeature = null;
        #endregion

        private void Awake()
        {
            if (_rendererData != null)
                _blurFeature = (BlurFeature)_rendererData.rendererFeatures.Find(feature => feature is BlurFeature);
            else if (_rendererData2D != null)
                _blurFeature = (BlurFeature)_rendererData2D.rendererFeatures.Find(feature => feature is BlurFeature);
        }

        #region Public Functions
        public IEnumerator BlurIn(float duration = 1f, Action done = null)
        {
            yield return Blur(MAXVALUE, duration);
            done?.Invoke();
        }

        public IEnumerator BlurOut(float duration = 1f, Action done = null)
        {
            yield return Blur(MINVALUE, duration);
            done?.Invoke();
        }

        public IEnumerator BlurAmount(float targetValue, float duration = 1f, Action done = null)
        {
            targetValue = Mathf.Clamp(targetValue, MINVALUE, MAXVALUE);
            yield return Blur(targetValue, duration);
            done?.Invoke();
        }

        public void BlurAmout(float blurAmout)
        {
            _blurFeature.FullScreenPass.SetBlurStrength(blurAmout);
        }

        #endregion

        #region Blur Controls
        private IEnumerator Blur(float targetValue, float duration = 1f)
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
        }

        #endregion
    }
}
