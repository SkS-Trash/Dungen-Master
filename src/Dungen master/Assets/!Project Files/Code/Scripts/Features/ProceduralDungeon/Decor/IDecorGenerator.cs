using System.Collections.Generic;
using ProceduralDungeon.Data.Types;

namespace ProceduralDungeon
{
    public interface IDecorGenerator
    {
        void GenerateDecor(TileType[,] map, List<Room> rooms);
        DecorType[,] DecorLayer { get; }
    }
}