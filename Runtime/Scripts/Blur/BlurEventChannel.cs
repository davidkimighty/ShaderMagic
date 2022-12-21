using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.Shaders
{
    [CreateAssetMenu(fileName = "EventChannel_Blur", menuName = "CollieMollie/Event Channels/Blur")]
    public class BlurEventChannel : EffectEventChannel
    {
        #region Events
        public event Action<float, float, Action> OnBlurInRequest = null;
        public event Action<float, float, Action> OnBlurOutRequest = null;

        #endregion

        #region Publishers
        public override void RaiseEvent(EffectDir dir, Action done = null)
        {
            switch (dir)
            {
                case EffectDir.In:
                    OnBlurInRequest?.Invoke(-1f, -1f, done);
                    break;

                case EffectDir.Out:
                    OnBlurOutRequest?.Invoke(-1f, -1f, done);
                    break;
            }
        }

        public override void RaiseEvent(EffectDir dir, float amount, float duration = 0, Action done = null)
        {
            switch (dir)
            {
                case EffectDir.In:
                    OnBlurInRequest?.Invoke(amount, duration, done);
                    break;

                case EffectDir.Out:
                    OnBlurOutRequest?.Invoke(amount, duration, done);
                    break;
            }
        }

        #endregion
    }
}
