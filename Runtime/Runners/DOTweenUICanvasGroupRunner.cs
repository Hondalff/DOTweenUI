using DG.Tweening;
using UnityEngine;

namespace DOTweenUI
{
    public class DOTweenUICanvasGroupRunner : IDOTweenUIAnimationRunner
    {
        public DOTweenUIAnimationType AnimationType => DOTweenUIAnimationType.CanvasGroup;

        public Tween CreateTween(DOTweenUIEntry entry)
        {
            DOTweenUICanvasGroupSettings settings = entry.CanvasGroupSettings;
            CanvasGroup canvasGroup = settings.CanvasGroup;

            ApplyStartState(canvasGroup, settings);

            float endAlpha = ResolveTarget(canvasGroup, settings);

            Tween tween = canvasGroup.DOFade(endAlpha, entry.PlaybackSettings.Duration);

            tween.OnComplete(() => { ApplyCompleteState(canvasGroup, settings); });

            return tween;
        }

        private static float ResolveTarget(CanvasGroup target, DOTweenUICanvasGroupSettings settings)
        {
            switch (settings.CanvasGroupMode)
            {
                case DOTweenUICanvasGroupMode.FromTo:
                    target.alpha = settings.FromAlpha;
                    return settings.ToAlpha;

                case DOTweenUICanvasGroupMode.ToAbsolute:
                    return settings.ToAlpha;

                default:
                    return settings.ToAlpha;
            }
        }

        private static void ApplyStartState(CanvasGroup target, DOTweenUICanvasGroupSettings settings)
        {
            target.interactable = settings.InteractableOnStart;
            target.blocksRaycasts = settings.BlocksRaycastsOnStart;
        }

        private static void ApplyCompleteState(CanvasGroup target, DOTweenUICanvasGroupSettings settings)
        {
            target.interactable = settings.InteractableOnComplete;
            target.blocksRaycasts = settings.BlocksRaycastsOnComplete;
        }
    }
}