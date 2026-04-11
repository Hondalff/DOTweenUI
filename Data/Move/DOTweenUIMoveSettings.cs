using System;
using UnityEngine;

namespace DOTweenUI
{
    [Serializable]
    public class DOTweenUIMoveSettings
    {
        [SerializeField] private DOTweenUIMoveSpace moveSpace = DOTweenUIMoveSpace.AnchoredPosition;
        [SerializeField] private DOTweenUIMoveMode moveMode = DOTweenUIMoveMode.FromTo;
        [SerializeField] private Vector2 from;
        [SerializeField] private Vector2 to;

        public DOTweenUIMoveSpace MoveSpace => moveSpace;
        public DOTweenUIMoveMode MoveMode => moveMode;
        public Vector2 From => from;
        public Vector2 To => to;
    }
}