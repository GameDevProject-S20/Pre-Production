using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    private Component[] texts;
    public int rightClickStage = 0;

    void Awake()
    {
        gameObject.GetComponent<Image>().enabled = false;
        texts = GetComponentsInChildren<Text>();
        foreach (Text tex in texts)
        {
            tex.gameObject.SetActive(false);
        }
    }

    public void GenerateTooltip(string title, string desc)
    {
        gameObject.GetComponent<Image>().enabled = true;
        gameObject.SetActive(true);
        texts = GetComponentsInChildren<Text>(true);
        foreach (Text tex in texts)
        {
            tex.gameObject.SetActive(true);
        }

        gameObject.transform.GetChild(0).GetComponent<Text>().text = title;
        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = desc;
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
        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.Tooltip + "\n \n Right Click for Details";
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
        string descriptor = item.Description;

        // Only if in Inventory
        if (this == InventoryWindow.Instance.tooltip && item.IsConsumable)
        {
            int health = item.GetHealthCured();
            descriptor += $"\n\nRestores {health} health";
            descriptor += "\n\nRight click to consume";
        }
        
        descriptor += "\n\nWeight per Unit:" + item.Weight.ToString() + "\n \n";

        string tagList = "";
        string iconList = "";
        foreach (ItemTag tag in item.tags)
        {
            tagList += tag.ToString() + ", ";
            switch (tag)
            {
                case ItemTag.None:
                    break;
                case ItemTag.General:
                    iconList += makeIcon(3);
                    break;
                case ItemTag.Fuel:
                    iconList += makeIcon(2);
                    break;
                case ItemTag.Useable:
                    iconList += makeIcon(10);
                    break;
                case ItemTag.Food:
                    iconList += makeIcon(1);
                    break;
                case ItemTag.Luxury:
                    iconList += makeIcon(4);
                    break;
                case ItemTag.Medical:
                    iconList += makeIcon(5);
                    break;
                case ItemTag.Building_Materials:
                    iconList += makeIcon(13);
                    break;
                case ItemTag.Tools_And_Parts:
                    iconList += makeIcon(8);
                    break;
                case ItemTag.Combat:
                    iconList += makeIcon(14);
                    break;
                case ItemTag.Scientific:
                    iconList += makeIcon(7);
                    break;
                case ItemTag.Mineral:
                    iconList += makeIcon(6);
                    break;
                case ItemTag.Antique:
                    iconList += makeIcon(12);
                    break;
                case ItemTag.Advanced:
                    iconList += makeIcon(11);
                    break;
            }
            iconList += "  ";
        }
        tagList = tagList.Substring(0, tagList.Length - 2);
        tagList = tagList.Replace("_"," ");
        iconList = iconList.Substring(0, iconList.Length - 2);

        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = descriptor + tagList + "\n" + iconList;
    }

    public string makeIcon(int a)
    {
        string temp = "<sprite=" + a + ">";
        return temp;
    }
}
