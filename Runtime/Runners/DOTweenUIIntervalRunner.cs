using DG.Tweening;

namespace DOTweenUI
{
    public class DOTweenUIIntervalRunner : IDOTweenUIAnimationRunner
    {
        public DOTweenUIAnimationType AnimationType => DOTweenUIAnimationType.Interval;

        public Tween CreateTween(DOTweenUIEntry entry)
        {
            if (entry == null)
                return null;

            return DOVirtual.DelayedCall(entry.IntervalSettings.Interval, () => { });
        }
    }
}