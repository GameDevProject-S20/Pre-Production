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

    [SerializeField]
    GameObject button1;

    [SerializeField]
    GameObject button2;

    int action1;
    int action2;

    // stores the town data for later reference
    private Town townData;

    private void Start() {
        townData = TownManager.Current.GetCurrentTownData();
        nameTextField.text = townData.Name;
        descriptionTextField.text = "They are led by <color="+townData.Colour+">"+ townData.Leader +"</color>.";

        action1 = townData.shops[0];
        action2 = townData.shops[1];

        Shop shop1 = ShopManager.Current.GetShopById(townData.shops[0]);
        button1.transform.Find("Text").Find("Text_ActionName").GetComponent<TextMeshProUGUI>().text = shop1.name;
        button1.transform.Find("Text").Find("Text_ActionDescription").GetComponent<TextMeshProUGUI>().text = shop1.shortDescription;

        Shop shop2 = ShopManager.Current.GetShopById(townData.shops[1]);
        button2.transform.Find("Text").Find("Text_ActionName").GetComponent<TextMeshProUGUI>().text = shop2.name;
        button2.transform.Find("Text").Find("Text_ActionDescription").GetComponent<TextMeshProUGUI>().text = shop2.shortDescription;
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

    public void OnStoreButtonClick(int id){
        DataTracker.Current.currentShop = (id == 0) ? action1 : action2;
        SceneManager.LoadScene("InventoryTestScene");
    }



}

public struct ActionListItem {
    public Sprite Icon;
    public string Name;
    public string Description;
    public string SceneName;
}