using System.Collections.Generic;
using ProceduralDungeon.Data;

namespace ProceduralDungeon
{
    public interface IDecorGenerator
    {
        void GenerateDecor(TileType[,] map, List<Room> rooms);
        DecorType[,] DecorLayer { get; }
    }
}