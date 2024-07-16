using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;




namespace HyyderWorks.UI
{

    public enum CurrentTabState
    {
        Hover,
        Selected,
        Inactive
    }


    public class TabGroup : MonoBehaviour
    {

        [SerializeField] private string tabGroupName;
        [SerializeField] private bool isDefaultTabGroup;


        public event Action<string> OnTabSelect;

        [BoxGroup("Tab Display Setting")]
        [SerializeField] private Color tabHoverColor;
        [BoxGroup("Tab Display Setting")]
        [SerializeField] private Color tabSelectedColor;
        [BoxGroup("Tab Display Setting")]
        [SerializeField] private Color tabInactiveColor;

        [BoxGroup("Tab Text Display Setting")]
        [SerializeField] private Color tabHoverTextColor;
        [BoxGroup("Tab Text Display Setting")]
        [SerializeField] private Color tabSelectedTextColor;
        [BoxGroup("Tab Text Display Setting")]
        [SerializeField] private Color tabInactiveTextColor;

        private Tab activeTabButton;

        private List<Tab> tabButtons;

        [Header("Events")]
        public UnityEvent onTabGroupOpenedEvent;
        public UnityEvent onTabGroupClosedEvent;


        [SerializeField] private bool m_Debugging;
        [ShowIf("m_Debugging")]
        [SerializeField] private CurrentTabState m_CurrentTabState;
        [SerializeField, ReadOnly] private TabGroup m_ActiveTabGroup;
        [SerializeField, ReadOnly] private Tab m_ActiveTab;



        public static Stack<TabGroup> TabGroupsStack;
        public static event Action<bool> EndOfStackEvent;



        private void Awake()
        {

            if (isDefaultTabGroup)
            {
                AddToStack();
            }
        }
       

        public void Subscribe(Tab tabButton)
        {
            if (tabButtons == null)
            {
                tabButtons = new List<Tab>();
            }
            tabButtons.Add(tabButton);


        }


        public void OnTabEnter(Tab tabButton)
        {
            ResetTabState();
            if (activeTabButton != null && tabButton == activeTabButton) return;
            if (tabButton.tabGFX) tabButton.tabGFX.color = tabHoverColor;
            if (tabButton.buttonText) tabButton.SetTextColor(tabHoverTextColor);
            if (tabButton.icon) tabButton.icon.color = tabHoverTextColor;
        }

        public void OnTabExit(Tab tabButton)
        {
            ResetTabState();
            if (activeTabButton == null && tabButton != activeTabButton)
            {

                if (tabButton.tabGFX) tabButton.tabGFX.color = tabInactiveColor;
                if (tabButton.buttonText) tabButton.SetTextColor(tabInactiveTextColor);
                if (tabButton.icon) tabButton.icon.color = tabInactiveTextColor;
            }
        }

        public void OnTabSelected(Tab tabButton)
        {
            if (activeTabButton != null)
            {
                activeTabButton.Deselect();
            }

            activeTabButton = tabButton;
            activeTabButton.Select();
            ShowPage();

            ResetTabState();
            if (tabButton.tabGFX) tabButton.tabGFX.color = tabSelectedColor;
            if (tabButton.buttonText) tabButton.SetTextColor(tabSelectedTextColor);
            if (tabButton.icon) tabButton.icon.color = tabSelectedTextColor;
            OnTabSelect?.Invoke(tabButton.TabName);
        }
        void ShowPage()
        {
            foreach (Tab tabButton in tabButtons)
            {
                if (tabButton.page == null) continue;
                if (tabButton == activeTabButton)
                {

                    tabButton.ShowPage(true);
                }
                else
                {
                    tabButton.ShowPage(false);
                }
            }
        }




        void ResetTabState()
        {

            foreach (Tab tabButton in tabButtons)
            {
                if (activeTabButton != null && tabButton.TabName.Equals(activeTabButton.TabName)) continue;

                if (tabButton.tabGFX) tabButton.tabGFX.color = tabInactiveColor;
                if (tabButton.buttonText) tabButton.SetTextColor(tabInactiveTextColor);
                if (tabButton.icon) tabButton.icon.color = tabInactiveTextColor;
            }

        }


        public void DeselectAll()
        {
            if (tabButtons == null) return;

            foreach (Tab tabButton in tabButtons)
            {

                if (tabButton.tabGFX) tabButton.tabGFX.color = tabInactiveColor;
                if (tabButton.buttonText) tabButton.SetTextColor(tabInactiveTextColor);
                if (tabButton.icon) tabButton.icon.color = tabInactiveTextColor;
            }
            activeTabButton = null;
        }

        public void AddToStack()
        {
            if (TabGroupsStack == null)
                TabGroupsStack = new Stack<TabGroup>();

            if (!TabGroupsStack.Contains(this))
            {
                Debug.Log("Added Tab Group" + this.name);
                TabGroupsStack.Push(this);
                onTabGroupOpenedEvent?.Invoke();
                EndOfStackEvent?.Invoke(false);
            }
        }


        public void CloseAll()
        {
            ResetActiveTabButton(this);
            PreviousState();
        }

        public static void PreviousState()
        {
            if (TabGroupsStack.Count == 0)
            {
                Debug.Log("Tab Groups Stack is Empty");
                return;
            }

            TabGroup ActiveTabGroup = TabGroupsStack.Peek();
            if (ActiveTabGroup.activeTabButton != null)
            {
                ResetActiveTabButton(ActiveTabGroup);
                EndOfStackEvent?.Invoke(false);
            }
            else
            {
                if (TabGroupsStack.Count == 1 && ActiveTabGroup.activeTabButton == null)
                {
                    EndOfStackEvent?.Invoke(true);
                    return; //We want to keep the default tab group

                }
                ActiveTabGroup.onTabGroupClosedEvent?.Invoke();
                TabGroupsStack.Pop();
                ActiveTabGroup = TabGroupsStack.Peek();
                ActiveTabGroup.onTabGroupOpenedEvent?.Invoke();
                EndOfStackEvent?.Invoke(false);
            }
        }

        private static void ResetActiveTabButton(TabGroup activeTabGroup)
        {
            activeTabGroup.activeTabButton?.Deselect();
            activeTabGroup.activeTabButton = null;
            activeTabGroup.ShowPage();
            activeTabGroup.ResetTabState();
        }

        private void SetDefaultTabState()
        {
            Tab[] tabs = GetComponentsInChildren<Tab>();
            Color currentButtonColor = Color.black;
            Color currentButtonTextColor = Color.black;
            switch (m_CurrentTabState)
            {
                case CurrentTabState.Hover:
                    currentButtonColor = tabHoverColor;
                    currentButtonTextColor = tabHoverTextColor;
                    break;
                case CurrentTabState.Selected:
                    currentButtonColor = tabSelectedColor;
                    currentButtonTextColor = tabSelectedTextColor;
                    break;
                case CurrentTabState.Inactive:
                    currentButtonColor = tabInactiveColor;
                    currentButtonTextColor = tabInactiveTextColor;
                    break;
                default:
                    break;
            }
            foreach (Tab tab in tabs)
            {
                if (tab.tabGFX) tab.tabGFX.color = currentButtonColor;
                if (tab.buttonText) tab.SetTextColor(currentButtonTextColor);
                if (tab.icon) tab.icon.color = currentButtonTextColor;
            }
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            this.name = tabGroupName + "[Tab Group]";
            if (m_Debugging)
                SetDefaultTabState();

        }
#endif

    }
}