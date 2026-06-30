using ProceduralDungeon.Data;

namespace ProceduralDungeon
{
    public class WallFixer
    {
        private readonly TileType[,] _map;
        private readonly int _mapWidth;
        private readonly int _mapHeight;

        public WallFixer(TileType[,] map, int mapWidth, int mapHeight)
        {
            _map = map;
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
        }

        public void CleanDungeon()
        {
            AddWallsAroundFloor();
            for (var x = 0; x < _mapWidth; x++)
            for (var y = 0; y < _mapHeight; y++)
            {
                if (_map[x, y] != TileType.Wall) continue;
                if (!ShouldKeepWall(x, y))
                    _map[x, y] = TileType.Empty;
            }
        }

        private void AddWallsAroundFloor()
        {
            for (var x = 0; x < _mapWidth; x++)
            for (var y = 0; y < _mapHeight; y++)
            {
                if (_map[x, y] != TileType.Empty) continue;
                if (HasAdjacentFloor(x, y))
                    _map[x, y] = TileType.Wall;
            }
        }

        private bool HasAdjacentFloor(int x, int y)
        {
            for (var dx = -1; dx <= 1; dx++)
            for (var dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                int nx = x + dx, ny = y + dy;
                if (nx < 0 || ny < 0 || nx >= _mapWidth || ny >= _mapHeight) continue;
                var neighbor = _map[nx, ny];
                if (neighbor is TileType.Floor or TileType.Start or TileType.Exit)
                    return true;
            }

            return false;
        }

        private bool ShouldKeepWall(int x, int y)
        {
            for (var dx = -1; dx <= 1; dx++)
            for (var dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                int nx = x + dx, ny = y + dy;
                if (nx < 0 || ny < 0 || nx >= _mapWidth || ny >= _mapHeight) continue;
                var neighbor = _map[nx, ny];
                if (neighbor is TileType.Floor or TileType.Start or TileType.Exit)
                    return true;
            }

            return false;
        }
    }
}