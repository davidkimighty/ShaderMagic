using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace ShaderMagic.Shaders
{
    [CreateAssetMenu(fileName = "EventChannel_Blur", menuName = "ShaderMagic/Event Channels/Blur")]
    public class BlurEventChannel : ScriptableObject
    {
        public event Func<float, float, IEnumerator> OnRequestBlur = null;
        public event Func<float, float, Task> OnRequestBlurAsync = null;

        public event Action OnBeforeBlur = null;
        public event Action OnAfterBlur = null;

        #region Publishers
        public IEnumerator RequestBlur(float targetValue, float duration)
        {
            OnBeforeBlur?.Invoke();

            if (OnRequestBlur != null)
                yield return OnRequestBlur.Invoke(targetValue, duration);

            OnAfterBlur?.Invoke();
        }

        public async Task RequestBlurAsync(float targetValue, float duration)
        {
            OnBeforeBlur?.Invoke();

            if (OnRequestBlurAsync != null)
                await OnRequestBlurAsync.Invoke(targetValue, duration);

            OnAfterBlur?.Invoke();
        }

        #endregion
    }
}
