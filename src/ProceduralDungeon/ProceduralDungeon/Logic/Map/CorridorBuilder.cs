namespace ProceduralDungeon
{
    public class CorridorBuilder
    {
        private readonly TileType[,] _map;
        private readonly int _mapWidth;
        private readonly int _mapHeight;

        public CorridorBuilder(TileType[,] map, int mapWidth, int mapHeight)
        {
            _map = map;
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
        }

        public void CreateCorridors(List<Room> rooms, IEnumerable<Edge> edges)
        {
            foreach (var edge in edges)
            {
                var a = rooms[edge.RoomA];
                var b = rooms[edge.RoomB];
                CreateCurvedCorridor(new Point(a.CenterX, a.CenterY), new Point(b.CenterX, b.CenterY));
            }
        }

        private void CreateCurvedCorridor(Point from, Point to, int radius = 1, double directionMemory = 0.7)
        {
            var path = BuildCurvedPath(from, to, directionMemory);
            DrawCorridorPath(path, radius);
        }

        private List<Point> BuildCurvedPath(Point from, Point to, double directionMemory)
        {
            var path = new List<Point>();
            int x = from.X, y = from.Y;
            int dx = Math.Sign(to.X - x), dy = Math.Sign(to.Y - y);
            int steps = 0, maxSteps = _mapWidth + _mapHeight;
            double lastDirX = dx, lastDirY = dy;
            var rand = new Random();
            while ((x != to.X || y != to.Y) && steps < maxSteps)
            {
                double tx = to.X - x, ty = to.Y - y;
                var len = Math.Sqrt(tx * tx + ty * ty);
                if (len > 0.1)
                {
                    tx /= len;
                    ty /= len;
                }

                var dirX = lastDirX * directionMemory + tx * (1 - directionMemory);
                var dirY = lastDirY * directionMemory + ty * (1 - directionMemory);
                dirX += (rand.NextDouble() - 0.5) * 0.5;
                dirY += (rand.NextDouble() - 0.5) * 0.5;
                var dlen = Math.Sqrt(dirX * dirX + dirY * dirY);
                if (dlen > 0.1)
                {
                    dirX /= dlen;
                    dirY /= dlen;
                }

                var stepX = Math.Abs(dirX) > Math.Abs(dirY) ? Math.Sign(dirX) : 0;
                var stepY = Math.Abs(dirY) >= Math.Abs(dirX) ? Math.Sign(dirY) : 0;
                if (rand.NextDouble() < 0.2)
                {
                    stepX = Math.Sign(dirX);
                    stepY = Math.Sign(dirY);
                }

                x = Math.Clamp(x + stepX, 1, _mapWidth - 2);
                y = Math.Clamp(y + stepY, 1, _mapHeight - 2);
                path.Add(new Point(x, y));
                lastDirX = dirX;
                lastDirY = dirY;
                steps++;
            }

            return path;
        }

        private void DrawCorridorPath(List<Point> path, int radius)
        {
            for (var i = 0; i < path.Count; i += 2)
            {
                var p = path[i];
                for (var dxr = -radius; dxr <= radius; dxr++)
                for (var dyr = -radius; dyr <= radius; dyr++)
                {
                    int nx = p.X + dxr, ny = p.Y + dyr;
                    if (nx >= 0 && nx < _mapWidth && ny >= 0 && ny < _mapHeight)
                        _map[nx, ny] = TileType.Floor;
                }
            }
        }
    }

    public struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}