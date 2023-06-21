using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace ShaderMagic.Shaders
{
    [CreateAssetMenu(fileName = "EventChannel_Fade", menuName = "ShaderMagic/Event Channels/Fade")]
    public class FadeEventChannel : ScriptableObject
    {
        public event Func<float, float, IEnumerator> OnRequestFade = null;
        public event Func<float, float, Task> OnRequestFadeAsync = null;
        public event Action<Color> OnRequestColorChange = null;

        public event Action OnBeforeFade = null;
        public event Action OnAfterFade = null;

        #region Publishers
        public IEnumerator RequestFade(float targetValue, float duration)
        {
            OnBeforeFade?.Invoke();

            if (OnRequestFade != null)
                yield return OnRequestFade.Invoke(targetValue, duration);

            OnAfterFade?.Invoke();
        }

        public async Task RequestFadeAsync(float targetValue, float duration)
        {
            OnBeforeFade?.Invoke();

            if (OnRequestFadeAsync != null)
                await OnRequestFadeAsync.Invoke(targetValue, duration);

            OnAfterFade?.Invoke();
        }

        public void RequestColorChange(Color color)
        {
            OnRequestColorChange?.Invoke(color);
        }

        #endregion
    }
}
