using TMPro;
using UnityEngine;

/// <summary>
/// A component used to display text
/// </summary>
public class HWText : UIView
{
    [Multiline]
    [SerializeField] private string value;
    [SerializeField]private TextMeshProUGUI text;
    [SerializeField] private TMP_FontAsset fontAsset;
    [SerializeField] private Color color = Color.white;

    private void Awake()
    {
        if (text == null) text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetColor(Color _color)
    {
        color = _color;
        text.color = color;
    }


    public override void UpdateUI(Component sender, object[] payload)
    {
        if (payload.Length == 0)
        {
            Debug.LogError("Insufficient payload given to", this);
            return;
        }

        string value = payload[0].ToString(); //Convert the payload to string
        SetText(value); //update the value

    }

    public void SetText(string value) => text.text = value;


    protected override void Reset()
    {
        base.Reset();
        if (text == null) text = GetComponentInChildren<TextMeshProUGUI>();
        this.name = "HWText";
        value = "New Text";
        
    }


    private void OnValidate()
    {
        this.name = value + "[HWText]";
        if (text)
        {
            text.text = value;
            text.color = color;
            text.font = fontAsset;
            text.UpdateFontAsset();
        }
    }
    
}
