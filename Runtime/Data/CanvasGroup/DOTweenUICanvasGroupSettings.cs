using System;
using UnityEngine;

namespace DOTweenUI
{
    [Serializable]
    public class DOTweenUICanvasGroupSettings
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private DOTweenUICanvasGroupMode mode = DOTweenUICanvasGroupMode.FromTo;
        
        [SerializeField, Range(0f, 1f)] private float fromAlpha = 1f;
        [SerializeField, Range(0f, 1f)] private float toAlpha = 1f;
        
        [Header("On Start")]
        [SerializeField] private bool interactableOnStart;
        [SerializeField] private bool blocksRaycastsOnStart;
        
        [Header("On Complete")]
        [SerializeField] private bool interactableOnComplete = true;
        [SerializeField] private bool blocksRaycastsOnComplete = true;
        
        public CanvasGroup CanvasGroup => canvasGroup;
        public DOTweenUICanvasGroupMode CanvasGroupMode => mode;
        public float FromAlpha => fromAlpha;
        public float ToAlpha => toAlpha;
        
        public bool InteractableOnStart => interactableOnStart;
        public bool BlocksRaycastsOnStart => blocksRaycastsOnStart;
        
        public bool InteractableOnComplete => interactableOnComplete;
        public bool BlocksRaycastsOnComplete => blocksRaycastsOnComplete;
    }
}