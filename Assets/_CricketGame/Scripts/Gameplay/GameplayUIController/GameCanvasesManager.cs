using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

[System.Serializable]
public struct GameCanvasInfo
{
    public HWPanel Canvas;
    public PlayerRole PlayerRole;
}

public class GameCanvasesManager : MonoBehaviour
{
    [SerializeField] private GameCanvasInfo[] gameCanvasInfoArray;

    private Player localPlayer;

    private void Awake()
    {
        localPlayer = PhotonNetwork.LocalPlayer;
    }

    /// <summary>
    /// Called from HWEvent
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="payload"></param>
    public void OnPhotonPlayerInfoChanged(Component sender,object[] payload)
    {
        PhotonPlayer _localPlayer = (PhotonPlayer)sender;
        if(_localPlayer.Player == localPlayer)
        {
            ShowCanvasBasedOnRole(_localPlayer.PlayerRole);
        }
    } 


    public void ShowCanvasBasedOnRole(PlayerRole playerRole)
    {
        foreach (GameCanvasInfo _canvasInfo in gameCanvasInfoArray)
        {
            if (_canvasInfo.PlayerRole.Equals(playerRole)) _canvasInfo.Canvas.Show();
            else _canvasInfo.Canvas.Hide();
        }
    }

}
