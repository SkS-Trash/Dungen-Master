namespace ProceduralDungeon
{
    public interface IMapGenerator
    {
        TileType[,] Map { get; }
        List<Room> Rooms { get; }
        void GenerateMap(int roomCount, int roomMinSize, int roomMaxSize);
    }
}