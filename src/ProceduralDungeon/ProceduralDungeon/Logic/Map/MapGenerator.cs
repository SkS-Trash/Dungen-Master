namespace ProceduralDungeon
{
    public class MapGenerator : IMapGenerator
    {
        public readonly int MapWidth;
        public readonly int MapHeight;
        public TileType[,] Map { get; }
        public List<Room> Rooms { get; } = [];

        private readonly Random _random = new();

        private Point _startPoint = new(0, 0);
        private Point _exitPoint = new(0, 0);

        public MapGenerator(int width, int height)
        {
            MapWidth = width;
            MapHeight = height;
            Map = new TileType[MapWidth, MapHeight];

            for (var x = 0; x < MapWidth; x++)
            for (var y = 0; y < MapHeight; y++)
                Map[x, y] = TileType.Wall;
        }

        public void GenerateMap(int roomCount, int roomMinSize, int roomMaxSize)
        {
            for (var i = 0; i < roomCount; i++)
            {
                var roomWidth = _random.Next(roomMinSize, roomMaxSize + 1);
                var roomHeight = _random.Next(roomMinSize, roomMaxSize + 1);
                var roomX = _random.Next(1, MapWidth - roomWidth - 1);
                var roomY = _random.Next(1, MapHeight - roomHeight - 1);

                var roll = _random.NextDouble();
                var roomType = roll switch
                {
                    < 0.05 => RoomType.Hard,
                    < 0.1 => RoomType.Treatment,
                    < 0.25 => RoomType.Trap,
                    _ => RoomType.Normal
                };

                var newRoom = new Room(roomX, roomY, roomWidth, roomHeight, roomType);

                if (Rooms.Any(otherRoom => newRoom.Intersects(otherRoom)))
                    continue;

                CreateRoom(newRoom);

                if (Rooms.Count > 0)
                {
                    var prevRoom = Rooms[^1];
                    CreateCorridor(prevRoom, newRoom);
                }

                Rooms.Add(newRoom);
            }


            if (Rooms.Count > 0)
            {
                PlaceStartAndExit();
            }
            else
            {
                throw new InvalidOperationException("No rooms generated.");
            }
        }

        private void PlaceStartAndExit()
        {
            Room startRoom = null;
            Room exitRoom = null;
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
            int dx = a.CenterX - b.CenterX;
            int dy = a.CenterY - b.CenterY;
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
                if (ny >= 0 && ny < MapHeight)
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
                if (nx >= 0 && nx < MapWidth)
                    Map[nx, y] = TileType.Floor;
            }
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