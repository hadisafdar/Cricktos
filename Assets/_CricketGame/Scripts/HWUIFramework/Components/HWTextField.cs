using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static TMPro.TMP_InputField;

public class HWTextField : UIView
{
    [Header("Data")]
    [SerializeField] private string placeholder;
    [SerializeField] private ContentType contentType;
    [Header("Styling")]
    [SerializeField] private Color textColor;
    [SerializeField] private Color placeHolderColor;

    [Header("Components")]
    [SerializeField] private HWText errorText;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI placeholderText;

    [Header("Dependencies")]
    [SerializeField, Required] private TMP_InputField inputFieldReference;

    // Validation function delegate
    public delegate Tuple<bool, string> ValidationDelegate(string input);

    // Validation function to be set by the user
    public ValidationDelegate ValidationFunction;

    public UnityEvent<string> OnValueChanged;

    private void Start()
    {
        errorText.Hide();
        inputFieldReference.onValueChanged.AddListener(ValidateInput);
        inputFieldReference.onValueChanged.AddListener((value)=>OnValueChanged?.Invoke(value));
    }

    public override void UpdateUI(Component sender, object[] payload)
    {
        // You can handle UI updates or events here
    }

    private void OnValidate()
    {
        if (text)
        {
            text.color = textColor;
        }

        if (placeholderText)
        {
            placeholderText.text = placeholder;
            placeholderText.color = placeHolderColor;
        }
        if (inputFieldReference)
        {
            inputFieldReference.contentType = contentType;
        }
        this.name = placeholder + "[Text Field]";
    }

    // Updated SetValue method with validation
    public string SetValue(string value)
    {
        inputFieldReference.text = value;

        

        return inputFieldReference.text;
    }


    private void ValidateInput(string value)
    {
        
        // Trigger validation
        if (ValidationFunction != null)
        {
            Debug.Log("Validating");
            Tuple<bool, string> validationResponse = ValidationFunction(value);
            if (!validationResponse.Item1)
            {
                // Show error if validation fails
                ShowError(validationResponse.Item2);
            }
            else
            {
                // Hide error if validation succeeds
                HideError();
            }
        }
    }

    // Show error message
    private void ShowError(string errorMessage)
    {
        errorText.SetText(errorMessage);
        errorText.Show();
    }

    // Hide error message
    private void HideError()
    {
        errorText.Hide();
    }

    public string Value => inputFieldReference.text;

    public void SetPlaceholder(string value) => inputFieldReference.placeholder.GetComponent<TextMeshProUGUI>().text = value;
    public void SetContentType(ContentType cType) => inputFieldReference.contentType = cType;
    public void SetCharacterLimit(int characterLimit) => inputFieldReference.characterLimit = characterLimit;

}
