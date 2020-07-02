using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TownMenu : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nameTextField;
    [SerializeField]
    TextMeshProUGUI descriptionTextField;

    // stores the town data for later reference
    private Town townData;

    private void Start() {
        townData = TownManager.Current.GetCurrentTownData();
        nameTextField.text = townData.Name;
        descriptionTextField.text = "They are led by <color="+townData.Colour+">"+ townData.Leader +"</color>.";
    }


    /// <summary>
    /// Sets the text for a label within a button.
    /// </summary>
    /// <param name="button"></param>
    /// <param name="childName"></param>
    /// <param name="text"></param>
    private void SetLabelText(GameObject button, string childName, string text)
    {
        var child = button.transform.Find(childName).gameObject;
        var textComponent = child.GetComponent<TextMeshProUGUI>();
        textComponent.text = text;
    }

    public void OnButtonClick(){
        SceneManager.LoadScene("MapScene");
    }


}

public struct ActionListItem {
    public Sprite Icon;
    public string Name;
    public string Description;
    public string SceneName;
}