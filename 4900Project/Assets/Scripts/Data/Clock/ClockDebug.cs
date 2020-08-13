using UnityEngine;

public class ClockDebug : MonoBehaviour
{
    public void SetJustBeforeNightTime()
    {
        Clock.Instance.DeveloperTools.SetHour(Clock.NightStartHour - 1);
    }
}
