using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.Shaders
{
    [CreateAssetMenu(fileName = "EventChannel_Fade", menuName = "CollieMollie/Event Channels/Fade")]
    public class FadeEventChannel : EffectEventChannel
    {
        #region Events
        public event Action<float, float, Action> OnFadeInRequest = null;
        public event Action<float, float, Action> OnFadeOutRequest = null;

        #endregion

        #region Publishers
        public override void RaiseEvent(EffectDir dir, Action done = null)
        {
            switch (dir)
            {
                case EffectDir.In:
                    OnFadeInRequest?.Invoke(-1f, -1f, done);
                    break;

                case EffectDir.Out:
                    OnFadeOutRequest?.Invoke(-1f, -1f, done);
                    break;
            }
        }

        public override void RaiseEvent(EffectDir dir, float amount, float duration = 0, Action done = null)
        {
            switch (dir)
            {
                case EffectDir.In:
                    OnFadeInRequest?.Invoke(amount, duration, done);
                    break;

                case EffectDir.Out:
                    OnFadeOutRequest?.Invoke(amount, duration, done);
                    break;
            }
        }
        #endregion
    }
}
