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
        [SerializeField] private DOTweenUIAnimationType animationType = DOTweenUIAnimationType.Move;
        [SerializeField] private DOTweenUIPlaybackSettings playbackSettings = new();
        [SerializeField] private DOTweenUIMoveSettings moveSettings = new();
        [SerializeField] private DOTweenUIScaleSettings scaleSettings = new();
        [SerializeField] private DOTweenUIRotateSettings rotateSettings = new();
        [SerializeField] private DOTweenUICanvasGroupSettings canvasGroupSettings = new();
        
        [SerializeField] private DOTweenUIEvents events = new();

        public string Id => id;
        public bool Enabled => enabled;
        public DOTweenUITrigger Trigger => trigger;
        public DOTweenUIAnimationType AnimationType => animationType;
        public DOTweenUIPlaybackSettings PlaybackSettings => playbackSettings;
        public DOTweenUIMoveSettings MoveSettings => moveSettings;
        public DOTweenUIScaleSettings ScaleSettings => scaleSettings;
        public DOTweenUIRotateSettings RotateSettings => rotateSettings;
        public DOTweenUICanvasGroupSettings CanvasGroupSettings => canvasGroupSettings;
        public DOTweenUIEvents Events => events;
    }
}