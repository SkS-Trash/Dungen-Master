using System.Buffers;

namespace ProceduralDungeon
{
    public class CorridorBuilder
    {
        private readonly TileType[,] _map;
        private readonly int _mapWidth;
        private readonly int _mapHeight;
        private readonly Random _random;

        public CorridorBuilder(TileType[,] map, int mapWidth, int mapHeight, Random random)
        {
            _map = map;
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
            _random = random;
        }

        public void CreateCorridors(List<Room> rooms, IEnumerable<Edge> edges)
        {
            foreach (var edge in edges)
            {
                var a = rooms[edge.RoomA];
                var b = rooms[edge.RoomB];
                var from = new Point(a.CenterX, a.CenterY);
                var to = new Point(b.CenterX, b.CenterY);
                int method = _random.Next(4);
                switch (method)
                {
                    case 0:
                        CreateAStarCorridor(from, to);
                        break;
                    case 1:
                        CreateLCorridor(from, to);
                        break;
                    case 2:
                        CreateZigzagCorridor(from, to);
                        break;
                    case 3:
                        CreateNoisyCorridor(from, to);
                        break;
                }
            }
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

        private void CreateAStarCorridor(Point from, Point to, int radius = 1)
        {
            var path = BuildAStarPath(from, to);
            DrawCorridorPath(path, radius);
        }

        private List<Point> BuildAStarPath(Point from, Point to)
        {
            var comparer = Comparer<(double f, int count, Point p)>.Create((a, b) =>
            {
                var cmp = a.f.CompareTo(b.f);
                if (cmp != 0) return cmp;
                cmp = a.count.CompareTo(b.count);
                return cmp != 0
                    ? cmp
                    : a.p.X != b.p.X
                        ? a.p.X.CompareTo(b.p.X)
                        : a.p.Y.CompareTo(b.p.Y);
            });

            var openSet = new SortedSet<(double f, int count, Point p)>(comparer);
            var cameFrom = new Dictionary<Point, Point>();
            var gScore = new Dictionary<Point, double>();
            var fScore = new Dictionary<Point, double>();
            var closedSet = new HashSet<Point>();
            var counter = 0;

            gScore[from] = 0;
            fScore[from] = Heuristic(from, to);
            openSet.Add((fScore[from], counter++, from));

            int[] dx = [0, 1, 0, -1];
            int[] dy = [-1, 0, 1, 0];

            while (openSet.Count > 0)
            {
                var current = openSet.Min.p;
                if (current.X == to.X && current.Y == to.Y)
                {
                    var path = ArrayPool<List<Point>>.Shared.Rent(1)[0] ?? [];
                    path.Clear();
                    path.AddRange(ReconstructPath(cameFrom, current));
                    return path;
                }

                openSet.Remove(openSet.Min);
                closedSet.Add(current);

                for (var dir = 0; dir < 4; dir++)
                {
                    var nx = current.X + dx[dir];
                    var ny = current.Y + dy[dir];
                    if (nx < 0 || ny < 0 || nx >= _mapWidth || ny >= _mapHeight)
                        continue;
                    var neighbor = new Point(nx, ny);
                    if (closedSet.Contains(neighbor))
                        continue;
                    var tentativeG = gScore[current] + GetTileCost(nx, ny);
                    if (gScore.TryGetValue(neighbor, out var oldG) && tentativeG >= oldG)
                        continue;
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    fScore[neighbor] = tentativeG + Heuristic(neighbor, to);
                    openSet.Add((fScore[neighbor], counter++, neighbor));
                }
            }

            return ArrayPool<List<Point>>.Shared.Rent(1)[0] ?? [];
        }

        private double Heuristic(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        private double GetTileCost(int x, int y)
        {
            switch (_map[x, y])
            {
                case TileType.Floor:
                case TileType.Start:
                case TileType.Exit:
                    return 1.0;
                case TileType.Wall:
                    return 100.0;
                default:
                    return 10.0;
            }
        }

        private List<Point> ReconstructPath(Dictionary<Point, Point> cameFrom, Point current)
        {
            var path = ArrayPool<List<Point>>.Shared.Rent(1)[0] ?? [];
            path.Clear();
            path.Add(current);
            while (cameFrom.TryGetValue(current, out var prev))
            {
                current = prev;
                path.Add(current);
            }

            path.Reverse();
            return path;
        }

        private void CreateLCorridor(Point from, Point to, int radius = 1)
        {
            var path = new List<Point>();
            if (_random.Next(2) == 0)
            {
                for (int x = from.X; x != to.X; x += Math.Sign(to.X - from.X))
                    path.Add(new Point(x, from.Y));
                for (int y = from.Y; y != to.Y; y += Math.Sign(to.Y - from.Y))
                    path.Add(new Point(to.X, y));
            }
            else
            {
                for (int y = from.Y; y != to.Y; y += Math.Sign(to.Y - from.Y))
                    path.Add(new Point(from.X, y));
                for (int x = from.X; x != to.X; x += Math.Sign(to.X - from.X))
                    path.Add(new Point(x, to.Y));
            }

            path.Add(to);
            DrawCorridorPath(path, radius);
        }

        private void CreateZigzagCorridor(Point from, Point to, int radius = 1)
        {
            var path = new List<Point>();
            int x = from.X, y = from.Y;
            path.Add(new Point(x, y));
            int dx = Math.Sign(to.X - x);
            int dy = Math.Sign(to.Y - y);
            int steps = Math.Max(Math.Abs(to.X - x), Math.Abs(to.Y - y));
            for (int i = 0; i < steps; i++)
            {
                if (i % 2 == 0 && x != to.X) x += dx;
                else if (y != to.Y) y += dy;
                path.Add(new Point(x, y));
            }

            DrawCorridorPath(path, radius);
        }

        private void CreateNoisyCorridor(Point from, Point to, int radius = 1)
        {
            var path = new List<Point>();
            int x = from.X, y = from.Y;
            path.Add(new Point(x, y));
            while (x != to.X || y != to.Y)
            {
                if (x != to.X && y != to.Y)
                {
                    if (_random.Next(2) == 0) x += Math.Sign(to.X - x);
                    else y += Math.Sign(to.Y - y);
                }
                else if (x != to.X) x += Math.Sign(to.X - x);
                else if (y != to.Y) y += Math.Sign(to.Y - y);

                if (_random.Next(6) == 0)
                {
                    if (x != to.X && y > 1 && y < _mapHeight - 2) y += _random.Next(2) == 0 ? 1 : -1;
                    else if (y != to.Y && x > 1 && x < _mapWidth - 2) x += _random.Next(2) == 0 ? 1 : -1;
                }

                path.Add(new Point(x, y));
            }

            DrawCorridorPath(path, radius);
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