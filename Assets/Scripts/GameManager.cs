using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Scene? currentlyLoadedScene;
    
    private void Awake()
    {
        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        LoadScene("Menu");
    }

    public void LoadScene(string name)
    {
        Debug.Assert(Application.CanStreamedLevelBeLoaded(name), $"Can't load scene {name}");
        UnloadCurrentScene();
        SceneManager.LoadScene(name, LoadSceneMode.Additive);

    }

    private void UnloadCurrentScene()
    {
        if(currentlyLoadedScene != null)
        {
            SceneManager.UnloadSceneAsync((Scene)currentlyLoadedScene);
            currentlyLoadedScene = null;
        }
    }
    
    // called by SceneManager
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentlyLoadedScene = scene;
        SceneManager.SetActiveScene(scene);
    }
}
