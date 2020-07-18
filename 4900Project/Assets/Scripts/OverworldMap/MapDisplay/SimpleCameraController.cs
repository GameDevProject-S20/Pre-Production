using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraController : MonoBehaviour
{
    [SerializeField]
    float speed = 1.0f;
    [SerializeField]
    Vector3 max;
    [SerializeField]
    Vector3 min;

    private void Update() {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (input != Vector3.zero){
            Vector3 velocity = input * speed;
            Vector3 nextPosition = transform.position + velocity;
            nextPosition.x = Mathf.Clamp(nextPosition.x, min.x, max.x);
            nextPosition.z = Mathf.Clamp(nextPosition.z, min.z, max.z);
            transform.position = nextPosition;
        }

        MouseControl(20);
    }

    public void MouseControl(float borderSize = 6)
    {
        float verticalBorder = Screen.width/borderSize;
        float horizontalBorder = Screen.height/borderSize;

        Vector3 velocity = new Vector3(0, 0, 0);

        if (Input.mousePosition.x > Screen.width - verticalBorder)
        {
            velocity += new Vector3(1, 0, 0);
        }

        if (Input.mousePosition.x < verticalBorder)
        {
            velocity += new Vector3(-1, 0, 0);
        }

        if (Input.mousePosition.y > Screen.height - horizontalBorder)
        {
            velocity += new Vector3(0, 0, 1);
        }

        if ( Input.mousePosition.y < horizontalBorder)
        {
            velocity += new Vector3(0, 0, -1);
        }

        Vector3 nextPosition = velocity;
        nextPosition.x = Mathf.Clamp(velocity.x * speed, min.x, max.x);
        nextPosition.z = Mathf.Clamp(velocity.z * speed, min.z, max.z);
        transform.position += nextPosition;
    }
}
