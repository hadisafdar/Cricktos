using UnityEngine;
using UnityEngine.UI;

public class LoadingUIController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Image loadingBarFill;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        ShowLoadingScreen(false);
    }

    public void OnLoadStateChanged(Component sender,object[] payload)
    {
        if (!(sender is SceneManagement))
        {
            HWLogger.LogError(GetType(), GlobalDefine.LOG_INVALIDSENDERMESSAGE(typeof(SceneManagement),sender.GetType()));
            return;
        }

        bool loadingComplete = (bool)payload[0];
        ShowLoadingScreen(!loadingComplete);

    }

    public void OnLoadingProgress(Component sender, object[] payload)
    {
        if (!(sender is SceneManagement))
        {
            HWLogger.LogError(GetType(), GlobalDefine.LOG_INVALIDSENDERMESSAGE(typeof(SceneManagement), sender.GetType()));
            return;
        }
        float loadingProgress = (float)payload[0];
        loadingBarFill.fillAmount = loadingProgress;
    }


    private void ShowLoadingScreen(bool show)
    {
        canvasGroup.alpha = show ? 1 : 0;
        canvasGroup.interactable = show;
        canvasGroup.blocksRaycasts = show;
    }

}
