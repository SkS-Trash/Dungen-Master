using System.Collections.Generic;

namespace ProceduralDungeon
{
    public interface IDecorGenerator
    {
        void GenerateDecor(TileType[,] map, List<Room> rooms);
        DecorType[,] DecorLayer { get; }
    }
}