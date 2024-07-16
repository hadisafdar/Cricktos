using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;

public class NetworkObjectDestroyer : MonoBehaviourPun
{
    [SerializeField] private bool destroyAfterDelay;
    [SerializeField, ShowIf("destroyAfterDelay")] private float destroyDelay;


    private void Start()
    {
        if (destroyAfterDelay)
        {
            Invoke(nameof(Destroy), destroyDelay);
        }
    }

    public void Destroy()
    {
        if (photonView.Owner == PhotonNetwork.LocalPlayer)
        {
            Debug.Log("We do not request the ownership. Already mine.");
          
            SingletonManager.NetworkingManager.DestroyNetworkedObject(gameObject);
        }
        else
        {
            TransferOwnership();
        }
    }
    private void TransferOwnership()
    {
        photonView.RequestOwnership();
        photonView.RPC(nameof(DestroyObject), RpcTarget.AllViaServer);
    }
    [PunRPC]
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
