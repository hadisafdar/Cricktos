using TMPro;
using UnityEngine;

public class LoaderController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HWText loaderText;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Start()
    {
        HideLoader();
    }


    public void ShowLoader(string text)
    {
        loaderText.SetText(text);
        SetCanvasGroupState(true);
    }
    public void HideLoader()
    {
        SetCanvasGroupState(false);
    }


    private void SetCanvasGroupState(bool show)
    {
        canvasGroup.alpha = show?1:0;
        canvasGroup.interactable = show;
        canvasGroup.blocksRaycasts = show;
    }

}
