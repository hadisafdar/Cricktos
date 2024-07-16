using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HWCard : UIView,IPointerClickHandler
{
    [Header("Components")]
    [SerializeField] private HWText primary;
    [SerializeField] private HWText secondary;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image iconBG;
    
    [SerializeField] private Button actionButton;

    private object data;
    private Action<object> onAction;

    public override void UpdateUI(Component sender, object[] payload)
    {
        
    }

    public void InitializeCard(string primaryText = null, string secondaryText = null, Sprite icon = null, object data = null,string buttonText = null,Action<object> onAction = null)
    {
        UpdateText(primary, primaryText);
        UpdateText(secondary, secondaryText);
        this.data = data;

        SetIcon(icon);
        if (onAction != null)
        {
            if (buttonText != null)
            {
                actionButton.GetComponentInChildren<HWText>().UpdateUI(this, new object[] { buttonText });
            }
            actionButton.onClick.AddListener(OnClick);
            SetOnAction(onAction);
        }
        else
        {
            actionButton.onClick.RemoveAllListeners();
            SetOnAction(null);

        }
    }


    public void SetOnAction(Action<object> onAction)
    {
        this.onAction = onAction;
    }

    public void SetIcon(Sprite icon)
    {
        if (icon != null)
        {
            iconImage.sprite = icon;
            iconBG.gameObject.SetActive(true);
        }
        else
        {
            iconBG.gameObject.SetActive(false);
        }
    }


    private void UpdateText(HWText element, string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            element.UpdateUI(this, new object[] { text });
            element.gameObject.SetActive(true);
        }
        else
        {
            element.gameObject.SetActive(false);
        }
    }


    private void OnClick()
    {
        Debug.Log("On Click");
        onAction?.Invoke(data);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //We still want to invoke the action even if we haven't assigned a button
        if (onAction != null)
        {
            onAction?.Invoke(data);
        }
    }
}
