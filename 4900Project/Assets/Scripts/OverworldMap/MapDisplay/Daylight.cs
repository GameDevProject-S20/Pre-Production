using UnityEngine;
using SIEvents;

public class Daylight : MonoBehaviour
{
    Quaternion targetRotation;

    private void Start() {
        EventManager.Instance.OnTimeChanged.AddListener((System.TimeSpan _) => RotateSun());
        transform.rotation = Quaternion.Euler(15 * (Clock.Instance.Time.Hour - 6), -90, -90);
    }

    void RotateSun() {
        targetRotation = Quaternion.Euler(15 * (Clock.Instance.Time.Hour - 6), -90, -90);
    }

    private void Update() {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
    }
}
