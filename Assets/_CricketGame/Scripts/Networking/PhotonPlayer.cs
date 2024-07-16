using HyyderWorks.EventSystem;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
using UnityEngine;

public enum PlayerRole
{
    Batsman,
    Bowler
}

public class PhotonPlayer : MonoBehaviour
{

    [Header("Data")]
    [SerializeField] private BatsmanController batsmanControllerTemplate;
    [SerializeField] private BowlerController bowlerControllerTemplate;

    [Header("Events")]
    [SerializeField] private HWEvent onPhotonPlayerInfoChanged;

    [Header("Debug")]

    [ShowInInspector,ReadOnly] private PlayerController controller;

    [ShowInInspector,ReadOnly]
    public Player Player { get; set; }
    [ShowInInspector, ReadOnly]
    public PlayerRole PlayerRole { get; private set; }

    private PhotonView photonView;


    public PlayerController Controller => controller;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        GameManager.Instance.OnStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= OnGameStateChanged;
        }
    }

    public void SetPlayerRole(PlayerRole playerRole)
    {
        if (!photonView.IsMine) return;
        Debug.Log("Set Player Role to: " + playerRole);
        PlayerRole = playerRole;
        Player.CustomProperties["playerRole"] = PlayerRole;
        PhotonNetwork.LocalPlayer.SetCustomProperties(Player.CustomProperties);
        onPhotonPlayerInfoChanged.Raise(this, null);
        OnGameStateChanged(GameManager.Instance.CurrentState); //Manually call this method to avoid delays
    }

    private void OnGameStateChanged(GameState gameState)
    {
        if (gameState == GameState.GameStart)
        {
            Debug.Log("Spawning Player Controller");
            Transform spawnPoint = SpawnPointProvider.Instance.GetSpawnPoint(PlayerRole);
            switch (PlayerRole)
            {
                case PlayerRole.Batsman:
                    controller = SingletonManager.NetworkingManager.InstantiateNetworkedObject(batsmanControllerTemplate.name, spawnPoint.position, spawnPoint.rotation).GetComponent<PlayerController>();
                    break;
                case PlayerRole.Bowler:
                    controller = SingletonManager.NetworkingManager.InstantiateNetworkedObject(bowlerControllerTemplate.name, spawnPoint.position, spawnPoint.rotation).GetComponent<PlayerController>();
                    break;
                default:
                    break;
            }

            CameraManager.Instance.SetLocalPlayerRole(PlayerRole);


        }
    }
}
