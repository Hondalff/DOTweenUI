using System;
using UnityEngine;

namespace DOTweenUI
{
    [Serializable]
    public class DOTweenUIEntry
    {
        [SerializeField] private string id = "Animation";
        [SerializeField] private bool enabled = true;
        [SerializeField] private DOTweenUITrigger trigger = DOTweenUITrigger.OnEnable;

        [SerializeField] private DOTweenUICompositionMode compositionMode = DOTweenUICompositionMode.Append;
        [SerializeField, Min(0f)] private float insertAt;

        [SerializeField] private DOTweenUIAnimationType animationType = DOTweenUIAnimationType.Move;
        [SerializeField] private DOTweenUIPlaybackSettings playbackSettings = new();
        [SerializeField] private DOTweenUIMoveSettings moveSettings = new();
        [SerializeField] private DOTweenUIScaleSettings scaleSettings = new();
        [SerializeField] private DOTweenUIRotateSettings rotateSettings = new();
        [SerializeField] private DOTweenUICanvasGroupSettings canvasGroupSettings = new();
        [SerializeField] private DOTweenUIIntervalSettings intervalSettings = new();

        [SerializeField] private DOTweenUIEvents events = new();

        public string Id => id;
        public bool Enabled => enabled;
        public DOTweenUITrigger Trigger => trigger;

        public DOTweenUICompositionMode CompositionMode => compositionMode;
        public float InsertAt => insertAt;

        public DOTweenUIAnimationType AnimationType => animationType;
        public DOTweenUIPlaybackSettings PlaybackSettings => playbackSettings;
        public DOTweenUIMoveSettings MoveSettings => moveSettings;
        public DOTweenUIScaleSettings ScaleSettings => scaleSettings;
        public DOTweenUIRotateSettings RotateSettings => rotateSettings;
        public DOTweenUICanvasGroupSettings CanvasGroupSettings => canvasGroupSettings;
        public DOTweenUIIntervalSettings IntervalSettings => intervalSettings;
        public DOTweenUIEvents Events => events;
    }
}