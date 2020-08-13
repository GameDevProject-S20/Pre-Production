using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    public Button quitButton; 


    public RawImage Background;


    private void Start()
    {

        quitButton.onClick.AddListener(() =>
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        });

    }

    public void OnTryAgainClick()
    {
        SceneManager.LoadScene("InitializerScene", LoadSceneMode.Single);
    }

    public void OnMainMenuClick()
    {
        SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
    }

}
