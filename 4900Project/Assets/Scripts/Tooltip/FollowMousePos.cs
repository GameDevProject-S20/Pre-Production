using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMousePos : MonoBehaviour
{
    void LateUpdate()
    {

        transform.position = Input.mousePosition; 
        if (Input.mousePosition.x > 0.75 * Screen.width){
            transform.Translate(new Vector3(-gameObject.GetComponent<RectTransform>().sizeDelta.x,0 ,0));
        }
    }
}
