using System.Collections.Generic;
using ProceduralDungeon.Data.Types;

namespace ProceduralDungeon
{
    public class MapCleaner
    {
        private readonly TileType[,] _map;
        private readonly int _mapWidth;
        private readonly int _mapHeight;

        public MapCleaner(TileType[,] map, int mapWidth, int mapHeight)
        {
            _map = map;
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
        }

        public void CleanUnreachableFloor(Point startPoint)
        {
            var visited = RunBfsFromStart(startPoint);
            RemoveUnreachableFloor(visited);
        }

        private bool[,] RunBfsFromStart(Point startPoint)
        {
            var visited = new bool[_mapWidth, _mapHeight];
            var queue = new Queue<Point>();
            queue.Enqueue(startPoint);
            visited[startPoint.X, startPoint.Y] = true;

            int[] dx = { 0, 1, 0, -1 };
            int[] dy = { -1, 0, 1, 0 };

            while (queue.Count > 0)
            {
                var p = queue.Dequeue();
                for (var dir = 0; dir < 4; dir++)
                {
                    var nx = p.X + dx[dir];
                    var ny = p.Y + dy[dir];
                    if (nx < 0 || ny < 0 || nx >= _mapWidth || ny >= _mapHeight) continue;
                    if (visited[nx, ny]) continue;
                    if (_map[nx, ny] is not (TileType.Floor or TileType.Start or TileType.Exit)) continue;
                    visited[nx, ny] = true;
                    queue.Enqueue(new Point(nx, ny));
                }
            }

            return visited;
        }

        private void RemoveUnreachableFloor(bool[,] visited)
        {
            for (var x = 0; x < _mapWidth; x++)
            for (var y = 0; y < _mapHeight; y++)
            {
                if (_map[x, y] == TileType.Floor && !visited[x, y])
                    _map[x, y] = TileType.Empty;
            }
        }
    }
}