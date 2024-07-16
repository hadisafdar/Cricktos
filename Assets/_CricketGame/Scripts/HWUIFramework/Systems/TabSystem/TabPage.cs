using Sirenix.OdinInspector;
using UnityEngine;

public class TabPage : UIView
{
    [InfoBox("Optional")]
    [SerializeField] private HWText tabPageTitleText;


    public void SetTitle(string title)
    {
        if (tabPageTitleText) tabPageTitleText.SetText(title);
    }


    public override void UpdateUI(Component sender, object[] payload)
    {
        throw new System.NotImplementedException();
    }


}
