using System;
using UnityEngine;

namespace DOTweenUI
{
    [Serializable]
    public class DOTweenUIRotateSettings
    {
        [SerializeField] private Transform transform;
        [SerializeField] private DOTweenUIRotateMode rotateMode = DOTweenUIRotateMode.FromTo;
        [SerializeField] private Vector3 from;
        [SerializeField] private Vector3 to;
        
        public Transform Transform => transform;
        public DOTweenUIRotateMode RotateMode => rotateMode;
        public Vector3 From => from;
        public Vector3 To => to;
    }
}