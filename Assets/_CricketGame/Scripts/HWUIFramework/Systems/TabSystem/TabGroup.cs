using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

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

        [Header("Animation Settings")]
        [SerializeField] private float selectedTabScale = 1.2f;
        [SerializeField] private float animationDuration = 0.3f;
        [SerializeField] private Ease animationTween;


        [Header("Events")]
        public UnityEvent onTabGroupOpenedEvent;
        public UnityEvent onTabGroupClosedEvent;

        [SerializeField] private bool m_Debugging;
        [ShowIf("m_Debugging")]
        [SerializeField] private CurrentTabState m_CurrentTabState;
        [SerializeField, ReadOnly] private TabGroup m_ActiveTabGroup;
        [SerializeField, ReadOnly] private Tab m_ActiveTab;
        
        
        private Tab activeTabButton;
        private List<Tab> tabButtons;
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
            SetTabVisuals(tabButton, tabHoverColor, tabHoverTextColor);
        }

        public void OnTabExit(Tab tabButton)
        {
            ResetTabState();
            if (activeTabButton == null && tabButton != activeTabButton)
            {
                SetTabVisuals(tabButton, tabInactiveColor, tabInactiveTextColor);
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
            SetTabVisuals(tabButton, tabSelectedColor, tabSelectedTextColor);
            AnimateTabSelection(tabButton);

            OnTabSelect?.Invoke(tabButton.TabName);
        }

        private void ShowPage()
        {
            foreach (Tab tabButton in tabButtons)
            {
                if (tabButton.page == null) continue;
                tabButton.ShowPage(tabButton == activeTabButton);
            }
        }

        private void ResetTabState()
        {
            foreach (Tab tabButton in tabButtons)
            {
                if (activeTabButton != null && tabButton.TabName.Equals(activeTabButton.TabName)) continue;
                SetTabVisuals(tabButton, tabInactiveColor, tabInactiveTextColor);
            }
        }

        public void DeselectAll()
        {
            if (tabButtons == null) return;

            foreach (Tab tabButton in tabButtons)
            {
                SetTabVisuals(tabButton, tabInactiveColor, tabInactiveTextColor);
                tabButton.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
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
                SetTabVisuals(tab, currentButtonColor, currentButtonTextColor);
            }
        }

        private void SetTabVisuals(Tab tab, Color backgroundColor, Color textColor)
        {
            if (tab.background) tab.background.color = backgroundColor;
            if (tab.buttonText) tab.SetTextColor(textColor);
        }

        private void AnimateTabSelection(Tab tabButton)
        {
            tabButton.transform.DOScale(selectedTabScale, animationDuration).SetEase(Ease.OutBack);
            foreach (Tab tab in tabButtons)
            {
                if (tab != tabButton)
                {
                    tab.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
                }
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
