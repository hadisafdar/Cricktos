using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
using Sirenix.OdinInspector;
using System.Collections;

namespace HyyderWorks.UI
{
    public class Tab : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {

        [SerializeField] private string tabName;
        [SerializeField] private float delay;
        [SerializeField] private TabGroup tabGroup;

        [BoxGroup("UI Settings")]
        public HWText buttonText;
        [BoxGroup("UI Settings")]
        public Image icon;
        [BoxGroup("UI Settings")]
        public Image tabGFX;



        [BoxGroup("Page Setting")]
        [SerializeField] private bool m_UsePage;
        [BoxGroup("Page Setting")]
        [ShowIf("m_UsePage", true)]
        [BoxGroup("Page Setting")]
        public TabPage page;
        [BoxGroup("Page Setting")]
        [ShowIf("m_UsePage")]
        [SerializeField] private bool isStartTab;


        [BoxGroup("Events")]
        public UnityEvent onTabSelected;
        [BoxGroup("Events")]
        public UnityEvent onTabDeselected;


        public bool IsStartTab => isStartTab;
        public string TabName => tabName;
        // Start is called before the first frame update
     
        private void Start()
        {
            if (tabGroup == null)
            {
                Debug.LogError($"Tab Button {tabName} does not have any Tab Group parent.");
            }
            else
            {
                tabGroup.Subscribe(this);
            }

            ShowPage(isStartTab);

            if (isStartTab)
            {
                Select();
            }
            SetText();

        }

        [ContextMenu("Set Text")]
        private void SetText()
        {
            if (buttonText) buttonText.SetText(tabName);
            this.transform.name = "TabBTN[" + tabName + "]";
        }

        public void SetTextColor(Color color)
        {
            buttonText.SetColor(color);
        }



        public void OnPointerClick(PointerEventData eventData)
        {
            tabGroup.OnTabSelected(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tabGroup.OnTabEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tabGroup.OnTabExit(this);
        }



        public void Select()
        {
            onTabSelected?.Invoke();
        }
        public void Deselect()
        {
            onTabDeselected?.Invoke();
        }

        public void ShowPage(bool show)
        {
            if (page == null || !m_UsePage) return;
            if (show)
            {
                page.Show();
            }
            else
            {
                // If the GameObject is inactive, activate it temporarily
                bool wasActive = gameObject.activeSelf;
                if (!wasActive)
                {
                    gameObject.SetActive(true);
                }

                StopAllCoroutines();
                StartCoroutine(DoHide());

                // Deactivate the GameObject again if it was originally inactive
                if (!wasActive)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        private IEnumerator DoHide()
        {
            yield return new WaitForSeconds(delay);
            page.Hide();
        }


        private void OnValidate()
        {
            this.name = tabName + "[Tab Button]";
            if (buttonText != null)
            {
                buttonText.SetText(tabName);
            }
            else
            {
                buttonText = GetComponentInChildren<HWText>();
            }

            if (page)
            {
                page.name = TabName + "[Page]";
            }

        }

    }

}