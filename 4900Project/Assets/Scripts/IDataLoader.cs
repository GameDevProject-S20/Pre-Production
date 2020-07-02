public interface IDataLoader
{
    OverworldMap.LocationGraph LoadMap();
    PlayerData LoadPlayer();
}
