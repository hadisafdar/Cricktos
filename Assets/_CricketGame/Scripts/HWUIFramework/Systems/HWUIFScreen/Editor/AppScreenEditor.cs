namespace HyyderWorks.UI.Screen
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(AppScreen))]
    public class AppScreenEditor : Editor
    {
        AppScreen appScreen;
        private bool _isAlreadySelectedOnce;


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (Application.isPlaying) return;
            appScreen = target as AppScreen;
            if (Selection.activeGameObject.name == $"{appScreen.ScreenName}_Screen")
            {
                if (ScreenManager.CurrentSelectedScreen != null) ScreenManager.CurrentSelectedScreen.ShowContent(false);
                ScreenManager.CurrentSelectedScreen = appScreen;
                appScreen.ShowContent(true);
            }


        }




    }
}