using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace ShaderMagic.Shaders
{
    public class BlurController : MonoBehaviour
    {
        [SerializeField] private BlurEventChannel _blurEventChannel = null;
        [SerializeField] private BlurFeature _blurFeature = null;
        [SerializeField] private float _maxValue = 100f;
        [SerializeField] private float _minValue = 0f;
        [SerializeField] private AnimationCurve _blurCurve = null;

        private void OnEnable()
        {
            _blurEventChannel.OnRequestBlur += Blur;
            _blurEventChannel.OnRequestBlurAsync += BlurAsync;
        }

        private void OnDisable()
        {
            _blurEventChannel.OnRequestBlur -= Blur;
            _blurEventChannel.OnRequestBlurAsync -= BlurAsync;
        }

        #region Public Functions
        public IEnumerator Blur(float targetValue, float duration)
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

        public async Task BlurAsync(float targetValue, float duration)
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
        }

        #endregion
    }
}
