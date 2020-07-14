using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TownMenu : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI NameTextField;
    public TextMeshProUGUI DescriptionTextField;
    public GameObject Button1;
    public GameObject Button2;


    int action1;
    int action2;

    // stores the town data for later reference
    private Town townData;

    private void Start() {
        UpdateTown(); 
    }

    public void UpdateTown()
    {

        townData = TownManager.Instance.GetCurrentTownData();
        NameTextField.text = townData.Name;
        DescriptionTextField.text = "They are led by <color=" + townData.Colour + ">" + townData.Leader + "</color>.";

        action1 = townData.shops[0];
        action2 = townData.shops[1];

        Shop shop1 = ShopManager.Instance.GetShopById(townData.shops[0]);
        Button1.transform.Find("Text").Find("Text_ActionName").GetComponent<TextMeshProUGUI>().text = shop1.name;
        Button1.transform.Find("Text").Find("Text_ActionDescription").GetComponent<TextMeshProUGUI>().text = shop1.shortDescription;

        Shop shop2 = ShopManager.Instance.GetShopById(townData.shops[1]);
        Button2.transform.Find("Text").Find("Text_ActionName").GetComponent<TextMeshProUGUI>().text = shop2.name;
        Button2.transform.Find("Text").Find("Text_ActionDescription").GetComponent<TextMeshProUGUI>().text = shop2.shortDescription;
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
        DataTracker.Current.currentShopId = (id == 0) ? action1 : action2;
        SceneManager.LoadScene("InventoryTestScene", LoadSceneMode.Additive);
    }


}

public struct ActionListItem {
    public Sprite Icon;
    public string Name;
    public string Description;
    public string SceneName;
}
