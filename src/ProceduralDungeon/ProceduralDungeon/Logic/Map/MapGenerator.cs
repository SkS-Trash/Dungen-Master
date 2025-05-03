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
            // --- Poisson-disc sampling для генерации комнат ---
            var candidateRooms = GenerateRoomsPoissonDisc(roomCount, roomMinSize, roomMaxSize);
            if (candidateRooms.Count < roomCount)
                throw new InvalidOperationException($"Не удалось разместить все комнаты: {candidateRooms.Count} из {roomCount}. Попробуйте уменьшить roomCount или размеры комнат.");
            foreach (var room in candidateRooms)
                CreateRoom(room);
            Rooms.AddRange(candidateRooms);

            // --- Соединяем комнаты кривыми коридорами ---
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
            {
                var a = Rooms[edge.RoomA];
                var b = Rooms[edge.RoomB];
                CreateCurvedCorridor(new Point(a.CenterX, a.CenterY), new Point(b.CenterX, b.CenterY));
            }

            if (Rooms.Count > 0)
            {
                PlaceStartAndExit();
            }
            else throw new InvalidOperationException("Комнаты не сгенерированы.");

            CleanUnreachableFloor();
            CleanDungeon();
        }

        private void CleanUnreachableFloor()
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
                for (int dir = 0; dir < 4; dir++)
                {
                    int nx = p.X + dx[dir];
                    int ny = p.Y + dy[dir];
                    if (nx < 0 || ny < 0 || nx >= _mapWidth || ny >= _mapHeight) continue;
                    if (visited[nx, ny]) continue;
                    if (Map[nx, ny] is TileType.Floor or TileType.Start or TileType.Exit)
                    {
                        visited[nx, ny] = true;
                        queue.Enqueue(new Point(nx, ny));
                    }
                }
            }

            for (int x = 0; x < _mapWidth; x++)
                for (int y = 0; y < _mapHeight; y++)
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

        /// <summary>
        /// Создаёт кривой коридор между двумя точками с помощью random walk с памятью направления
        /// </summary>
        private void CreateCurvedCorridor(Point from, Point to, int radius = 1, double directionMemory = 0.7)
        {
            var path = new List<Point>();
            int x = from.X, y = from.Y;
            int dx = Math.Sign(to.X - x), dy = Math.Sign(to.Y - y);
            int steps = 0, maxSteps = _mapWidth + _mapHeight;
            double lastDirX = dx, lastDirY = dy;
            var rand = _random;
            while ((x != to.X || y != to.Y) && steps < maxSteps)
            {
                // Смесь направления к цели и предыдущего направления
                double tx = to.X - x, ty = to.Y - y;
                double len = Math.Sqrt(tx * tx + ty * ty);
                if (len > 0.1) { tx /= len; ty /= len; }
                double dirX = lastDirX * directionMemory + tx * (1 - directionMemory);
                double dirY = lastDirY * directionMemory + ty * (1 - directionMemory);
                // Добавляем случайный шум
                dirX += (rand.NextDouble() - 0.5) * 0.5;
                dirY += (rand.NextDouble() - 0.5) * 0.5;
                // Нормализация
                double dlen = Math.Sqrt(dirX * dirX + dirY * dirY);
                if (dlen > 0.1) { dirX /= dlen; dirY /= dlen; }
                // Дискретизация
                int stepX = Math.Abs(dirX) > Math.Abs(dirY) ? Math.Sign(dirX) : 0;
                int stepY = Math.Abs(dirY) >= Math.Abs(dirX) ? Math.Sign(dirY) : 0;
                // Иногда делаем диагональный шаг
                if (rand.NextDouble() < 0.2) { stepX = Math.Sign(dirX); stepY = Math.Sign(dirY); }
                // Делаем шаг
                x = Math.Clamp(x + stepX, 1, _mapWidth - 2);
                y = Math.Clamp(y + stepY, 1, _mapHeight - 2);
                path.Add(new Point(x, y));
                lastDirX = dirX; lastDirY = dirY;
                steps++;
            }
            // Сглаживание: простое удаление лишних точек (можно улучшить)
            for (int i = 0; i < path.Count; i += 2)
            {
                var p = path[i];
                for (int dxr = -radius; dxr <= radius; dxr++)
                    for (int dyr = -radius; dyr <= radius; dyr++)
                    {
                        int nx = p.X + dxr, ny = p.Y + dyr;
                        if (nx >= 0 && nx < _mapWidth && ny >= 0 && ny < _mapHeight)
                            Map[nx, ny] = TileType.Floor;
                    }
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

        /// <summary>
        /// Генерация прямоугольных комнат с помощью Poisson-disc sampling
        /// </summary>
        public List<Room> GenerateRoomsPoissonDisc(int roomCount, int roomMinSize, int roomMaxSize, int k = 30)
        {
            var rooms = new List<Room>();
            var candidates = new List<(int x, int y)>();
            int minDist = roomMinSize + 2; // минимальное расстояние между центрами комнат

            // Стартовая точка
            int startX = _random.Next(roomMinSize, _mapWidth - roomMinSize);
            int startY = _random.Next(roomMinSize, _mapHeight - roomMinSize);
            candidates.Add((startX, startY));

            int attempts = 0;
            while (rooms.Count < roomCount && candidates.Count > 0 && attempts < roomCount * k)
            {
                int idx = _random.Next(candidates.Count);
                var (cx, cy) = candidates[idx];
                candidates.RemoveAt(idx);

                // Случайный размер комнаты
                int width = _random.Next(roomMinSize, roomMaxSize + 1);
                int height = _random.Next(roomMinSize, roomMaxSize + 1);
                int x = Math.Clamp(cx - width / 2, 1, _mapWidth - width - 1);
                int y = Math.Clamp(cy - height / 2, 1, _mapHeight - height - 1);
                var room = new Room(x, y, width, height, RoomType.Normal);

                // Проверка на пересечения
                bool intersects = false;
                foreach (var other in rooms)
                    if (room.Intersects(other)) { intersects = true; break; }
                if (intersects) { attempts++; continue; }

                rooms.Add(room);
                attempts = 0;

                // Добавляем новые кандидаты вокруг центра
                for (int i = 0; i < k; i++)
                {
                    double angle = 2 * Math.PI * _random.NextDouble();
                    double dist = minDist + (_random.NextDouble() * minDist);
                    int nx = (int)(cx + Math.Cos(angle) * dist);
                    int ny = (int)(cy + Math.Sin(angle) * dist);
                    if (nx > roomMinSize && nx < _mapWidth - roomMinSize && ny > roomMinSize && ny < _mapHeight - roomMinSize)
                        candidates.Add((nx, ny));
                }
            }
            return rooms;
        }
    }
}