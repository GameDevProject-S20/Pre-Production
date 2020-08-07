using UnityEngine.SceneManagement;

public class CampfireManager
{
    public static CampfireManager Instance
    {
        get
        {
            if (instance == null) instance = new CampfireManager();
            return instance;
        }
    }

    private static CampfireManager instance;

    private CampfireManager()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("Campfire", LoadSceneMode.Additive);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode _)
    {
        if (scene.name == "Campfire")
        {
            // Need to make BackgroundMusic a singleton
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (scene.name == "Campfire")
        {

        }
    }
}
