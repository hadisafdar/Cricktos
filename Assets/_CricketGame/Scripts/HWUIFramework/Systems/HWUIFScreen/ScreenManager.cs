namespace HyyderWorks.UI.Screen
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class ScreenManager : GenericSingletonPersistent<ScreenManager>
    {
        [SerializeField] private AppScreen[] persistentScreens;
        private static Stack<AppScreen> _screenStack;


        //Events
        public static event Action<string> onScreenAdded;
        public static event Action<string> onScreenRemoved;


        //For editor
        public static AppScreen CurrentSelectedScreen;

        public override void Awake()
        {
            base.Awake();
            _screenStack = new();
            SceneManager.activeSceneChanged += ActiveSceneChanged;
        }



        private void ActiveSceneChanged(Scene arg0, Scene arg1)
        {
            Debug.Log("Clear Scene Stack");
            Clear();
        }

        public static void Clear()
        {
            //Clear all existing screens
            while (_screenStack.Count > 0)
            {
                PopScreen(true);
            }
            _screenStack = new();
            CurrentSelectedScreen = null;
        }

        private void Start()
        {
            if (CurrentSelectedScreen)
            {
                CurrentSelectedScreen.ShowContent(false);
            }
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PopScreen();
            }
        }

        public AppScreen GetPersistentScreen(string screenName)
        {
            AppScreen screen = persistentScreens.FirstOrDefault(s => s.ScreenName.Equals(screenName));
            return screen;
        }

        /// <summary>
        /// Adds a new screen onto the screen stack
        /// </summary>
        /// <param name="screen"></param>
        public static void AddScreen(AppScreen screen)
        {
            Debug.Log("Added " + screen.ScreenName);
            if (_screenStack == null) _screenStack = new();
            _screenStack.Push(screen);
            OnScreenAdded(screen.ScreenName);
        }

        /// <summary>
        /// Replaces the top most screen with the new screen
        /// </summary>
        /// <param name="screen"></param>
        public static void ReplaceScreen(AppScreen screen)
        {
            if (_screenStack == null) _screenStack = new();
            if (_screenStack.Count == 0)
            {
                AddScreen(screen);
                return;
            }
            PopScreen(canEmptyStack: true);
            AddScreen(screen);
            Debug.Log("Replaced with: " + screen.ScreenName);
        }

        /// <summary>
        /// Removes the top most screen
        /// </summary>
        public static void PopScreen(bool canEmptyStack = false)
        {
            if (_screenStack == null) return;
            if (_screenStack.Count == 1 && !canEmptyStack)
            {
                return;
            }
            AppScreen previousScreen = _screenStack.Pop();
            OnScreenRemoved(previousScreen.ScreenName);
            if (_screenStack.Count == 0) return;
            OnScreenAdded(_screenStack.Peek().ScreenName);
        }


        //Since stack only allows first in last out operation we cannot remove a screen of our choice
        //So we first copy the stack into a list and then remove the item. Then we convert the list back into a stack
        public static void RemoveScreen(AppScreen screen)
        {

            if (_screenStack == null || _screenStack.Count == 1)
            {
                return;
            }
            List<AppScreen> screensCopy = _screenStack.ToList();
            if (screensCopy.Contains(screen))
            {
                screensCopy.Remove(screen);
            }
            _screenStack = new Stack<AppScreen>(screensCopy);
            OnScreenRemoved(screen.ScreenName);
            OnScreenAdded(_screenStack.Peek().ScreenName);
        }


        public static void OnScreenAdded(string ScreenName)
        {
            onScreenAdded?.Invoke(ScreenName);
        }
        public static void OnScreenRemoved(string ScreenName)
        {
            onScreenRemoved?.Invoke(ScreenName);
        }

        public static void PopScreens(int amount)
        {
            if (amount >= _screenStack.Count - 1) return;
            for (int i = 0; i < amount; i++)
            {
                PopScreen();
            }
        }

    }


}