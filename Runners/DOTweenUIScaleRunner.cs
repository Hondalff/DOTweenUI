using DG.Tweening;
using DOTweenUI;
using UnityEngine;

namespace DOTweenUI.Runners
{
    public class DOTweenUIScaleRunner : IDOTweenUIAnimationRunner
    {
        public DOTweenUIAnimationType AnimationType => DOTweenUIAnimationType.Scale;

        public Tween CreateTween(DOTweenUI ui, DOTweenUIEntry entry)
        {
            DOTweenUIScaleSettings settings = entry.ScaleSettings;
            Transform target = ui.transform;
            float duration = entry.PlaybackSettings.Duration;

            Vector3 endValue = ResolveTarget(target, settings);

            return target.DOScale(endValue, duration);
        }

        private static Vector3 ResolveTarget(Transform target, DOTweenUIScaleSettings settings)
        {
            switch (settings.ScaleMode)
            {
                case DOTweenUIScaleMode.FromTo:
                    target.localScale = settings.From;
                    return settings.To;

                case DOTweenUIScaleMode.ToAbsolute:
                    return settings.To;

                case DOTweenUIScaleMode.MultiplyCurrent:
                    return Vector3.Scale(target.localScale, settings.To);

                default:
                    return settings.To;
            }
        }
    }
}