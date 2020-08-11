using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SIEvents;

public class Edge : MonoBehaviour
{
    MapNode node1;
    MapNode node2;
    LineRenderer lineRenderer;
    public void init(MapNode node1, MapNode node2){
        this.node1 = node1;
        this.node2 = node2;
        lineRenderer = GetComponent<LineRenderer>();
        EventManager.Instance.OnColourChange.AddListener(Colour);
    }
    private void OnDestroy() {
        EventManager.Instance.OnColourChange.RemoveListener(Colour);
    }


    void Colour(){
        Color color1 = Color.black;
        Color color2 = Color.black;
        GameObject icon1 = node1.gameObject.transform.Find("Icon").gameObject;
        GameObject icon2 = node2.gameObject.transform.Find("Icon").gameObject;

        if (icon1.activeInHierarchy) {
            color1 = icon1.GetComponent<SpriteRenderer>().color;
        }
        if (icon2.activeInHierarchy) {
            color2 = icon2.GetComponent<SpriteRenderer>().color;
        }
        if (icon1.activeInHierarchy && !icon2.activeInHierarchy) {
            color2 = color1;
            color2.a = 0.0f;
        }
        if (!icon1.activeInHierarchy && icon2.activeInHierarchy) {
            color1 = color2;
            color1.a = 0.0f;
        }
        lineRenderer.startColor = color1;
        lineRenderer.endColor = color2;
    }
}
