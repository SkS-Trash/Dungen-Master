using System.Collections.Generic;
using ProceduralDungeon.Data.Types;

namespace ProceduralDungeon
{
    public interface IMapGenerator
    {
        TileType[,] Map { get; }
        List<Room> Rooms { get; }
        void GenerateMap();
    }
}