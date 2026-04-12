#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DOTweenUI
{
    [CustomPropertyDrawer(typeof(global::DOTweenUI.DOTweenUIEntry))]
    public class DOTweenUIEntryDrawer : PropertyDrawer
    {
        private const float Space = 2f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            float height = 0f;

            height += EditorGUIUtility.singleLineHeight + Space;
            height += GetHeight(property, "enabled");
            height += GetHeight(property, "id");
            height += GetHeight(property, "trigger");
            height += GetHeight(property, "animationType");
            height += GetHeight(property, "playbackSettings");

            SerializedProperty animationTypeProp = property.FindPropertyRelative("animationType");
            if (animationTypeProp != null)
            {
                DOTweenUIAnimationType animationType = (DOTweenUIAnimationType)animationTypeProp.enumValueIndex;

                switch (animationType)
                {
                    case DOTweenUIAnimationType.Move:
                        height += GetHeight(property, "moveSettings");
                        break;

                    case DOTweenUIAnimationType.Scale:
                        height += GetHeight(property, "scaleSettings");
                        break;
                    case DOTweenUIAnimationType.Rotate:
                        height += GetHeight(property, "rotateSettings");
                        break;
                    case DOTweenUIAnimationType.CanvasGroup:
                        height += GetHeight(property, "canvasGroupSettings");
                        break;
                }
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty enabledProp = property.FindPropertyRelative("enabled");
            SerializedProperty idProp = property.FindPropertyRelative("id");
            SerializedProperty triggerProp = property.FindPropertyRelative("trigger");
            SerializedProperty animationTypeProp = property.FindPropertyRelative("animationType");
            SerializedProperty playbackProp = property.FindPropertyRelative("playbackSettings");
            SerializedProperty moveSettingsProp = property.FindPropertyRelative("moveSettings");
            SerializedProperty scaleSettingsProp = property.FindPropertyRelative("scaleSettings");
            SerializedProperty rotateSettingsProp = property.FindPropertyRelative("rotateSettings");
            SerializedProperty canvasGroupSettingsProp = property.FindPropertyRelative("canvasGroupSettings");

            if (enabledProp == null || idProp == null || triggerProp == null ||
                animationTypeProp == null || playbackProp == null ||
                moveSettingsProp == null || scaleSettingsProp == null || rotateSettingsProp == null
                || canvasGroupSettingsProp == null)
            {
                EditorGUI.LabelField(position, label.text, "DOTweenUIEntryDrawer: property not found");
                return;
            }

            Rect rect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            string title = string.IsNullOrWhiteSpace(idProp.stringValue)
                ? $"Animation ({animationTypeProp.enumDisplayNames[animationTypeProp.enumValueIndex]})"
                : idProp.stringValue;

            property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, title, true);

            if (!property.isExpanded)
            {
                return;
            }

            EditorGUI.indentLevel++;
            rect.y += EditorGUIUtility.singleLineHeight + Space;

            DrawProperty(ref rect, enabledProp);
            DrawProperty(ref rect, idProp);
            DrawProperty(ref rect, triggerProp);
            DrawProperty(ref rect, animationTypeProp);
            DrawProperty(ref rect, playbackProp);

            DOTweenUIAnimationType animationType = (DOTweenUIAnimationType)animationTypeProp.enumValueIndex;

            switch (animationType)
            {
                case DOTweenUIAnimationType.Move:
                    DrawProperty(ref rect, moveSettingsProp);
                    break;

                case DOTweenUIAnimationType.Scale:
                    DrawProperty(ref rect, scaleSettingsProp);
                    break;
                case DOTweenUIAnimationType.Rotate:
                    DrawProperty(ref rect, rotateSettingsProp);
                    break;
                case DOTweenUIAnimationType.CanvasGroup:
                    DrawProperty(ref rect, canvasGroupSettingsProp);
                    break;
            }

            EditorGUI.indentLevel--;
        }

        private static float GetHeight(SerializedProperty root, string relativeName)
        {
            SerializedProperty property = root.FindPropertyRelative(relativeName);

            if (property == null)
            {
                return EditorGUIUtility.singleLineHeight + Space;
            }

            return EditorGUI.GetPropertyHeight(property, true) + Space;
        }

        private static void DrawProperty(ref Rect rect, SerializedProperty property)
        {
            float height = EditorGUI.GetPropertyHeight(property, true);
            rect.height = height;
            EditorGUI.PropertyField(rect, property, true);
            rect.y += height + Space;
        }
    }
}
#endif