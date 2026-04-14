#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DOTweenUI.Editor
{
    [CustomPropertyDrawer(typeof(DOTweenUIEntry))]
    public class DOTweenUIEntryDrawer : PropertyDrawer
    {
        private const float Space = 2f;
        private const float SectionSpace = 6f;

        private const float HeaderToggleWidth = 22f;
        private const float HeaderToggleGap = 6f;
        private const float HeaderModeWidth = 95f;
        private const float HeaderRightGap = 4f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            float height = 0f;

            height += EditorGUIUtility.singleLineHeight + Space;
            height += GetHeight(property, "id");
            height += GetHeight(property, "trigger");

            SerializedProperty compositionModeProp = property.FindPropertyRelative("compositionMode");
            if (compositionModeProp != null &&
                (DOTweenUICompositionMode)compositionModeProp.enumValueIndex == DOTweenUICompositionMode.Insert)
            {
                height += GetHeight(property, "insertAt");
            }

            height += GetHeight(property, "animationType");

            SerializedProperty animationTypeProp = property.FindPropertyRelative("animationType");
            if (animationTypeProp != null)
            {
                DOTweenUIAnimationType animationType = (DOTweenUIAnimationType)animationTypeProp.enumValueIndex;

                if (animationType != DOTweenUIAnimationType.Interval)
                {
                    height += GetHeight(property, "playbackSettings");
                }

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

                    case DOTweenUIAnimationType.Interval:
                        height += GetHeight(property, "intervalSettings");
                        break;
                }
            }

            SerializedProperty eventsProp = property.FindPropertyRelative("events");
            if (eventsProp != null)
            {
                height += SectionSpace;
                height += EditorGUIUtility.singleLineHeight + Space;

                if (eventsProp.isExpanded)
                {
                    height += GetExpandedChildrenHeight(eventsProp);
                }
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty enabledProp = property.FindPropertyRelative("enabled");
            SerializedProperty idProp = property.FindPropertyRelative("id");
            SerializedProperty triggerProp = property.FindPropertyRelative("trigger");
            SerializedProperty compositionModeProp = property.FindPropertyRelative("compositionMode");
            SerializedProperty insertAtProp = property.FindPropertyRelative("insertAt");
            SerializedProperty animationTypeProp = property.FindPropertyRelative("animationType");
            SerializedProperty playbackProp = property.FindPropertyRelative("playbackSettings");
            SerializedProperty moveSettingsProp = property.FindPropertyRelative("moveSettings");
            SerializedProperty scaleSettingsProp = property.FindPropertyRelative("scaleSettings");
            SerializedProperty rotateSettingsProp = property.FindPropertyRelative("rotateSettings");
            SerializedProperty canvasGroupSettingsProp = property.FindPropertyRelative("canvasGroupSettings");
            SerializedProperty intervalSettingsProp = property.FindPropertyRelative("intervalSettings");
            SerializedProperty eventsProp = property.FindPropertyRelative("events");

            if (enabledProp == null || idProp == null || triggerProp == null ||
                compositionModeProp == null || insertAtProp == null ||
                animationTypeProp == null || playbackProp == null ||
                moveSettingsProp == null || scaleSettingsProp == null ||
                rotateSettingsProp == null || canvasGroupSettingsProp == null ||
                intervalSettingsProp == null || eventsProp == null)
            {
                EditorGUI.LabelField(position, label.text, "DOTweenUIEntryDrawer: property not found");
                return;
            }

            Rect headerRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            Rect toggleRect = new Rect(
                headerRect.x,
                headerRect.y,
                HeaderToggleWidth,
                headerRect.height);

            Rect modeRect = new Rect(
                headerRect.xMax - HeaderModeWidth,
                headerRect.y,
                HeaderModeWidth,
                headerRect.height);

            Rect foldoutRect = new Rect(
                toggleRect.xMax + HeaderToggleGap,
                headerRect.y,
                headerRect.width - HeaderToggleWidth - HeaderToggleGap - HeaderModeWidth - HeaderRightGap,
                headerRect.height);

            Rect centeredToggleRect = new Rect(
                toggleRect.x + 2f,
                toggleRect.y,
                16f,
                toggleRect.height);

            enabledProp.boolValue = EditorGUI.Toggle(centeredToggleRect, enabledProp.boolValue);

            string idText = string.IsNullOrWhiteSpace(idProp.stringValue)
                ? animationTypeProp.enumDisplayNames[animationTypeProp.enumValueIndex]
                : idProp.stringValue;

            string triggerText = triggerProp.enumDisplayNames[triggerProp.enumValueIndex];
            string typeText = animationTypeProp.enumDisplayNames[animationTypeProp.enumValueIndex];

            string title = $"{idText} • {triggerText} • {typeText}";
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, title, true);

            EditorGUI.BeginChangeCheck();
            int newModeIndex = EditorGUI.Popup(modeRect, compositionModeProp.enumValueIndex, compositionModeProp.enumDisplayNames);
            if (EditorGUI.EndChangeCheck())
            {
                compositionModeProp.enumValueIndex = newModeIndex;
                property.serializedObject.ApplyModifiedProperties();
            }

            if (!property.isExpanded)
            {
                return;
            }

            EditorGUI.indentLevel++;

            Rect rect = new Rect(
                position.x,
                position.y + EditorGUIUtility.singleLineHeight + Space,
                position.width,
                EditorGUIUtility.singleLineHeight);

            DrawProperty(ref rect, idProp);
            DrawProperty(ref rect, triggerProp);

            DOTweenUICompositionMode compositionMode = (DOTweenUICompositionMode)compositionModeProp.enumValueIndex;
            if (compositionMode == DOTweenUICompositionMode.Insert)
            {
                DrawProperty(ref rect, insertAtProp);
            }

            DrawProperty(ref rect, animationTypeProp);

            DOTweenUIAnimationType animationType = (DOTweenUIAnimationType)animationTypeProp.enumValueIndex;

            if (animationType != DOTweenUIAnimationType.Interval)
            {
                DrawProperty(ref rect, playbackProp);
            }

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

                case DOTweenUIAnimationType.Interval:
                    DrawProperty(ref rect, intervalSettingsProp);
                    break;
            }

            rect.y += SectionSpace;
            DrawFoldoutProperty(ref rect, eventsProp, "Events");

            if (eventsProp.isExpanded)
            {
                DrawExpandedChildren(ref rect, eventsProp);
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

        private static float GetExpandedChildrenHeight(SerializedProperty property)
        {
            float height = 0f;
            SerializedProperty iterator = property.Copy();
            SerializedProperty end = iterator.GetEndProperty();

            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, end))
            {
                height += EditorGUI.GetPropertyHeight(iterator, true) + Space;
                enterChildren = false;
            }

            return height;
        }

        private static void DrawProperty(ref Rect rect, SerializedProperty property)
        {
            float height = EditorGUI.GetPropertyHeight(property, true);
            rect.height = height;
            EditorGUI.PropertyField(rect, property, true);
            rect.y += height + Space;
        }

        private static void DrawFoldoutProperty(ref Rect rect, SerializedProperty property, string title)
        {
            rect.height = EditorGUIUtility.singleLineHeight;
            property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, title, true);
            rect.y += EditorGUIUtility.singleLineHeight + Space;
        }

        private static void DrawExpandedChildren(ref Rect rect, SerializedProperty parentProperty)
        {
            SerializedProperty iterator = parentProperty.Copy();
            SerializedProperty end = iterator.GetEndProperty();

            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, end))
            {
                float height = EditorGUI.GetPropertyHeight(iterator, true);
                rect.height = height;
                EditorGUI.PropertyField(rect, iterator, true);
                rect.y += height + Space;
                enterChildren = false;
            }
        }
    }
}
#endif