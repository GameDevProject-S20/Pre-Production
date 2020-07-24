using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

public class DebugButtonScript : MonoBehaviour
{
    public void OnButtonClick()
    {
        IDPage page = DialogueManager.Instance.GetActiveDialogue().GetPage();
        Debug.Log(page);
    }
}
