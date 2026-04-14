using DG.Tweening;
using UnityEngine;

namespace DOTweenUI
{
    public class DOTweenUIRotateRunner : IDOTweenUIAnimationRunner
    {
        public DOTweenUIAnimationType AnimationType => DOTweenUIAnimationType.Rotate;

        public Tween CreateTween(DOTweenUIEntry entry)
        {
            DOTweenUIRotateSettings settings = entry.RotateSettings;
            Transform target = entry.RotateSettings.Transform;
            float duration = entry.PlaybackSettings.Duration;

            Vector3 endValue = ResolveTarget(target, settings);

            return target.DOLocalRotate(endValue, duration);
        }

        private static Vector3 ResolveTarget(Transform target, DOTweenUIRotateSettings settings)
        {
            switch (settings.RotateMode)
            {
                case DOTweenUIRotateMode.FromTo:
                    target.localRotation = Quaternion.Euler(settings.From);
                    return settings.To;

                case DOTweenUIRotateMode.ToAbsolute:
                    return settings.To;

                case DOTweenUIRotateMode.ToRelative:
                    return target.localEulerAngles + settings.To;

                default:
                    return settings.To;
            }
        }
    }
}