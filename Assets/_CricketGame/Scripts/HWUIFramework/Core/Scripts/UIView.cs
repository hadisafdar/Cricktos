using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UIView : MonoBehaviour
{
    [Header("UI View Settings")]
    [SerializeField] private bool enableInteractionsOnShow  = true;
    [SerializeField] private bool hideOnAwake;
    public UnityEvent OnShow;   
    public UnityEvent OnHide;

    private CanvasGroup canvasGroup;
    private void Awake()
    {  
        canvasGroup = GetComponent<CanvasGroup>();
        SetInteraction(true);
        if (hideOnAwake) Hide();
        if (!gameObject.activeInHierarchy && !hideOnAwake)
        {
            //Force activate the object
            gameObject.SetActive(true);
        }
    }

 

    /// <summary>
    /// Show the view
    /// </summary>
    [Button("Show")]
    public void Show()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        SetInteraction(enableInteractionsOnShow);
    }

    private void SetInteraction(bool show)
    {
        canvasGroup.alpha = show?1:0;
        canvasGroup.interactable = show;
        canvasGroup.blocksRaycasts = show;
    }

    /// <summary>
    /// Hide the view
    /// </summary>
    [Button("Hide")]
    public void Hide()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();

        SetInteraction(false);
    }

    public abstract void UpdateUI(Component sender, object[] payload);


    protected virtual void Reset()
    {

        canvasGroup = GetComponent<CanvasGroup>();
    }
}
