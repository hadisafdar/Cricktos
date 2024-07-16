using Photon.Pun;
using UnityEngine;

public abstract class PlayerController : MonoBehaviour
{
    public PhotonView PhotonView { get; private set; }
    public PhotonPlayer PhotonPlayer { get; private set; }
    protected virtual void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        PhotonPlayer = GetComponent<PhotonPlayer>();
    }
}
