using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TransitionEventChannel", menuName = "ShaderMagic/EventChannel/Transition")]
public class TransitionEventChannel : ScriptableObject
{
    public event Action OnTransitionStart;
    public event Action OnTransitionEnd;

    public void StartTransition()
    {
        OnTransitionStart?.Invoke();
    }

    public void EndTransition()
    {
        OnTransitionEnd?.Invoke();
    }
}
