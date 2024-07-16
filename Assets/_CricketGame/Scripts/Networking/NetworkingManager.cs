using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using System;
using Photon.Realtime;

public class NetworkingManager : MonoBehaviourPunCallbacks
{
    public const string DEFAULTROOMCODE = "1234";

  
    public bool ConnectedToMaster=>PhotonNetwork.IsConnected;

    // C# events
    public event Action E_OnConnectedToMaster;
    public event Action<string> E_OnError;
    public event Action E_OnJoinLobby;

    public event Action E_OnLeaveRoomEvent;
    public event Action<bool> E_OnRoomJoinEvent; // true = success; false = failure

    public event Action<Photon.Realtime.Player> E_OnPlayerEnteredRoom;
    public event Action<Photon.Realtime.Player> E_OnPlayerLeftRoom;


    private void Start()
    {
        ConnectToMaster();
    }

    public void ConnectToMaster()
    {
        if (!ConnectedToMaster)
        {
            
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to server successfully.");
        E_OnConnectedToMaster?.Invoke();
        
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        E_OnError?.Invoke($"Disconnected from server. Cause: {cause}");
    }

    public void JoinLobby()
    {
        if (!ConnectedToMaster)
        {
            Debug.LogError("Not Connected To Master");
            return;
        }
        PhotonNetwork.JoinLobby();
    }

  

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined lobby successfully.");
        E_OnJoinLobby?.Invoke();
    }

    public void JoinRoom(string roomCode, int maxPlayers)
    {
        Debug.Log("Trying to join or create room with code: " + roomCode);
        PhotonNetwork.JoinOrCreateRoom(roomCode, new Photon.Realtime.RoomOptions { MaxPlayers = maxPlayers, BroadcastPropsChangeToAll = true }, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        // Handle joining logic here
        E_OnRoomJoinEvent?.Invoke(true);
        

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        E_OnRoomJoinEvent?.Invoke(false);
        Debug.Log($"Failed to join room. Reason: {message}");
    }

    public void LoadNetworkedScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    public GameObject InstantiateNetworkedObject(string name, Vector3 position, Quaternion rotation)
    {
        GameObject networkedObject = PhotonNetwork.Instantiate(name, position, rotation);
        return networkedObject;
    }

    public GameObject InstantiateNetworkedRoomObject(string name, Vector3 position, Quaternion rotation)
    {
        GameObject networkedObject = PhotonNetwork.InstantiateRoomObject(name, position, rotation);
        return networkedObject;
    }


    public void DestroyNetworkedObject(GameObject gameObject)
    {
        PhotonNetwork.Destroy(gameObject);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        E_OnLeaveRoomEvent?.Invoke();
    }

    #region [Player]
    public void AssignNickname(string nickname)
    {
        PhotonNetwork.NickName = nickname;
        Debug.Log("Assigned player nickname: " + nickname);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        E_OnPlayerEnteredRoom?.Invoke(newPlayer);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        E_OnPlayerLeftRoom?.Invoke(otherPlayer);
    }

    public PhotonPlayer GetLocalPhotonPlayer()
    {
        PhotonPlayer[] photonPlayers = FindObjectsOfType<PhotonPlayer>();
        foreach (PhotonPlayer _player in photonPlayers)
        {
            if (_player.Player == PhotonNetwork.LocalPlayer) return _player;
        }
        return null;
    }

    #endregion
}
