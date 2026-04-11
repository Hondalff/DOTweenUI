using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DOTweenUI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class DOTweenUI : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerClickHandler
    {
        [SerializeField] private bool playOnStart;
        [SerializeField] private bool killTweensOnDisable = true;
        [SerializeField] private bool debugLogs;
        [SerializeField] private List<DOTweenUIEntry> animations = new();

        private RectTransform rectTransform;
        private DOTweenUIRuntimeStore runtimeStore;
        private DOTweenUIRunnerFactory runnerFactory;

        public RectTransform RectTransform => rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            runtimeStore = new DOTweenUIRuntimeStore();
            runnerFactory = new DOTweenUIRunnerFactory();
        }

        private void Start()
        {
            if (playOnStart)
            {
                Play(DOTweenUITrigger.OnStart);
            }
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
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }

            if (debugLogs)
            {
                Debug.Log($"[{name}] Play trigger: {trigger}", this);
            }

            for (int i = 0; i < animations.Count; i++)
            {
                DOTweenUIEntry entry = animations[i];

                if (entry == null || !entry.Enabled)
                    continue;

                if (entry.Trigger != trigger)
                    continue;

                PlayEntry(entry, i);
            }
        }

        public void Stop(DOTweenUITrigger trigger)
        {
            for (int i = 0; i < animations.Count; i++)
            {
                DOTweenUIEntry entry = animations[i];

                if (entry == null)
                    continue;

                if (entry.Trigger != trigger)
                    continue;

                runtimeStore.Kill(entry);
            }
        }

        public void StopAll()
        {
            runtimeStore.KillAll();
        }

        private void PlayEntry(DOTweenUIEntry entry, int index)
        {
            DOTweenUIPlaybackSettings playback = entry.PlaybackSettings;

            if (playback.KillPreviousTweenBeforePlay)
            {
                runtimeStore.Kill(entry);
            }

            IDOTweenUIAnimationRunner runner = runnerFactory.Get(entry.AnimationType);
            Tween tween = runner.CreateTween(this, entry);

            if (tween == null)
            {
                if (debugLogs)
                {
                    Debug.LogWarning($"[{name}] Tween was not created. Entry = {entry.Id}, Index = {index}", this);
                }

                return;
            }

            ApplyPlaybackSettings(tween, playback);

            tween.OnPlay(() =>
            {
                if (debugLogs)
                {
                    Debug.Log($"[{name}] Tween started. Entry = {entry.Id}, Index = {index}", this);
                }
            });

            tween.OnComplete(() =>
            {
                if (debugLogs)
                {
                    Debug.Log($"[{name}] Tween completed. Entry = {entry.Id}, Index = {index}", this);
                }
            });

            tween.OnKill(() =>
            {
                runtimeStore.Remove(entry);

                if (debugLogs)
                {
                    Debug.Log($"[{name}] Tween killed. Entry = {entry.Id}, Index = {index}", this);
                }
            });

            runtimeStore.Register(entry, tween);
        }

        private static void ApplyPlaybackSettings(Tween tween, DOTweenUIPlaybackSettings playback)
        {
            int loops = playback.Loops == 0 ? 1 : playback.Loops;

            tween.SetDelay(playback.Delay)
                .SetEase(playback.Ease)
                .SetLoops(loops, playback.LoopType)
                .SetAutoKill(playback.AutoKill)
                .SetUpdate(playback.UpdateMode == DOTweenUIAnimationUpdate.Unscaled);
        }
    }
}