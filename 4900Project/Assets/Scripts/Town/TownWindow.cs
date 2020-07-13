using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TownWindow : MonoBehaviour
{
    [SerializeField]
    GameObject ActionPrefab;

    Transform ActionMenu;
    // stores the town data for later reference
    private Town townData;

    private void Start()
    {
        ActionMenu = transform.Find("TownBackground").Find("DataBorder").Find("DataBackground").Find("TownActionsBorder").Find("TownActionsBackground").Find("ActionList").Find("ActionGrid");
        //townData = TownManager.Instance.GetCurrentTownData();
        townData = new Town(15, "Big Test Town", "Sheriff Briggs");
        transform.Find("TownBackground").Find("TownName").GetComponent<Text>().text = townData.Name;
        transform.Find("TownBackground").Find("DataBorder").Find("DataBackground").Find("TownDataBackground").Find("TownData").Find("Description").GetComponent<Text>().text = "They are led by <color=" + townData.Colour + ">" + townData.Leader + "</color>.";

        GameObject NewAction = Instantiate(ActionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        NewAction.transform.parent = ActionMenu;
        NewAction.transform.Find("Name").GetComponent<Text>().text = townData.Leader;
        NewAction.transform.Find("Description").GetComponent<Text>().text = "I run this here town, talk to me if you need help.";

        NewAction = Instantiate(ActionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        NewAction.transform.Find("Name").GetComponent<Text>().text = townData.Leader;
        NewAction.transform.parent = ActionMenu;
        NewAction.transform.Find("Description").GetComponent<Text>().text = "I am Sheriff #2.";

        NewAction = Instantiate(ActionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        NewAction.transform.parent = ActionMenu;
        NewAction.transform.Find("Name").GetComponent<Text>().text = townData.Leader;
        NewAction.transform.Find("Description").GetComponent<Text>().text = "I'm Sheriff #3!";

        NewAction = Instantiate(ActionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        NewAction.transform.parent = ActionMenu;
        NewAction.transform.Find("Name").GetComponent<Text>().text = townData.Leader;
        NewAction.transform.Find("Description").GetComponent<Text>().text = "I'm the last Sheriff...";

    }
}
