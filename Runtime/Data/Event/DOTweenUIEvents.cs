using System;
using UnityEngine;
using UnityEngine.Events;

namespace DOTweenUI
{
    [Serializable]
    public class DOTweenUIEvents
    {
        [SerializeField] private UnityEvent onPlay = new();
        [SerializeField] private UnityEvent onComplete = new();
        [SerializeField] private UnityEvent onKill = new();
        
        public UnityEvent OnPlay => onPlay;
        public UnityEvent OnComplete => onComplete;
        public UnityEvent OnKill => onKill;
    }
}