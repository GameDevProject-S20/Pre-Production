using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SIEvents;

public class Daylight : MonoBehaviour
{

    Quaternion targetRotation;

    private void Start() {
        EventManager.Instance.OnTimeAdvance.AddListener(RotateSun);
        transform.rotation = Quaternion.Euler(15 * (DataTracker.Current.hourCount - 6), -90, -90);
    }

    void RotateSun(int i) {
        targetRotation = Quaternion.Euler(15 * (DataTracker.Current.hourCount - 6), -90, -90);
    }

    private void Update() {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
    }
}
