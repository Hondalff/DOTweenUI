using DG.Tweening;

namespace DOTweenUI
{
    public interface IDOTweenUIAnimationRunner
    {
        DOTweenUIAnimationType AnimationType { get; }

        Tween CreateTween(DOTweenUIEntry entry);
    }
}