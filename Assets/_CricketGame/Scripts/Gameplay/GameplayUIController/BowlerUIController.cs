using HyyderWorks.EventSystem;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class BowlerUIController : MonoBehaviour
{
    [SerializeField] private Button ballXButton;
    [SerializeField] private Slider ballXSlider;



    [Header("Events")]
    [SerializeField] private HWEvent OnBallXSelect;



    private void Awake()
    {
        ballXButton.onClick.AddListener(OnSelect);
    }

    public void OnBallXChange(Component sender, object[] payload)
    {
        float x = (float)payload[0];
        ballXSlider.value = x;
    }

    private void OnSelect()
    {
        PhotonPlayer photonPlayer = SingletonManager.NetworkingManager.GetLocalPhotonPlayer();
        if (photonPlayer.Player != PhotonNetwork.LocalPlayer) return;
        if (photonPlayer.PlayerRole != PlayerRole.Bowler) return;
        OnBallXSelect.Raise(this, null);
    
    }

    
}
