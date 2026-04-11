using DG.Tweening;

namespace DOTweenUI
{
    public interface IDOTweenUIAnimationRunner
    {
        DOTweenUIAnimationType AnimationType { get; }

        Tween CreateTween(DOTweenUI ui, DOTweenUIEntry entry);
    }
}