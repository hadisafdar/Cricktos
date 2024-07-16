namespace HyyderWorks.UI.Screen
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(ScreenChangerButton))]
    public class ScreenChangerButtonEditor : Editor
    {
        private SerializedProperty _isPopButton;
        private SerializedProperty _replaceScreen;
        private SerializedProperty _popExistingFirst;
        private SerializedProperty _canEmptyStack;
        private SerializedProperty _screen;
        private SerializedProperty _persistentScreenName;

        private void OnEnable()
        {
            _isPopButton = serializedObject.FindProperty("_isPopButton");
            _replaceScreen = serializedObject.FindProperty("_replaceScreen");
            _popExistingFirst = serializedObject.FindProperty("_popExistingFirst");
            _canEmptyStack = serializedObject.FindProperty("_canEmptyStack");
            _screen = serializedObject.FindProperty("_screenToNavigateTo");
            _persistentScreenName = serializedObject.FindProperty("_persistentScreenName");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_isPopButton, new GUIContent("Is Pop Button"));

            if (!_isPopButton.boolValue)
            {
                EditorGUILayout.PropertyField(_replaceScreen, new GUIContent("Replace Existing Screen"));
                EditorGUILayout.PropertyField(_popExistingFirst, new GUIContent("Pop Existing Screen First"));
                EditorGUILayout.PropertyField(_screen);
                EditorGUILayout.PropertyField(_persistentScreenName);
            }
            else
            {
                EditorGUILayout.PropertyField(_canEmptyStack, new GUIContent("Can Empty Stack"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
