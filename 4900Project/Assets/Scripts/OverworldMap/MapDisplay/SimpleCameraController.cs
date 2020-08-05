using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SIEvents;
public class SimpleCameraController : MonoBehaviour
{
    //[SerializeField] Currently not used // WSAD Controls 
    float panSpeed = 0.1f;
    Vector3 min; //4.68, 10, 6.51
    Vector3 max; //-4.68, 4, -6.54


    [Header("Controls for zoom with scroll wheel")]
    [SerializeField]
    float zoomSpeed; // 4
    [SerializeField]
    float scrollMax; // 10
    [SerializeField]
    float scrollMin; // 4

    [Header("Settings for mouse dragging")]
    [SerializeField]
    public float dragSpeed; // -0.25
    [SerializeField]
    public float outerLeft; // -55
    [SerializeField]
    public float outerRight; // 30
    [SerializeField]
    public float outerUp; // 41
    [SerializeField]
    public float outerDown; // -45

    private Vector3 dragOrigin;

    // Currently not used 
    float panBorderSizeInMenu = 15;
    float panBorderSize = 80;

    bool active = true;

    int FreezeCount = 0; 

    private void Awake()
    {
        EventManager.Instance.FreezeMap.AddListener(() => {
            active = false;
            FreezeCount++;
        });
        EventManager.Instance.UnfreezeMap.AddListener(() => {
            FreezeCount--;
            if (FreezeCount <= 0)
            {
                FreezeCount = 0;
                active = true;
            }
        });
    }

    private void Update()
    {

        if (active)
        {
            GeneralPurposeControl(); //WSAD! 
            transform.position = MouseScrollControl(transform.position);
            MouseDragControl();
        }

    }

    // WSAD controls
    public void GeneralPurposeControl()
    {
        Vector3 position = transform.position;

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        Vector3 nextPosition = position;


        if (input != Vector3.zero)
        {
            Vector3 velocity = input * panSpeed * GetDragMultiplier() / 2.0f;
            nextPosition = position + velocity;

            // Calculate the expect translation to make sure it does not exceed max/min 
            Vector3 expectedTranslation = nextPosition;


            //Check t0 make sure not outside of the bounds 
            if (outerRight > expectedTranslation.x && expectedTranslation.x > outerLeft)
            {
                transform.Translate(new Vector3(velocity.x, 0, 0), Space.World);
            }

            if (outerUp > expectedTranslation.z && expectedTranslation.z > outerDown)
            {
                transform.Translate(new Vector3(0, 0, velocity.z), Space.World);
            }
        }


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
        nextPosition.x = Mathf.Clamp(nextPosition.x, scrollMin, scrollMax);
        nextPosition.z = Mathf.Clamp(nextPosition.z, scrollMin, scrollMax);

        return nextPosition;
    }

    public Vector3 MouseScrollControl(Vector3 position)
    {
        Vector3 nextPosition = position;

        nextPosition.y -= Input.mouseScrollDelta.y * zoomSpeed;
        nextPosition.y = Mathf.Clamp(nextPosition.y, scrollMin, scrollMax);

        return nextPosition;
    }

    public void MouseDragControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;


        float multiplier = GetDragMultiplier();

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed * multiplier, 0, pos.y * dragSpeed * multiplier);

        // Calculate the expect translation to make sure it does not exceed max/min 
        Vector3 expectedTranslation = transform.position + move;


        //Check to make sure not outside of the bounds 
        if (outerRight > expectedTranslation.x && expectedTranslation.x > outerLeft)
        {
            transform.Translate(new Vector3(move.x, 0, 0), Space.World);
        }

        if (outerUp > expectedTranslation.z && expectedTranslation.z > outerDown)
        {
            transform.Translate(new Vector3(0, 0, move.z), Space.World);
        }

    }

    /// <summary>
    /// Retrieves the Drag multiplier from the SettingsManager.
    /// </summary>
    /// <returns></returns>
    float GetDragMultiplier()
    {
        return DataTracker.Current.SettingsManager.ScrollingSpeedMultiplier;
    }
}
