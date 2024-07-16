using Sirenix.OdinInspector;
using Udar.SceneManager;
using UnityEngine;
using UnityEngine.UI;

public class InitScreenController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private SceneField initialScene;
    [InfoBox("This property should not be used in a release build",InfoMessageType.Warning)]
    [SerializeField] private bool assignRandomNickname;


    [Header("Connection")]
    [SerializeField] private CanvasGroup connectionScreen;
    [SerializeField] private CanvasGroup connectionFailureScreen;
    [SerializeField] private Button retryConnectionBtn;

    private bool isInitialized = false;
    private bool connectedToMaster = false;


  

    private void Start()
    {
        ConnectToMaster();
        retryConnectionBtn.onClick.AddListener(ConnectToMaster);
    }
    private void ConnectToMaster()
    {
        ShowScreen(connectionScreen, true);
        ShowScreen(connectionFailureScreen, false);
        SingletonManager.NetworkingManager.ConnectToMaster();
        SingletonManager.NetworkingManager.E_OnConnectedToMaster += OnConnectedToMaster;
        SingletonManager.NetworkingManager.E_OnError += OnConnectionFailure;
    }

    private void OnConnectedToMaster()
    {
        ShowScreen(connectionScreen, false);
        connectedToMaster = true;
        SingletonManager.NetworkingManager.AssignNickname("Player" + Random.Range(1, 1000));
        SingletonManager.SceneManagement.LoadSceneAsync(initialScene.BuildIndex, null);
    }

    private void OnConnectionFailure(string error)
    {
        ShowScreen(connectionScreen, false);
        ShowScreen(connectionFailureScreen, true);
    }


  
    private void OnDestroy()
    {
        SingletonManager.NetworkingManager.E_OnConnectedToMaster -= OnConnectedToMaster;
        SingletonManager.NetworkingManager.E_OnError -= OnConnectionFailure;
    }

    public void ShowScreen(CanvasGroup canvasGroup,bool show)
    {
        canvasGroup.alpha = show ? 1 : 0;
        canvasGroup.interactable = show;
        canvasGroup.blocksRaycasts = show;
    }

}
