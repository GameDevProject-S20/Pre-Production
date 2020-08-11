using Dialogue;
using Encounters;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Campfire : MonoBehaviour
{
    public RawImage Background;

    private void Start()
    {
        EncounterManager.Instance.RunEncounterById(5000);
        DialogueManager.Instance.ActiveDialogueChanged.AddListener(() =>
        {
            if (DialogueManager.Instance.GetActiveDialogue() == null)
            {
                StartCoroutine("CloseScene");
            }
        });
    }

    private IEnumerator CloseScene()
    {
        Color startColour = Background.color;
        float time = 0f;
        float fadeTime = 2f;

        while (Background.color != Color.black)
        {
            Background.color = Color.Lerp(startColour, Color.black, time / fadeTime);
            time += Time.deltaTime;
            yield return null;
        }

        Background.color = startColour;
        SceneManager.UnloadSceneAsync("Campfire");
    }
}
