namespace ProceduralDungeon
{
    public class MapGenerator : IMapGenerator
    {
        public TileType[,] Map { get; }
        public List<Room> Rooms { get; } = [];

        private readonly int _mapWidth;
        private readonly int _mapHeight;
        private readonly Random _random;

        private Point _startPoint = new(0, 0);
        private Point _exitPoint = new(0, 0);

        public MapGenerator(int width, int height, Random random)
        {
            _mapWidth = width;
            _mapHeight = height;

            Map = new TileType[_mapWidth, _mapHeight];

            for (var x = 0; x < _mapWidth; x++)
            for (var y = 0; y < _mapHeight; y++)
                Map[x, y] = TileType.Wall;

            _random = random;
        }

        public void GenerateMap(int roomCount, int roomMinSize, int roomMaxSize)
        {
            Rooms.Clear();
            var candidateRooms = GenerateRoomsPoissonDisc(roomCount, roomMinSize, roomMaxSize);
            ValidateRoomCount(candidateRooms, roomCount);
            CreateAllRooms(candidateRooms);
            Rooms.AddRange(candidateRooms);

            var edges = GenerateRoomEdges();
            var mstEdges = KruskalMST(Rooms.Count, edges);
            var extraEdges = SelectExtraEdges(edges, mstEdges);
            CreateCorridors(mstEdges.Concat(extraEdges));

            if (Rooms.Count > 0)
            {
                PlaceStartAndExit();
            }
            else throw new InvalidOperationException("Комнаты не сгенерированы.");

            CleanUnreachableFloor();
            CleanDungeon();
        }

        private void ValidateRoomCount(List<Room> candidateRooms, int roomCount)
        {
            if (candidateRooms.Count < roomCount)
                throw new InvalidOperationException(
                    $"Не удалось разместить все комнаты: {candidateRooms.Count} из {roomCount}. Попробуйте уменьшить roomCount или размеры комнат.");
        }

        private void CreateAllRooms(List<Room> candidateRooms)
        {
            foreach (var room in candidateRooms)
                CreateRoom(room);
        }

        private List<Edge> GenerateRoomEdges()
        {
            var edges = new List<Edge>();
            var k = 3;
            for (var i = 0; i < Rooms.Count; i++)
            {
                var dists = new List<(int j, float dist)>();
                for (var j = 0; j < Rooms.Count; j++)
                {
                    if (i == j) continue;
                    dists.Add((j, CalculateDistance(Rooms[i], Rooms[j])));
                }

                foreach (var (j, dist) in dists.OrderBy(t => t.dist).Take(k))
                {
                    if (!edges.Any(e => (e.RoomA == i && e.RoomB == j) || (e.RoomA == j && e.RoomB == i)))
                        edges.Add(new Edge(i, j, dist));
                }
            }

            return edges;
        }

        private List<Edge> SelectExtraEdges(List<Edge> edges, List<Edge> mstEdges)
        {
            var extraEdges = new List<Edge>();
            var nonMstEdges = edges.Except(mstEdges).ToList();
            var extraCount = Math.Min(2, nonMstEdges.Count);
            for (var i = 0; i < extraCount; i++)
            {
                var idx = _random.Next(nonMstEdges.Count);
                extraEdges.Add(nonMstEdges[idx]);
                nonMstEdges.RemoveAt(idx);
            }

            return extraEdges;
        }

        private void CreateCorridors(IEnumerable<Edge> edges)
        {
            foreach (var edge in edges)
            {
                var a = Rooms[edge.RoomA];
                var b = Rooms[edge.RoomB];
                CreateCurvedCorridor(new Point(a.CenterX, a.CenterY), new Point(b.CenterX, b.CenterY));
            }
        }

        private void CleanUnreachableFloor()
        {
            var visited = RunBfsFromStart();
            RemoveUnreachableFloor(visited);
        }

        private bool[,] RunBfsFromStart()
        {
            var visited = new bool[_mapWidth, _mapHeight];
            var queue = new Queue<Point>();
            queue.Enqueue(_startPoint);
            visited[_startPoint.X, _startPoint.Y] = true;

            int[] dx = [0, 1, 0, -1];
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
                    if (Map[nx, ny] is TileType.Floor or TileType.Start or TileType.Exit)
                    {
                        visited[nx, ny] = true;
                        queue.Enqueue(new Point(nx, ny));
                    }
                }
            }

