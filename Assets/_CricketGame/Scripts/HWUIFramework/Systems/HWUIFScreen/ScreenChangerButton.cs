namespace HyyderWorks.UI.Screen
{
    using UnityEngine;
    using UnityEngine.UI;
    public class ScreenChangerButton : MonoBehaviour
    {

        [Tooltip("Is this button used to pop the current screen")] [SerializeField] private bool _isPopButton;
        public bool IsPopButton
        {
            get
            {
                return _isPopButton;
            }
            set
            {
                _isPopButton = value;
            }
        }
        [Tooltip("Will this button replace current screen")] [SerializeField] private bool _replaceScreen;
        public bool ReplaceScreen
        {
            get
            {
                return _replaceScreen;
            }
            set
            {
                _replaceScreen = value;
            }
        }
        [SerializeField] private AppScreen _screenToNavigateTo;
        [SerializeField] private string _persistentScreenName;
        [Tooltip("Should we pop existing screen before navigating to new screen")] [SerializeField] private bool _popExistingFirst;
        [Tooltip("Can this button empty stack on pop")] [SerializeField] private bool _canEmptyStack;

        public bool PopExisting
        {
            get
            {
                return _popExistingFirst;
            }
            set
            {
                _popExistingFirst = value;
            }
        }
        public bool CanEmptyStack
        {
            get
            {
                return _canEmptyStack;
            }
            set
            {
                _canEmptyStack = value;
            }
        }
        public AppScreen ScreenToNavigateTo
        {
            get
            {
                return _screenToNavigateTo;
            }
            set
            {
                _screenToNavigateTo = value;
            }
        }

        //Internal
        private Button _button;

        private void Awake()
        {

            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {

            _button.onClick.AddListener(_isPopButton ? PopScreen : NavigateToScreen);

        }
        private void OnDisable()
        {

            _button.onClick.RemoveListener(_isPopButton ? PopScreen : NavigateToScreen);

        }


        private void NavigateToScreen()
        {
            if (_popExistingFirst) PopScreen();
            if (_screenToNavigateTo == null)
            {
                _screenToNavigateTo = ScreenManager.Instance.GetPersistentScreen(_persistentScreenName);
                if (_screenToNavigateTo == null)
                {
                    Debug.Log("Cannot Navigate to NULL screen", this);
                    return;
                }
            }
            if (_replaceScreen)
            {
                ScreenManager.ReplaceScreen(_screenToNavigateTo);
            }
            else
            {
                ScreenManager.AddScreen(_screenToNavigateTo);
            }
        }
        private void PopScreen() => ScreenManager.PopScreen(_canEmptyStack);
    }

}