using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsWindow : MonoBehaviour
{

    public void CloseCreditsWindow()
    {

        SceneManager.UnloadSceneAsync("Credits");
    }


}
