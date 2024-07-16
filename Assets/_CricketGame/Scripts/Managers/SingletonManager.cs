using Sirenix.OdinInspector;
using UnityEngine;

public class SingletonManager : GenericSingleton<SingletonManager>
{
    [Header("References")]
    [InfoBox("Not Necessary to Fill in")]
    [SerializeField] private SceneManagement sceneManagement;
    [SerializeField] private NetworkingManager networkingManager;
    [SerializeField] private LoaderController loaderController;



    #region [Getters]
    public static SceneManagement SceneManagement => instance.sceneManagement ??= FindObjectOfType<SceneManagement>();
    public static NetworkingManager NetworkingManager => instance.networkingManager ??= FindObjectOfType<NetworkingManager>();

    public static LoaderController LoaderController => instance.loaderController ??= FindObjectOfType<LoaderController>();

    #endregion


    [Button("Fill Dependencies")]
    private void FillDependencies()
    {
        sceneManagement = FindObjectOfType<SceneManagement>();
        networkingManager = FindObjectOfType<NetworkingManager>();
        loaderController = FindObjectOfType<LoaderController>();
    }

}
