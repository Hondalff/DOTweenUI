using System.Collections.Generic;
using DG.Tweening;

namespace DOTweenUI
{
    public class DOTweenUIRuntimeStore
    {
        private readonly Dictionary<object, Tween> activeTweens = new();

        public void Register(object key, Tween tween)
        {
            if (key == null || tween == null)
                return;

            activeTweens[key] = tween;
        }

        public void Kill(object key)
        {
            if (key == null)
                return;

            if (!activeTweens.TryGetValue(key, out Tween tween))
                return;

            if (tween != null && tween.IsActive())
            {
                tween.Kill();
            }
            else
            {
                activeTweens.Remove(key);
            }
        }

        public void Remove(object key)
        {
            if (key == null)
                return;

            activeTweens.Remove(key);
        }

        public void KillAll()
        {
            if (activeTweens.Count == 0)
                return;

            List<Tween> tweens = new(activeTweens.Values);

            for (int i = 0; i < tweens.Count; i++)
            {
                Tween tween = tweens[i];

                if (tween != null && tween.IsActive())
                {
                    tween.Kill();
                }
            }

            activeTweens.Clear();
        }
    }
}