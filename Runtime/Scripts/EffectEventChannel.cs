using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.Shaders
{
    public abstract class EffectEventChannel : ScriptableObject
    {
        public virtual void RaiseEvent(EffectDir dir, Action done = null) { }

        public virtual void RaiseEvent(EffectDir dir, float amount, float duration = 0, Action done = null) { }

    }

    public enum EffectDir { In, Out }
}
