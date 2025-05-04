using ProceduralDungeon.Data.Types;

namespace ProceduralDungeon
{
    public class DecorDistanceChecker
    {
        public bool HasNearbyDecor(int x, int y, int radius, DecorType[,] decorLayer)
        {
            for (var dx = -radius; dx <= radius; dx++)
            for (var dy = -radius; dy <= radius; dy++)
            {
                var nx = x + dx;
                var ny = y + dy;
                if (nx >= 0 && nx < decorLayer.GetLength(0) &&
                    ny >= 0 && ny < decorLayer.GetLength(1) &&
                    decorLayer[nx, ny] != DecorType.None)
                {
                    return true;
                }
            }

            return false;
        }
    }
}