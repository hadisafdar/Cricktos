using Udar.SceneManager;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Lobby Scene")]
    [SerializeField] private SceneField lobbyScene;

    [Header("Buttons")]
    [SerializeField] private Button joinLobbyBtn;


    private void Awake()
    {
        joinLobbyBtn.onClick.AddListener(JoinLobby);
    }

    private void JoinLobby()
    {
        joinLobbyBtn.interactable = false;
        SingletonManager.LoaderController.ShowLoader(GlobalDefine.MESSAGE_JOININGLOBBY);
        SingletonManager.NetworkingManager.JoinLobby();
        SingletonManager.NetworkingManager.E_OnJoinLobby += OnJoinedLobby;
    }


    private void OnJoinedLobby()
    {
        SingletonManager.NetworkingManager.JoinRoom("Room Test",2); //Join a random room with 2 players
        SingletonManager.NetworkingManager.E_OnRoomJoinEvent += OnJoinedRoomState;
        
    }



    private void OnJoinedRoomState(bool success)
    {
        joinLobbyBtn.interactable = true;
        if (success)
        {
            Debug.Log("Join Room Success!");
            SingletonManager.LoaderController.HideLoader();
            SingletonManager.SceneManagement.LoadSceneAsync(lobbyScene.BuildIndex);
        }
        else
        {
            //Show some sort of error here
            Debug.LogError("Could not Load Room!");
        }
    }

    private void OnDestroy()
    {
        SingletonManager.NetworkingManager.E_OnJoinLobby -= OnJoinedLobby;
        SingletonManager.NetworkingManager.E_OnRoomJoinEvent -= OnJoinedRoomState;

    }
}
