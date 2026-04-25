using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DOTweenUI
{
    [DisallowMultipleComponent]
    public class DOTweenUIAnimator : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerClickHandler
    {
        [SerializeField] private bool killTweensOnDisable = true;
        [SerializeField] private bool debugLogs;
        [SerializeField] private List<DOTweenUIEntry> animations = new();

        private DOTweenUIRuntimeStore runtimeStore;
        private DOTweenUIRunnerFactory runnerFactory;

        public IReadOnlyList<DOTweenUIEntry> Animations => animations;

        private void Awake()
        {
            EnsureInitialized();
        }

        private void Start()
        {
            Play(DOTweenUITrigger.OnStart);
        }

        private void OnEnable()
        {
            Play(DOTweenUITrigger.OnEnable);
        }

        private void OnDisable()
        {
            if (killTweensOnDisable)
            {
                runtimeStore?.KillAll();
                return;
            }

            Play(DOTweenUITrigger.OnDisable);
        }

        private void OnDestroy()
        {
            runtimeStore?.KillAll();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Play(DOTweenUITrigger.PointerEnter);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Play(DOTweenUITrigger.PointerExit);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Play(DOTweenUITrigger.PointerDown);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Play(DOTweenUITrigger.PointerUp);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Play(DOTweenUITrigger.Click);
        }

        [ContextMenu("Play Manual")]
        public void PlayManual()
        {
            Play(DOTweenUITrigger.Manual);
        }

        public void Play(DOTweenUITrigger trigger)
        {
            EnsureInitialized();

            if (debugLogs)
            {
                Debug.Log($"[{name}] Play trigger: {trigger}", this);
            }

            runtimeStore.Kill(trigger);

            Sequence sequence = DOTween.Sequence();
            bool hasTweens = false;

            for (int i = 0; i < animations.Count; i++)
            {
                DOTweenUIEntry entry = animations[i];

                if (entry == null || !entry.Enabled)
                    continue;

                if (entry.Trigger != trigger)
                    continue;

                Tween tween = CreateConfiguredTween(entry, i);
                if (tween == null)
                    continue;

                hasTweens = true;

                switch (entry.CompositionMode)
                {
                    case DOTweenUICompositionMode.Append:
                        sequence.Append(tween);
                        break;

                    case DOTweenUICompositionMode.Join:
                        sequence.Join(tween);
                        break;
                }
            }

            if (!hasTweens)
            {
                sequence.Kill();
                return;
            }

            sequence.OnKill(() =>
            {
                runtimeStore.Remove(trigger);

                if (debugLogs)
                {
                    Debug.Log($"[{name}] Sequence killed. Trigger = {trigger}", this);
                }
            });

            runtimeStore.Register(trigger, sequence);
        }

        public void Stop(DOTweenUITrigger trigger)
        {
            runtimeStore.Kill(trigger);
        }

        public void StopAll()
        {
            runtimeStore.KillAll();
        }

        public Tween CreatePreviewTween(DOTweenUIEntry entry)
        {
            if (entry == null || !entry.Enabled)
                return null;

            EnsureInitialized();

            Tween tween = CreateConfiguredTween(entry, -1, false);
            return tween;
        }

        public Tween CreatePreviewSequence(DOTweenUITrigger trigger)
        {
            if (animations == null || animations.Count == 0)
                return null;

            EnsureInitialized();

            Sequence sequence = DOTween.Sequence();
            bool hasTweens = false;

            for (int i = 0; i < animations.Count; i++)
            {
                DOTweenUIEntry entry = animations[i];

                if (entry == null || !entry.Enabled)
                    continue;

                if (entry.Trigger != trigger)
                    continue;

                Tween tween = CreateConfiguredTween(entry, i, false);
                if (tween == null)
                    continue;

                hasTweens = true;

                switch (entry.CompositionMode)
                {
                    case DOTweenUICompositionMode.Append:
                        sequence.Append(tween);
                        break;

                    case DOTweenUICompositionMode.Join:
                        sequence.Join(tween);
                        break;
                }
            }

            if (!hasTweens)
            {
                sequence.Kill();
                return null;
            }

            return sequence;
        }

        private Tween CreateConfiguredTween(DOTweenUIEntry entry, int index, bool invokeEvents = true)
        {
            if (entry == null)
                return null;

            IDOTweenUIAnimationRunner runner = runnerFactory.Get(entry.AnimationType);
            Tween tween = runner.CreateTween(entry);

            if (tween == null)
            {
                if (debugLogs && index >= 0)
                {
                    Debug.LogWarning($"[{name}] Tween was not created. Entry = {entry.Id}, Index = {index}", this);
                }

                return null;
            }

            ApplyPlaybackSettings(tween, entry.PlaybackSettings);

            if (invokeEvents)
            {
                AppendOnPlay(tween, () =>
                {
                    entry.Events.OnPlay?.Invoke();

                    if (debugLogs && index >= 0)
                    {
                        Debug.Log($"[{name}] Tween started. Entry = {entry.Id}, Index = {index}", this);
                    }
                });

                AppendOnComplete(tween, () =>
                {
                    entry.Events.OnComplete?.Invoke();

                    if (debugLogs && index >= 0)
                    {
                        Debug.Log($"[{name}] Tween completed. Entry = {entry.Id}, Index = {index}", this);
                    }
                });

                AppendOnKill(tween, () =>
                {
                    entry.Events.OnKill?.Invoke();

                    if (debugLogs && index >= 0)
                    {
                        Debug.Log($"[{name}] Tween killed. Entry = {entry.Id}, Index = {index}", this);
                    }
                });
            }

            return tween;
        }

        private void EnsureInitialized()
        {
            runtimeStore ??= new DOTweenUIRuntimeStore();
            runnerFactory ??= new DOTweenUIRunnerFactory();
        }

        private static void ApplyPlaybackSettings(Tween tween, DOTweenUIPlaybackSettings playback)
        {
            int loops = playback.Loops == 0 ? 1 : playback.Loops;
            
            if (loops < 0)
                loops = int.MaxValue;
            
            tween.SetDelay(playback.Delay)
                .SetEase(playback.Ease)
                .SetLoops(loops, playback.LoopType)
                .SetAutoKill(playback.AutoKill)
                .SetUpdate(playback.UpdateMode == DOTweenUIAnimationUpdate.Unscaled);
        }

        private static void AppendOnPlay(Tween tween, TweenCallback callback)
        {
            TweenCallback previous = tween.onPlay;
            tween.onPlay = () =>
            {
                previous?.Invoke();
                callback?.Invoke();
            };
        }

        private static void AppendOnComplete(Tween tween, TweenCallback callback)
        {
            TweenCallback previous = tween.onComplete;
            tween.onComplete = () =>
            {
                previous?.Invoke();
                callback?.Invoke();
            };
        }

        private static void AppendOnKill(Tween tween, TweenCallback callback)
        {
            TweenCallback previous = tween.onKill;
            tween.onKill = () =>
            {
                previous?.Invoke();
                callback?.Invoke();
            };
        }
    }
}