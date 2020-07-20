using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraController : MonoBehaviour
{
    [SerializeField]
    float panSpeed = 0.1f;
    [SerializeField]
    float zoomSpeed = 4.0f;
    [SerializeField]
    Vector3 max;
    [SerializeField]
    Vector3 min;


    float panBorderSize = 20;

    private Vector3 lastMousePosition = Vector3.zero;
    private bool mouseJustClicked = false;


    private void Update() 
    {
        transform.position =  GeneralPurposeControl(transform.position);
        transform.position = MousePanControl(transform.position);
        transform.position = MouseScrollControl(transform.position);
    }


    public Vector3 GeneralPurposeControl(Vector3 position)
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 nextPosition = position;
        if (input != Vector3.zero){
            Vector3 velocity = input * panSpeed;
            nextPosition = position + velocity;
            nextPosition.x = Mathf.Clamp(nextPosition.x, min.x, max.x);
            nextPosition.z = Mathf.Clamp(nextPosition.z, min.z, max.z);

        }
            return nextPosition;
    }

    public Vector3 MousePanControl(Vector3 position)
    {
        float verticalBorder = Screen.width/panBorderSize;
        float horizontalBorder = Screen.height/panBorderSize;

        Vector3 velocity = Vector3.zero;

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
        velocity = velocity * panSpeed;

        Vector3 nextPosition = position + velocity;
        nextPosition.x = Mathf.Clamp(nextPosition.x, min.x, max.x);
        nextPosition.z = Mathf.Clamp(nextPosition.z, min.z, max.z);

        return nextPosition;
    }

    public Vector3 MouseScrollControl(Vector3 position)
    {
        Vector3 nextPosition = position;

        nextPosition.y += Input.mouseScrollDelta.y * zoomSpeed;
        nextPosition.y = Mathf.Clamp(nextPosition.y, min.y, max.y);

        return nextPosition;
    }

    public Vector3 MouseDragControl(Vector3 position)
    {
        Vector3 nextPosition = position;
        Vector3 input = Vector3.zero;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, position.y));

        if (Input.GetMouseButton(0) && !mouseJustClicked)
        {
            mouseJustClicked = true;
            lastMousePosition = mousePosition;
        }
        if (Input.GetMouseButton(0)) {
            input = (lastMousePosition - mousePosition);
            lastMousePosition = mousePosition;

            nextPosition = position + input;
        }
          else {
            mouseJustClicked = false;
        }
        return nextPosition;

    }
}
