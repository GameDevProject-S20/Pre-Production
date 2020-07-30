/// <summary>
/// Is satisfied when a particular game state is met at the time of checking.
/// </summary>
public interface IPresentCondition
{
    bool IsSatisfied();
}
