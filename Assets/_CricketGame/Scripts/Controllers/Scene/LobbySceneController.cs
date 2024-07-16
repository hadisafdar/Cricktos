using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Udar.SceneManager;
using System.Collections;
using UnityEngine.UI;

public class LobbySceneController : MonoBehaviourPunCallbacks
{
    
    [SerializeField] private SceneField gameScene;
    [SerializeField] private Button startButton;

    [Header("Player Cards")]
    [SerializeField] private LobbyPlayerCard playerACard;
    [SerializeField] private LobbyPlayerCard playerBCard;


    [Header("Debug")]
    [SerializeField] private bool inDebugMode;

    private void Awake()
    {

        startButton.onClick.AddListener(StartGame);
        SetStartButtonVisibility();
    }

    private void SetStartButtonVisibility()
    {
        int maxPlayers = inDebugMode ? 1 : 2;

        startButton.gameObject.SetActive(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= maxPlayers);
    }

    private void Start()
    {
        UpdatePlayerCards();
        
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player player)
    {
        base.OnPlayerLeftRoom(player);
        UpdatePlayerCards();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player player)
    {
        base.OnPlayerEnteredRoom(player);
        UpdatePlayerCards();
      
    }

    private void UpdatePlayerCards()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.LogError("Not in a room.");
            HideAllPlayerCards();
            return;
        }

        int playerCount = PhotonNetwork.CurrentRoom.Players.Count;
        ManagePlayerCardVisibility(playerCount);

        int index = 0;
        foreach (KeyValuePair<int, Photon.Realtime.Player> kvp in PhotonNetwork.CurrentRoom.Players)
        {
            if (index < 2)
            {
                UpdatePlayerCard(index, kvp.Value);
                index++;
            }
            else
            {
                Debug.LogWarning("More players than expected.");
            }
        }

        SetStartButtonVisibility();
    }



    private void StartGame()
    {
        
        if (PhotonNetwork.IsMasterClient)
        {
            SingletonManager.NetworkingManager.LoadNetworkedScene(gameScene.Name);
            base.photonView.RPC(nameof(RPC_StartGame), RpcTarget.Others);    
        }
    }

    [PunRPC]
    private void RPC_StartGame()
    {
        SingletonManager.NetworkingManager.LoadNetworkedScene(gameScene.Name);
    }

    private void UpdatePlayerCard(int index, Photon.Realtime.Player player)
    {
        if (index == 0)
        {
            if (!playerACard.IsInitialized)
                playerACard.Initialize(player);
        }
        else if (index == 1)
        {
            if (!playerBCard.IsInitialized)
                playerBCard.Initialize(player);
        }
    }

    private void ManagePlayerCardVisibility(int playerCount)
    {
        switch (playerCount)
        {
            case 0:
                HideAllPlayerCards();
                break;
            case 1:
                ShowOnlyPlayerACard();
                break;
            case 2:
                ShowBothPlayerCards();
                break;
            default:
                Debug.LogWarning("Unexpected number of players.");
                break;
        }
    }

    private void HideAllPlayerCards()
    {
        playerACard.Hide();
        playerBCard.Hide();
    }

    private void ShowOnlyPlayerACard()
    {
        playerACard.Show();
        playerBCard.Hide();
    }

    private void ShowBothPlayerCards()
    {
        playerACard.Show();
        playerBCard.Show();
    }

    private void OnDestroy()
    {
        SingletonManager.NetworkingManager.E_OnPlayerEnteredRoom -= OnPlayerEnteredRoom;
        SingletonManager.NetworkingManager.E_OnPlayerLeftRoom -= OnPlayerLeftRoom;
    }
}
