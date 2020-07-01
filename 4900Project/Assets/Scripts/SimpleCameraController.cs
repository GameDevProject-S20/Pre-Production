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
    }
}
