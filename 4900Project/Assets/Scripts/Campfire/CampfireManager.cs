using SIEvents;
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

    public void LoadCampfireScene()
    {
        SceneManager.LoadScene("Campfire", LoadSceneMode.Additive);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode _)
    {
        if (scene.name == "Campfire")
        {
            EventManager.Instance.OnCampfireStarted.Invoke();
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (scene.name == "Campfire")
        {
            EventManager.Instance.OnCampfireEnded.Invoke();
        }
    }
}
