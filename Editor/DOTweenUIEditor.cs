#if UNITY_EDITOR
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace DOTweenUI.Editor
{
    [CustomEditor(typeof(DOTweenUI))]
    public class DOTweenUIEditor : UnityEditor.Editor
    {
        private static readonly List<Tween> ActivePreviewTweens = new();
        private static readonly string PreviewTweenId = "DOTweenUI_EditorPreview";

        private static PreviewSnapshot currentSnapshot;
        private static bool isPreviewing;
        private static double lastEditorTime;
        private static bool dotweenEditorInitialized;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space(10f);
            EditorGUILayout.LabelField("Preview", EditorStyles.boldLabel);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Manual"))
                    StartPreview((DOTweenUI)target, DOTweenUITrigger.Manual);

                if (GUILayout.Button("On Enable"))
                    StartPreview((DOTweenUI)target, DOTweenUITrigger.OnEnable);

                if (GUILayout.Button("On Disable"))
                    StartPreview((DOTweenUI)target, DOTweenUITrigger.OnDisable);

                if (GUILayout.Button("On Start"))
                    StartPreview((DOTweenUI)target, DOTweenUITrigger.OnStart);
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Pointer Enter"))
                    StartPreview((DOTweenUI)target, DOTweenUITrigger.PointerEnter);

                if (GUILayout.Button("Pointer Exit"))
                    StartPreview((DOTweenUI)target, DOTweenUITrigger.PointerExit);

                if (GUILayout.Button("Pointer Down"))
                    StartPreview((DOTweenUI)target, DOTweenUITrigger.PointerDown);

                if (GUILayout.Button("Pointer Up"))
                    StartPreview((DOTweenUI)target, DOTweenUITrigger.PointerUp);

                if (GUILayout.Button("Click"))
                    StartPreview((DOTweenUI)target, DOTweenUITrigger.Click);
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUI.backgroundColor = new Color(1f, 0.6f, 0.6f);

                if (GUILayout.Button("Stop Preview"))
                {
                    StopPreview(restoreState: true);
                }

                GUI.backgroundColor = Color.white;
            }
        }

        private static void StartPreview(DOTweenUI component, DOTweenUITrigger trigger)
        {
            if (component == null)
                return;

            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            StopPreview(restoreState: true);

            if (!dotweenEditorInitialized)
            {
                DOTween.Init();
                dotweenEditorInitialized = true;
            }

            currentSnapshot = PreviewSnapshot.Capture(component);
            ActivePreviewTweens.Clear();

            IReadOnlyList<DOTweenUIEntry> entries = component.Animations;
            for (int i = 0; i < entries.Count; i++)
            {
                DOTweenUIEntry entry = entries[i];

                if (entry == null || !entry.Enabled)
                    continue;

                if (entry.Trigger != trigger)
                    continue;

                Tween tween = component.CreatePreviewTween(entry);

                if (tween == null)
                    continue;

                tween.SetId(PreviewTweenId);
                tween.SetUpdate(UpdateType.Manual, true);
                tween.SetAutoKill(true);

                ActivePreviewTweens.Add(tween);
            }

            if (ActivePreviewTweens.Count == 0)
            {
                currentSnapshot?.Restore();
                currentSnapshot = null;
                return;
            }

            lastEditorTime = EditorApplication.timeSinceStartup;

            EditorApplication.update -= UpdatePreview;
            EditorApplication.update += UpdatePreview;

            isPreviewing = true;

            SceneView.RepaintAll();
        }

        private static void UpdatePreview()
        {
            if (!isPreviewing)
                return;

            double now = EditorApplication.timeSinceStartup;
            float deltaTime = (float)(now - lastEditorTime);
            lastEditorTime = now;

            if (deltaTime < 0f)
                deltaTime = 0f;

            DOTween.ManualUpdate(deltaTime, deltaTime);

            bool hasActiveTweens = false;

            for (int i = 0; i < ActivePreviewTweens.Count; i++)
            {
                Tween tween = ActivePreviewTweens[i];

                if (tween != null && tween.IsActive() && !tween.IsComplete())
                {
                    hasActiveTweens = true;
                    break;
                }
            }

            SceneView.RepaintAll();

            if (!hasActiveTweens)
            {
                StopPreview(restoreState: true);
            }
        }

        private static void StopPreview(bool restoreState)
        {
            EditorApplication.update -= UpdatePreview;
            isPreviewing = false;

            DOTween.Kill(PreviewTweenId);

            for (int i = 0; i < ActivePreviewTweens.Count; i++)
            {
                Tween tween = ActivePreviewTweens[i];

                if (tween != null && tween.IsActive())
                {
                    tween.Kill();
                }
            }

            ActivePreviewTweens.Clear();

            if (restoreState)
            {
                currentSnapshot?.Restore();
            }

            currentSnapshot = null;
            SceneView.RepaintAll();
        }

        private sealed class PreviewSnapshot
        {
            private readonly Dictionary<int, RectTransformState> rectStates = new();
            private readonly Dictionary<int, TransformState> transformStates = new();
            private readonly Dictionary<int, CanvasGroupState> canvasGroupStates = new();

            public static PreviewSnapshot Capture(DOTweenUI component)
            {
                PreviewSnapshot snapshot = new PreviewSnapshot();

                IReadOnlyList<DOTweenUIEntry> entries = component.Animations;
                for (int i = 0; i < entries.Count; i++)
                {
                    DOTweenUIEntry entry = entries[i];
                    if (entry == null)
                        continue;

                    CaptureEntry(snapshot, entry);
                }

                return snapshot;
            }

            public void Restore()
            {
                foreach (RectTransformState state in rectStates.Values)
                {
                    state.Restore();
                }

                foreach (TransformState state in transformStates.Values)
                {
                    state.Restore();
                }

                foreach (CanvasGroupState state in canvasGroupStates.Values)
                {
                    state.Restore();
                }
            }

            private static void CaptureEntry(PreviewSnapshot snapshot, DOTweenUIEntry entry)
            {
                if (entry.MoveSettings != null && entry.MoveSettings.RectTransform != null)
                {
                    RectTransform rectTransform = entry.MoveSettings.RectTransform;
                    int id = rectTransform.GetInstanceID();

                    if (!snapshot.rectStates.ContainsKey(id))
                    {
                        snapshot.rectStates.Add(id, new RectTransformState(rectTransform));
                    }
                }

                if (entry.ScaleSettings != null && entry.ScaleSettings.Transform != null)
                {
                    Transform transform = entry.ScaleSettings.Transform;
                    int id = transform.GetInstanceID();

                    if (!snapshot.transformStates.ContainsKey(id))
                    {
                        snapshot.transformStates.Add(id, new TransformState(transform));
                    }
                }

                if (entry.RotateSettings != null && entry.RotateSettings.Transform != null)
                {
                    Transform transform = entry.RotateSettings.Transform;
                    int id = transform.GetInstanceID();

                    if (!snapshot.transformStates.ContainsKey(id))
                    {
                        snapshot.transformStates.Add(id, new TransformState(transform));
                    }
                }

                if (entry.CanvasGroupSettings != null && entry.CanvasGroupSettings.CanvasGroup != null)
                {
                    CanvasGroup canvasGroup = entry.CanvasGroupSettings.CanvasGroup;
                    int id = canvasGroup.GetInstanceID();

                    if (!snapshot.canvasGroupStates.ContainsKey(id))
                    {
                        snapshot.canvasGroupStates.Add(id, new CanvasGroupState(canvasGroup));
                    }
                }
            }
        }

        private readonly struct RectTransformState
        {
            private readonly RectTransform target;
            private readonly Vector2 anchoredPosition;
            private readonly Vector3 localPosition;
            private readonly Vector3 localScale;
            private readonly Quaternion localRotation;

            public RectTransformState(RectTransform target)
            {
                this.target = target;
                anchoredPosition = target.anchoredPosition;
                localPosition = target.localPosition;
                localScale = target.localScale;
                localRotation = target.localRotation;
            }

            public void Restore()
            {
                if (target == null)
                    return;

                target.anchoredPosition = anchoredPosition;
                target.localPosition = localPosition;
                target.localScale = localScale;
                target.localRotation = localRotation;

                EditorUtility.SetDirty(target);
            }
        }

        private readonly struct TransformState
        {
            private readonly Transform target;
            private readonly Vector3 localPosition;
            private readonly Vector3 localScale;
            private readonly Quaternion localRotation;

            public TransformState(Transform target)
            {
                this.target = target;
                localPosition = target.localPosition;
                localScale = target.localScale;
                localRotation = target.localRotation;
            }

            public void Restore()
            {
                if (target == null)
                    return;

                target.localPosition = localPosition;
                target.localScale = localScale;
                target.localRotation = localRotation;

                EditorUtility.SetDirty(target);
            }
        }

        private readonly struct CanvasGroupState
        {
            private readonly CanvasGroup target;
            private readonly float alpha;
            private readonly bool interactable;
            private readonly bool blocksRaycasts;

            public CanvasGroupState(CanvasGroup target)
            {
                this.target = target;
                alpha = target.alpha;
                interactable = target.interactable;
                blocksRaycasts = target.blocksRaycasts;
            }

            public void Restore()
            {
                if (target == null)
                    return;

                target.alpha = alpha;
                target.interactable = interactable;
                target.blocksRaycasts = blocksRaycasts;

                EditorUtility.SetDirty(target);
            }
        }
    }
}
#endif