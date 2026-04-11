using System.Collections.Generic;
using DG.Tweening;

namespace DOTweenUI
{
    public class DOTweenUIRuntimeStore
    {
        private readonly Dictionary<DOTweenUIEntry, Tween> activeTweens = new ();
        
        public void Register(DOTweenUIEntry entry, Tween tween)
        {
            activeTweens[entry] = tween;
        }

        public void Kill(DOTweenUIEntry entry)
        {
            if (!activeTweens.TryGetValue(entry, out Tween tween))
                return;

            if (tween != null && tween.IsActive())
            {
                tween.Kill();
            }
            else
            {
                activeTweens.Remove(entry);
            }
        }

        public void Remove(DOTweenUIEntry entry)
        {
            activeTweens.Remove(entry);
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