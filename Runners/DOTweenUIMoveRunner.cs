using DG.Tweening;
using UnityEngine;

namespace DOTweenUI.Runners
{
    public class DOTweenUIMoveRunner : IDOTweenUIAnimationRunner
    {
        public DOTweenUIAnimationType AnimationType => DOTweenUIAnimationType.Move;

        public Tween CreateTween(DOTweenUIEntry entry)
        {
            DOTweenUIMoveSettings settings = entry.MoveSettings;
            RectTransform rectTransform = entry.MoveSettings.RectTransform;
            float duration = entry.PlaybackSettings.Duration;

            Vector2 target = ResolveTarget(rectTransform, settings);

            return settings.MoveSpace switch
            {
                DOTweenUIMoveSpace.AnchoredPosition => rectTransform.DOAnchorPos(target, duration),
                DOTweenUIMoveSpace.LocalPosition => rectTransform.DOLocalMove(target, duration),
                _ => null,
            };
        }

        private static Vector2 ResolveTarget(RectTransform rectTransform, DOTweenUIMoveSettings settings)
        {
            switch (settings.MoveMode)
            {
                case DOTweenUIMoveMode.FromTo:
                    ApplyFrom(rectTransform, settings);
                    return settings.To;

                case DOTweenUIMoveMode.ToAbsolute:
                    return settings.To;

                case DOTweenUIMoveMode.ToRelative:
                    return settings.MoveSpace switch
                    {
                        DOTweenUIMoveSpace.AnchoredPosition => rectTransform.anchoredPosition + settings.To,
                        DOTweenUIMoveSpace.LocalPosition => (Vector2)rectTransform.localPosition + settings.To,
                        _ => settings.To,
                    };

                default:
                    return settings.To;
            }
        }

        private static void ApplyFrom(RectTransform rectTransform, DOTweenUIMoveSettings settings)
        {
            switch (settings.MoveSpace)
            {
                case DOTweenUIMoveSpace.AnchoredPosition:
                    rectTransform.anchoredPosition = settings.From;
                    break;

                case DOTweenUIMoveSpace.LocalPosition:
                    Vector3 currentLocalPosition = rectTransform.localPosition;
                    rectTransform.localPosition = new Vector3(settings.From.x, settings.From.y, currentLocalPosition.z);
                    break;
            }
        }
    }
}