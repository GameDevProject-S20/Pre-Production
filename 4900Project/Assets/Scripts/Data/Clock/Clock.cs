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

    public static readonly int DayStartHour = 6;

    public static readonly int NightStartHour = 20;

    public int StartYear = 2094;
    public int StartMonth = 3;
    public int StartDay = 7;
    public int StartHour = DayStartHour;

    public DevTools DeveloperTools { get; private set; }

    private Clock()
    {
        Time = new DateTime(StartYear, StartMonth, StartDay, StartHour, 0, 0);
        DeveloperTools = new DevTools(this);
    }

    public void IncrementHour(int hours)
    {
        DateTime newTime = Time.AddHours(hours);
        TimeSpan diff = newTime.Subtract(Time);
        Time = newTime;

        EventManager.Instance.OnTimeChanged.Invoke(diff);
    }

    public bool IsDay()
    {
        return Time.Hour >= DayStartHour && Time.Hour < NightStartHour;
    }


    public void StartNewDay()
    {
        TimeSpan hour = new TimeSpan(DayStartHour, 0, 0);
        DateTime newTime = Time.AddDays(1).Date + hour;
        TimeSpan diff = newTime.Subtract(Time);
        Time = newTime;

        EventManager.Instance.OnTimeChanged.Invoke(diff);
    }

    public class DevTools
    {
        private Clock clock;

        private DevTools() { }

        public DevTools(Clock clock)
        {
            this.clock = clock;
            clock.DeveloperTools = this;
        }

        public void SetHour(int hour)
        {
            DateTime newTime = clock.Time.Date.AddHours(hour);
            TimeSpan diff = newTime.Subtract(clock.Time);
            clock.Time = newTime;

            EventManager.Instance.OnTimeChanged.Invoke(diff);
        }
    }
}