using UnityEngine;

public class HWPanel : UIView
{
    public override void UpdateUI(Component sender, object[] payload)
    {
        return;
    }

    protected override void Reset()
    {
        base.Reset();
        this.name = "HWPanel";
    }
}
