using SIEvents;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class ClockUI : MonoBehaviour
{
    public Text text;

    [SerializeField]

    private void Start()
    {
        EventManager.Instance.OnTimeChanged.AddListener((TimeSpan _) => SetTime());
        SetTime();
    }

    private void SetTime()
    {
        text.text = string.Format("Date: {0}", Clock.Instance.Time.ToString("yyyy-MM-dd H:mm"));
    }
}
