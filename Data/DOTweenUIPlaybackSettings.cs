using System;
using DG.Tweening;
using UnityEngine;

namespace DOTweenUI
{
    [Serializable]
    public class DOTweenUIPlaybackSettings
    {
        [SerializeField, Min(0f)] private float delay = 0f;
        [SerializeField, Min(0f)] private float duration = 0.3f;
        [SerializeField] private Ease ease = Ease.Linear;
        [SerializeField, Range(-1, 100)] private int loops = 1;
        [SerializeField] private LoopType loopType = LoopType.Restart;
        [SerializeField] private bool autoKill = true;
        [SerializeField] private bool killPreviousTweenBeforePlay = true;
        [SerializeField] private DOTweenUIAnimationUpdate updateMode = DOTweenUIAnimationUpdate.Unscaled;

        public float Delay => delay;
        public float Duration => duration;
        public Ease Ease => ease;
        public int Loops => loops;
        public LoopType LoopType => loopType;
        public bool AutoKill => autoKill;
        public bool KillPreviousTweenBeforePlay => killPreviousTweenBeforePlay;
        public DOTweenUIAnimationUpdate UpdateMode => updateMode;
    }
}