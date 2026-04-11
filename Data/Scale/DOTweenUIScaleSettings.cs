using System;
using UnityEngine;

namespace DOTweenUI
{
    [Serializable]
    public class DOTweenUIScaleSettings
    {
        [SerializeField] private DOTweenUIScaleMode scaleMode = DOTweenUIScaleMode.FromTo;
        [SerializeField] private Vector3 from = Vector3.one;
        [SerializeField] private Vector3 to = Vector3.one;

        public DOTweenUIScaleMode ScaleMode => scaleMode;
        public Vector3 From => from;
        public Vector3 To => to;
    }
}