            return visited;
        }

        private void RemoveUnreachableFloor(bool[,] visited)
        {
            for (var x = 0; x < _mapWidth; x++)
            for (var y = 0; y < _mapHeight; y++)
            {
                if (Map[x, y] == TileType.Floor && !visited[x, y])
                    Map[x, y] = TileType.Empty;
            }
        }

        private void CleanDungeon()
        {
            for (var x = 0; x < _mapWidth; x++)
            for (var y = 0; y < _mapHeight; y++)
            {
                if (Map[x, y] != TileType.Wall) continue;

                if (!ShouldKeepWall(x, y))
                    Map[x, y] = TileType.Empty;
            }
        }

        private bool ShouldKeepWall(int x, int y)
        {
            for (var dx = -1; dx <= 1; dx++)
            for (var dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int nx = x + dx, ny = y + dy;

                if (nx < 0 || ny < 0 || nx >= _mapWidth || ny >= _mapHeight) continue;

                var neighbor = Map[nx, ny];

                if (neighbor is TileType.Floor or TileType.Start or TileType.Exit)
                    return true;
            }

            return false;
        }

        private void PlaceStartAndExit()
        {
            Room? startRoom = null;
            Room? exitRoom = null;
            float maxDistance = 0;

            foreach (var roomA in Rooms)
            foreach (var roomB in Rooms)
            {
                if (roomA == roomB) continue;

                var distance = CalculateDistance(roomA, roomB);

                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    startRoom = roomA;
                    exitRoom = roomB;
                }
            }

            if (startRoom != null && exitRoom != null)
            {
                _startPoint = new Point(startRoom.CenterX, startRoom.CenterY);
                _exitPoint = new Point(exitRoom.CenterX, exitRoom.CenterY);
            }
            else if (Rooms.Count == 1)
            {
                var room = Rooms[0];
                _startPoint = _exitPoint = new Point(room.CenterX, room.CenterY);
            }

            Map[_startPoint.X, _startPoint.Y] = TileType.Start;
            Map[_exitPoint.X, _exitPoint.Y] = TileType.Exit;
        }

        private float CalculateDistance(Room a, Room b)
        {
            var dx = a.CenterX - b.CenterX;
            var dy = a.CenterY - b.CenterY;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        private void CreateRoom(Room room)
        {
            for (var x = room.X; x < room.X + room.Width; x++)
            for (var y = room.Y; y < room.Y + room.Height; y++)
                Map[x, y] = TileType.Floor;
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
            var rand = _random;
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
                        Map[nx, ny] = TileType.Floor;
                }
            }
        }

        private class Edge
        {
            public int RoomA { get; }
            public int RoomB { get; }
            public float Weight { get; }

            public Edge(int a, int b, float w)
            {
                RoomA = a;
                RoomB = b;
                Weight = w;
            }
        }

        private List<Edge> KruskalMST(int roomCount, List<Edge> edges)
        {
            var parent = new int[roomCount];
            for (var i = 0; i < roomCount; i++) parent[i] = i;

            var mst = new List<Edge>();
            foreach (var edge in edges.OrderBy(e => e.Weight))
            {
                int a = FindSet(edge.RoomA, parent), b = FindSet(edge.RoomB, parent);
                if (a != b)
                {
                    mst.Add(edge);
                    UnionSets(a, b, parent);
                }
            }

            return mst;
        }

        private int FindSet(int x, int[] parent)
        {
            return parent[x] == x ? x : parent[x] = FindSet(parent[x], parent);
        }

        private void UnionSets(int x, int y, int[] parent)
        {
            parent[FindSet(x, parent)] = FindSet(y, parent);
        }

        private struct Point
        {
            public int X { get; }
            public int Y { get; }

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        private List<Room> GenerateRoomsPoissonDisc(int roomCount, int roomMinSize, int roomMaxSize, int k = 30)
        {
            var rooms = new List<Room>();
            var candidates = new List<(int x, int y)>();
            var minDist = roomMinSize + 2;

            var (startX, startY) = GetRandomStartPoint(roomMinSize);
            candidates.Add((startX, startY));

            var attempts = 0;
            while (rooms.Count < roomCount && candidates.Count > 0 && attempts < roomCount * k)
            {
                var idx = _random.Next(candidates.Count);
                var (cx, cy) = candidates[idx];
                candidates.RemoveAt(idx);

                var room = TryCreateRoom(cx, cy, roomMinSize, roomMaxSize);
                if (room == null || HasIntersection(room, rooms))
                {
                    attempts++;
                    continue;
                }

                rooms.Add(room);
                attempts = 0;

                AddNewCandidates(cx, cy, minDist, k, roomMinSize, candidates);
            }

            return rooms;
        }

        private (int x, int y) GetRandomStartPoint(int roomMinSize)
        {
            var x = _random.Next(roomMinSize, _mapWidth - roomMinSize);
            var y = _random.Next(roomMinSize, _mapHeight - roomMinSize);
            return (x, y);
        }

        private Room? TryCreateRoom(int cx, int cy, int roomMinSize, int roomMaxSize)
        {
            var width = _random.Next(roomMinSize, roomMaxSize + 1);
            var height = _random.Next(roomMinSize, roomMaxSize + 1);
            var x = Math.Clamp(cx - width / 2, 1, _mapWidth - width - 1);
            var y = Math.Clamp(cy - height / 2, 1, _mapHeight - height - 1);
            return new Room(x, y, width, height, RoomType.Normal);
        }

        private bool HasIntersection(Room room, List<Room> rooms)
        {
            return rooms.Any(other => room.Intersects(other));
        }

        private void AddNewCandidates(int cx, int cy, int minDist, int k, int roomMinSize,
            List<(int x, int y)> candidates)
        {
            for (var i = 0; i < k; i++)
            {
                var angle = 2 * Math.PI * _random.NextDouble();
                var dist = minDist + _random.NextDouble() * minDist;
                var nx = (int)(cx + Math.Cos(angle) * dist);
                var ny = (int)(cy + Math.Sin(angle) * dist);
                if (nx > roomMinSize && nx < _mapWidth - roomMinSize && ny > roomMinSize &&
                    ny < _mapHeight - roomMinSize)
                    candidates.Add((nx, ny));
            }
        }
    }
}