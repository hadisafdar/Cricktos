using Cinemachine;
using System.Linq;
using UnityEngine;
using static CameraManager;

[System.Serializable]
public class CameraInfo
{
    public CinemachineVirtualCamera Camera;
    public CameraView CameraView;
}

public class CameraManager : GenericSingleton<CameraManager>
{
    public enum CameraView
    {
        FocusBatsman,
        FocusBowler,
        FocusBall,
    }
    [SerializeField] private CameraInfo[] cameras;

    private PlayerRole localPlayerRole;
    private CameraView currentCameraView;


    private CameraInfo currentCamera;

    public void SetLocalPlayerRole(PlayerRole role)
    {
        localPlayerRole = role;
        switch (localPlayerRole)
        {
            case PlayerRole.Batsman:
                SetCameraView(CameraView.FocusBatsman);
                break;
            case PlayerRole.Bowler:
                SetCameraView(CameraView.FocusBowler);
                break;
            default:
                break;
        }
    }

    public void SetCameraView(CameraView cameraView)
    {
        currentCameraView = cameraView;
        if (currentCamera != null) currentCamera.Camera.Priority = 0; //Set priority to 0;
        currentCamera = cameras.FirstOrDefault(c => c.CameraView.Equals(currentCameraView));
        if(currentCamera != null)
        {
            currentCamera.Camera.Priority = 10;
        }
        
    }




}
