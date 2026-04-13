using System;
using UnityEngine.Events;

namespace DOTweenUI
{
    [Serializable]
    public class DOTweenUIEvents
    {
        public UnityEvent OnPlay = new ();
        public UnityEvent OnComplete = new ();
        public UnityEvent OnKill = new ();
    }
}