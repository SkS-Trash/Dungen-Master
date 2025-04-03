using System.Collections.Generic;

namespace ProceduralDungeon.Decor
{
    public interface IDecorGenerator
    {
        void GenerateDecor(TileType[,] map, List<Room> rooms);
        DecorType[,] DecorLayer { get; }
    }
}