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
        gameObject.transform.GetChild(1).GetComponent<Text>().text = item.Tooltip;
    }
}
