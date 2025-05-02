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
            var candidateRooms = new List<Room>();
            int attempts = 0;
            while (candidateRooms.Count < roomCount && attempts < roomCount * 10)
            {
                var roomWidth = _random.Next(roomMinSize, roomMaxSize + 1);
                var roomHeight = _random.Next(roomMinSize, roomMaxSize + 1);
                var roomX = _random.Next(1, _mapWidth - roomWidth - 1);
                var roomY = _random.Next(1, _mapHeight - roomHeight - 1);

                var roll = _random.NextDouble();
                var roomType = roll switch
                {
                    < 0.05 => RoomType.Hard,
                    < 0.1 => RoomType.Treatment,
                    < 0.25 => RoomType.Trap,
                    _ => RoomType.Normal
                };

                var newRoom = new Room(roomX, roomY, roomWidth, roomHeight, roomType);
                if (candidateRooms.Any(otherRoom => newRoom.Intersects(otherRoom)))
                {
                    attempts++;
                    continue;
                }
                candidateRooms.Add(newRoom);
                attempts = 0;
            }

            foreach (var room in candidateRooms)
                CreateRoom(room);
            Rooms.AddRange(candidateRooms);

            var edges = new List<Edge>();
            int k = 3;
            for (int i = 0; i < Rooms.Count; i++)
            {
                var dists = new List<(int j, float dist)>();
                for (int j = 0; j < Rooms.Count; j++)
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

            var mstEdges = KruskalMST(Rooms.Count, edges);

            var extraEdges = new List<Edge>();
            var nonMstEdges = edges.Except(mstEdges).ToList();
            int extraCount = Math.Min(2, nonMstEdges.Count);
            for (int i = 0; i < extraCount; i++)
            {
                var idx = _random.Next(nonMstEdges.Count);
                extraEdges.Add(nonMstEdges[idx]);
                nonMstEdges.RemoveAt(idx);
            }

            foreach (var edge in mstEdges.Concat(extraEdges))
                CreateCorridor(Rooms[edge.RoomA], Rooms[edge.RoomB]);

            if (Rooms.Count > 0)
            {
                PlaceStartAndExit();
            }
            else throw new InvalidOperationException("Комнаты не сгенерированы.");

            CleanDungeon();
        }

        private void CleanDungeon()
        {
            for (var x = 0; x < _mapWidth; x++)
                for (var y = 0; y < _mapHeight; y++)
                {
                    if (Map[x, y] != TileType.Wall) continue;

                    var keepWall = false;
                    for (var dx = -1; dx <= 1 && !keepWall; dx++)
                        for (var dy = -1; dy <= 1; dy++)
                        {
                            if (dx == 0 && dy == 0) continue;

                            int nx = x + dx, ny = y + dy;

                            if (nx < 0 || ny < 0 || nx >= _mapWidth || ny >= _mapHeight) continue;

                            var neighbor = Map[nx, ny];

                            if (neighbor is not (TileType.Floor or TileType.Start or TileType.Exit)) continue;

                            keepWall = true;
                            break;
                        }

                    if (!keepWall)
                        Map[x, y] = TileType.Empty;
                }
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

        private void CreateCorridor(Room roomA, Room roomB)
        {
            var x1 = roomA.CenterX;
            var y1 = roomA.CenterY;
            var x2 = roomB.CenterX;
            var y2 = roomB.CenterY;

            if (_random.Next(0, 2) == 0)
            {
                CreateHorizontalCorridor(x1, x2, y1);
                CreateVerticalCorridor(y1, y2, x2);
            }
            else
            {
                CreateVerticalCorridor(y1, y2, x1);
                CreateHorizontalCorridor(x1, x2, y2);
            }
        }

        private void CreateHorizontalCorridor(int x1, int x2, int y)
        {
            var start = Math.Min(x1, x2);
            var end = Math.Max(x1, x2);

            for (var x = start; x <= end; x++)
                for (var dy = -1; dy <= 1; dy++)
                {
                    var ny = y + dy;
                    if (ny >= 0 && ny < _mapHeight)
                        Map[x, ny] = TileType.Floor;
                }
        }

        private void CreateVerticalCorridor(int y1, int y2, int x)
        {
            var start = Math.Min(y1, y2);
            var end = Math.Max(y1, y2);

            for (var y = start; y <= end; y++)
                for (var dx = -1; dx <= 1; dx++)
                {
                    var nx = x + dx;
                    if (nx >= 0 && nx < _mapWidth)
                        Map[nx, y] = TileType.Floor;
                }
        }

        // --- Edge и Kruskal ---
        private class Edge
        {
            public int RoomA { get; }
            public int RoomB { get; }
            public float Weight { get; }
            public Edge(int a, int b, float w) { RoomA = a; RoomB = b; Weight = w; }
        }

        private List<Edge> KruskalMST(int roomCount, List<Edge> edges)
        {
            var parent = new int[roomCount];
            for (int i = 0; i < roomCount; i++) parent[i] = i;
            int Find(int x) => parent[x] == x ? x : (parent[x] = Find(parent[x]));
            void Union(int x, int y) { parent[Find(x)] = Find(y); }

            var mst = new List<Edge>();
            foreach (var edge in edges.OrderBy(e => e.Weight))
            {
                int a = Find(edge.RoomA), b = Find(edge.RoomB);
                if (a != b)
                {
                    mst.Add(edge);
                    Union(a, b);
                }
            }
            return mst;
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
    }
}