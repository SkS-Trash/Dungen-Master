using System.Collections.Generic;
using ProceduralDungeon.Data;

namespace ProceduralDungeon
{
    public interface IMapGenerator
    {
        TileType[,] Map { get; }
        List<Room> Rooms { get; }
        void GenerateMap();
    }
}