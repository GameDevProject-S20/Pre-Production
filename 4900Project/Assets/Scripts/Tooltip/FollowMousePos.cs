using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMousePos : MonoBehaviour
{
    void LateUpdate()
    {
        transform.position = Input.mousePosition; 
    }
}
