using System;
using UnityEngine;

namespace DOTweenUI
{
    [Serializable]
    public class DOTweenUIIntervalSettings
    {
        [SerializeField, Min(0f)] private float interval = 0.25f;

        public float Interval => interval;
    }
}