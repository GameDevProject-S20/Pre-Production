using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private Component[] texts;
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.GetComponent<Image>().enabled = false;
        texts = GetComponentsInChildren<Text>();
        foreach (Text tex in texts)
        {
            tex.gameObject.SetActive(false);
        }
    }

    public void GenerateTooltip (Item item)
    {
        gameObject.GetComponent<Image>().enabled = true;
        gameObject.SetActive(true);
        texts = GetComponentsInChildren<Text>(true);
        foreach (Text tex in texts)
        {
            tex.gameObject.SetActive(true);
        }

        gameObject.transform.GetChild(0).GetComponent<Text>().text = item.DisplayName;
        gameObject.transform.GetChild(1).GetComponent<Text>().text = item.Tooltip + "\n \n Right Click for Details";
    }

    public void GenerateDetailedTooltip(Item item)
    {
        gameObject.GetComponent<Image>().enabled = true;
        gameObject.SetActive(true);
        texts = GetComponentsInChildren<Text>(true);
        foreach (Text tex in texts)
        {
            tex.gameObject.SetActive(true);
        }

        gameObject.transform.GetChild(0).GetComponent<Text>().text = item.DisplayName;
        string descriptor = item.Description + "\n \n";
        string tagList = "";
        foreach (typetag tag in item.tags)
        {
            tagList += tag.ToString() + ", ";
        }
        tagList = tagList.Substring(0, tagList.Length - 2);
        tagList = tagList.Replace("_"," ");

        gameObject.transform.GetChild(1).GetComponent<Text>().text = descriptor + tagList;
    }
}
