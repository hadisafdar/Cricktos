using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Sirenix.OdinInspector;
using HyyderWorks.EventSystem;
using System;

public class SceneManagement : MonoBehaviour
{
    [Header("Events")]
    [SerializeField, Required] private HWEvent onLoadStateChanged;
    [SerializeField, Required] private HWEvent onLoadProgressChanged;

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }


    public void LoadSceneAsync(int index,Action onSceneLoaded = null)
    {
        StartCoroutine(C_LoadSceneAsync(index,onSceneLoaded));
    }

    private IEnumerator C_LoadSceneAsync(int index, Action onSceneLoaded)
    {
        onLoadStateChanged.Raise(this, new object[] { false });

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
        asyncLoad.allowSceneActivation = false; // Start with scene activation disabled

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // Normalize progress

            // Raise progress changed event
            onLoadProgressChanged.Raise(this, new object[] { progress });

            Debug.Log("Loading progress: " + progress);

            if (asyncLoad.progress >= 0.9f)
            {
                // Once progress reaches 90%, allow scene activation
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        // Scene is fully loaded and activated
        Debug.Log("Scene Loaded");
        onLoadStateChanged.Raise(this, new object[] { true });

        onSceneLoaded?.Invoke();
    }

}
