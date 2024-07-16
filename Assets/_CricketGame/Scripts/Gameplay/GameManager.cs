using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
using UnityEngine;

public enum GameState
{
    NULL,
    GameStart,
    BowlerIdle,
    BowlerActive,
    BatsmanActive,
    BatSuccess,
    BatFailure,
    RoundEnd,
    GameEnd
}

public class GameManager : GenericSingleton<GameManager>
{
    [Header("Data")]
    [SerializeField] private bool masterIsBatsman;
    
    [Header("Spawning")]
    [SerializeField] private PhotonPlayer photonPlayerTemplate;


    [ShowInInspector, ReadOnly]
    public GameState CurrentState { get; private set; }

    public event Action<GameState> OnStateChanged;

    private List<Player> players = new List<Player>();


    private PhotonView photonView;

    public override void Awake()
    {
        base.Awake();
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
       CurrentState = GameState.NULL;
       SetState(GameState.GameStart);

    }
    public void SetState(GameState newState)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (CurrentState != newState)
        {
            CurrentState = newState;
            OnStateChanged?.Invoke(newState);

            Debug.Log($"Game state changed to: {newState}");
            // Perform actions based on the new state
            HandleStateChange(newState);
            photonView.RPC(nameof(RPC_GameStateChanged), RpcTarget.Others, CurrentState);

        }
    }

    private void HandleStateChange(GameState newState)
    {
        switch (newState)
        {
            case GameState.GameStart:
                OnGameStart();
                break;
                // Handle other states as needed
        }
    }

    private void OnGameStart()
    {
        GameObject photonPlayerGO =  SingletonManager.NetworkingManager.InstantiateNetworkedObject(photonPlayerTemplate.name, transform.position, transform.rotation);
        photonPlayerGO.TryGetComponent(out PhotonPlayer photonPlayer);
        photonPlayer.Player = PhotonNetwork.LocalPlayer;
        if (photonPlayer.Player ==  PhotonNetwork.MasterClient)
        {
            if (masterIsBatsman) photonPlayer.SetPlayerRole(PlayerRole.Batsman);
            else photonPlayer.SetPlayerRole(PlayerRole.Bowler);
        }
        else
        {
            if (masterIsBatsman) photonPlayer.SetPlayerRole(PlayerRole.Bowler);
            else photonPlayer.SetPlayerRole(PlayerRole.Batsman);
        }
    }


    [PunRPC]
    private void RPC_GameStateChanged(GameState gameState)
    {
        CurrentState = gameState;
        Debug.Log("Game State Changed On Client to: " + gameState);
        HandleStateChange(gameState);
    }
 
}
