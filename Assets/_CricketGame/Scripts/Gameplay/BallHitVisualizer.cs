using UnityEngine;

public class BallHitVisualizer : MonoBehaviour
{
    [SerializeField] private Transform visual;

    private PhotonPlayer localPlayer;


    private void Start()
    {
        SetVisibility();
    }

    public void OnBallXChange(Component sender, object[] payload)
    {
        SetVisibility();
        //If the local player is a bowler
        if (localPlayer.PlayerRole == PlayerRole.Bowler)
        {
            float x = (float)payload[0];
            visual.transform.position = new Vector3(x, visual.position.y, visual.position.z);
        }

    }

    private void SetVisibility()
    {
        if (localPlayer == null)
        {
            localPlayer = SingletonManager.NetworkingManager.GetLocalPhotonPlayer();
        }
        if(localPlayer)visual.gameObject.SetActive(localPlayer.PlayerRole == PlayerRole.Bowler);
    }
}
