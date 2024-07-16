namespace HyyderWorks.UI.Screen
{
    using UnityEngine;
    using UnityEngine.Events;

    public class AppScreen : MonoBehaviour
    {
        public string ScreenName; // Name of the screen

        // Serialized fields accessible in the Unity Editor
        [SerializeField] private CanvasGroup screenInterface; // Reference to the screen's UI
        [SerializeField] private bool welcomeScreen; // Flag indicating if this is a welcome screen
        [SerializeField] private UnityEvent onScreenOpenedEvent; // Event invoked when the screen is opened
        [SerializeField] private UnityEvent onScreenClosedEvent; // Event invoked when the screen is closed

        public bool WelcomeScreen => welcomeScreen;

        // Subscribe to screen events on Start
        private void Awake()
        {
            ScreenManager.onScreenAdded += EnableScreen;
            ScreenManager.onScreenRemoved += DisableScreen;


        }

        private void Start()
        {
            ShowContent(false);
            // If it's a welcome screen, set it as the current selected screen and add it
            if (welcomeScreen)
            {
                ScreenManager.AddScreen(this);
            }
        }

        // Unsubscribe from screen events on disable
        private void OnDestroy()
        {
            ScreenManager.onScreenAdded -= EnableScreen;
            ScreenManager.onScreenRemoved -= DisableScreen;
        }

        // Method to invoke the onScreenOpenedEvent
        public void OnScreenOpened()
        {
            // If there is an OnOpen event, invoke it
            onScreenOpenedEvent?.Invoke();
        }
        // Method to invoke the onScreenClosedEvent
        public void OnScreenClosed()
        {
            // If there is an OnClose event, invoke it
            onScreenClosedEvent?.Invoke();
        }

        // Callback method to enable the screen based on the provided screenName
        private void EnableScreen(string screenName)
        {
            ShowContent(this.ScreenName.Equals(screenName));
        }
        // Callback method to disable the screen based on the provided screenName
        private void DisableScreen(string screenName)
        {
            if (!ScreenName.Equals(screenName)) return;
            Debug.Log("Disabling" + screenName);
            ShowContent(false);

        }

        // Method to show or hide the screen's content
        public void ShowContent(bool show)
        {

            // Ensure there is a screen interface reference
            if (!screenInterface) return;
            SetScreenInterfaceState(show);
            if (show) OnScreenOpened();
            else OnScreenClosed();

        }


        private void SetScreenInterfaceState(bool visible)
        {
            screenInterface.interactable = visible;
            screenInterface.blocksRaycasts = visible;
            screenInterface.alpha = visible ? 1 : 0;
        }


#if UNITY_EDITOR
        // Automatically rename the GameObject based on the ScreenName for clarity in the editor
        private void OnValidate()
        {

            this.name = ScreenName + "_Screen";
        }


#endif


    }

}