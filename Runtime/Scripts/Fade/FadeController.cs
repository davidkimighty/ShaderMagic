using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace ShaderMagic.Shaders
{
    public class FadeController : MonoBehaviour
    {
        [SerializeField] private FadeEventChannel _fadeEventChannel = null;
        [SerializeField] private FadeFeature _fadeFeature = null;
        [SerializeField] private float _maxValue = 10f;
        [SerializeField] private float _minValue = 0f;
        [SerializeField] private float _defaultFadeValue = 3f;
        [SerializeField] private float _defaultDuration = 1f;
        [SerializeField] private AnimationCurve _fadeCurve = null;

        private void OnEnable()
        {
            _fadeEventChannel.OnRequestFade += Fade;
            _fadeEventChannel.OnRequestFadeAsync += FadeAsync;
            _fadeEventChannel.OnRequestColorChange += ChangeColor;
        }

        private void OnDisable()
        {
            _fadeEventChannel.OnRequestFade -= Fade;
            _fadeEventChannel.OnRequestFadeAsync -= FadeAsync;
            _fadeEventChannel.OnRequestColorChange -= ChangeColor;
        }

        #region Public Functions
        public IEnumerator Fade(float targetValue, float duration)
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
        }

        public async Task FadeAsync(float targetValue, float duration)
        {
            if (_fadeFeature == null) return;

            float elapsedTime = 0f;
            float startFadeValue = _fadeFeature.RenderPass.GetFadeAmount();

            while (elapsedTime < duration)
            {
                _fadeFeature.RenderPass.SetFadeAmount(Mathf.Lerp(startFadeValue, targetValue,
                    _fadeCurve.Evaluate(elapsedTime / duration)));

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            _fadeFeature.RenderPass.SetFadeAmount(targetValue);
        }

        public void ChangeColor(Color color)
        {
            _fadeFeature.RenderPass.SetFadeColor(color);
        }

        #endregion
    }
}
