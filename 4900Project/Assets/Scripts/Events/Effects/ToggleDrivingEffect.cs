using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDrivingEffect : IEffect
{
    private DataTracker.TravelType travelType;

    public ToggleDrivingEffect(bool drive)
    {
        this.travelType = (drive) ? DataTracker.TravelType.TRUCK : DataTracker.TravelType.WALK;
    }

    public bool Apply()
    {
        DataTracker.Current.SetTravelType(travelType);
        return DataTracker.Current.TravelMode == travelType;
    }
}
