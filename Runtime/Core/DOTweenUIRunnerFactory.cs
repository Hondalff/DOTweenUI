using System;
using System.Collections.Generic;

namespace DOTweenUI
{
    public class DOTweenUIRunnerFactory
    {
        private readonly Dictionary<DOTweenUIAnimationType, IDOTweenUIAnimationRunner> _runners;

        public DOTweenUIRunnerFactory()
        {
            _runners = new Dictionary<DOTweenUIAnimationType, IDOTweenUIAnimationRunner>
            {
                { DOTweenUIAnimationType.Move, new DOTweenUIMoveRunner() },
                { DOTweenUIAnimationType.Scale, new DOTweenUIScaleRunner() },
                { DOTweenUIAnimationType.Rotate, new DOTweenUIRotateRunner() },
                { DOTweenUIAnimationType.CanvasGroup, new DOTweenUICanvasGroupRunner() },
                { DOTweenUIAnimationType.Interval, new DOTweenUIIntervalRunner() },
                
            };
        }

        public IDOTweenUIAnimationRunner Get(DOTweenUIAnimationType animationType)
        {
            if (_runners.TryGetValue(animationType, out IDOTweenUIAnimationRunner runner))
            {
                return runner;
            }

            throw new ArgumentOutOfRangeException(nameof(animationType), animationType, "Runner is not registered.");
        }
    }
}