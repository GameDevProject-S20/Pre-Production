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

    float panBorderSizeInMenu = 15;
    float panBorderSize = 80;

    private Vector3 lastMousePosition = Vector3.zero;
    private bool mouseJustClicked = false;


    private void Update() 
    {
        transform.position = GeneralPurposeControl(transform.position);
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
        Vector3 velocity = Vector3.zero;

        var xPos = Input.mousePosition.x;
        var yPos = Input.mousePosition.y;

        var xBorder = panBorderSize;
        var yBorder = panBorderSize;
        bool inMenuRegion = (Screen.height - yPos <= panBorderSize);
        if (inMenuRegion)
        {
            xBorder = panBorderSizeInMenu;
            yBorder = panBorderSizeInMenu;
        }

        if (xPos > Screen.width - xBorder)
        {
            // Camera right
            velocity += new Vector3(1, 0, 0);
        }
        else if (xPos < xBorder)
        {
            // Camera left
            velocity += new Vector3(-1, 0, 0);
        }

        if (yPos > Screen.height - yBorder)
        {
            // Camera up
            velocity += new Vector3(0, 0, 1);
        }
        else if (yPos < yBorder)
        {
            // Camera down
            velocity += new Vector3(0, 0, -1);
        }

        velocity *= panSpeed;

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
