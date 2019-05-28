using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MarkAsPermanent : MonoBehaviour
{
    [SerializeField] string[] sceneNames = null;

    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (string sceneName in sceneNames)
        {
            if (sceneName == scene.name)
            {
                Destroy(gameObject);
            }
        }
    }
}
