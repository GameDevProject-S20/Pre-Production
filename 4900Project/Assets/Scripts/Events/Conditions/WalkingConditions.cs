using UnityEngine;

public class WalkingConditions
{
    public class IsWalkingCondition : IPresentCondition
    {
        public bool IsSatisfied()
        {
            return DataTracker.Current.travelMode == DataTracker.TravelType.WALK;
        }
    }

    public class IsDrivingCondition : IPresentCondition
    {
        public bool IsSatisfied()
        {
            return DataTracker.Current.travelMode == DataTracker.TravelType.TRUCK;
        }
    }
}
