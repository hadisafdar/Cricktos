using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyPlayerCard : MonoBehaviourPunCallbacks
{
    [Header("Data")]
    [InfoBox("Don't use in Release Build", InfoMessageType.Warning)]
    [SerializeField] private bool useRandomPlayerIcon;
    [PreviewField]
    [SerializeField, ShowIf("useRandomPlayerIcon")] private Sprite[] playerIcons;

    [Header("Settings")]
    [SerializeField] private Color localPlayerColor;
    [SerializeField] private Color opponentPlayerColor;
    [SerializeField] private bool reverseArrangement;

    [Header("UI")]
    [SerializeField] private Image playerIcon;
    [SerializeField] private Image outline;
    [SerializeField] private HWText playerNameText;
    [SerializeField] private HorizontalLayoutGroup layoutGroup;
    [SerializeField] private HWPanel cardView;

    private Photon.Realtime.Player player;
    public bool IsInitialized = false;

    private void Start()
    {
        layoutGroup.reverseArrangement = reverseArrangement;
    }

    public void Initialize(Photon.Realtime.Player player)
    {
        this.player = player;
        playerNameText.SetText(player.NickName);

        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            if (player == PhotonNetwork.LocalPlayer && useRandomPlayerIcon && !IsInitialized)
            {
                int iconIndex = Random.Range(0, playerIcons.Length);
                Hashtable playerProperties = new Hashtable { { "playerIcon", iconIndex } };
                PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
            }
        }
        else
        {
            // If not connected or not in room, delay setting custom properties or handle differently
            Debug.LogWarning("Photon not connected or not in room. Custom properties not set.");
        }

        UpdatePlayerCard();
        IsInitialized = true;
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == player && changedProps.ContainsKey("playerIcon"))
        {
            UpdatePlayerCard();
        }
    }

    private void UpdatePlayerCard()
    {
        if (player.CustomProperties.TryGetValue("playerIcon", out object iconIndexObj) && useRandomPlayerIcon)
        {
            int iconIndex = (int)iconIndexObj;
            playerIcon.sprite = playerIcons[iconIndex];
        }

        outline.color = player == PhotonNetwork.LocalPlayer ? localPlayerColor : opponentPlayerColor;
    }

    private void OnValidate()
    {
        layoutGroup.reverseArrangement = reverseArrangement;
    }

    public void Show()
    {
        cardView.Show();
    }

    public void Hide()
    {
        cardView.Hide();
        IsInitialized = false; // Reset initialization when hiding
    }
}
