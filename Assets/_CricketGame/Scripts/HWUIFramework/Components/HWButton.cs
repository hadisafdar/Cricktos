using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HWButton : UIView,IPointerClickHandler
{
    [Header("Sub Components")]
    [SerializeField] private HWText buttonText;
    public Action OnClickAction;


    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickAction?.Invoke();
    }

    public void SetOnClick(Action onClick)
    {
        OnClickAction = onClick;
    }
    
    public override void UpdateUI(Component sender, object[] payload)
    {
        
    }



}
