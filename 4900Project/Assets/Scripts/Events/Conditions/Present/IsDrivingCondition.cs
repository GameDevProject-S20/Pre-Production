public class IsDrivingCondition : IPresentCondition
{
    private DataTracker.TravelType requiredTravelType;

    public IsDrivingCondition(bool isDriving)
    {
        requiredTravelType = (isDriving) ? DataTracker.TravelType.TRUCK : DataTracker.TravelType.WALK;
    }
    
    public bool IsSatisfied()
    {
        return DataTracker.Current.TravelMode == requiredTravelType;
    }
}
