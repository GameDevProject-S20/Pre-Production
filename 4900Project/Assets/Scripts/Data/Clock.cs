using System;
using SIEvents;

[Serializable]
public class Clock
{
    private static Clock instance;

    public static Clock Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Clock();
            }
            return instance;
        }

        private set { }
    }

    public DateTime Time { get; private set; }

    private static readonly int DayStartHour = 6;

    public int StartYear = 2094;
    public int StartMonth = 3;
    public int StartDay = 7;
    public int StartHour = DayStartHour;

    private Clock()
    {
        Time = new DateTime(StartYear, StartMonth, StartDay, StartHour, 0, 0);
    }

    public void IncrementHour(int hours)
    {
        DateTime newTime = Time.AddHours(hours);
        TimeSpan diff = newTime.Subtract(Time);
        Time = newTime;

        EventManager.Instance.OnTimeChanged.Invoke(diff);
    }

    public void StartNewDay()
    {
        TimeSpan hour = new TimeSpan(DayStartHour, 0, 0);
        DateTime newTime = Time.AddDays(1).Date + hour;
        TimeSpan diff = newTime.Subtract(Time);
        Time = newTime;

        EventManager.Instance.OnTimeChanged.Invoke(diff);
    }
}