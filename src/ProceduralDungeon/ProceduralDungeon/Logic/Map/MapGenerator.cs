namespace ProceduralDungeon
{
    public class MapGenerator : IMapGenerator
    {
        public readonly int MapWidth;
        public readonly int MapHeight;

        public TileType[,] Map { get; }
        public List<Room> Rooms { get; }

        private readonly Random _random = new();

        public MapGenerator(int width, int height)
        {
            MapWidth = width;
            MapHeight = height;
            Map = new TileType[MapWidth, MapHeight];
            Rooms = new List<Room>();

            for (var x = 0; x < MapWidth; x++)
            for (var y = 0; y < MapHeight; y++)
            {
                Map[x, y] = TileType.Wall;
            }
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

                if (Rooms.Any(otherRoom => newRoom.Intersects(otherRoom))) continue;

                CreateRoom(newRoom);

                if (Rooms.Count > 0)
                {
                    var prevRoom = Rooms[^1];
                    CreateCorridor(prevRoom, newRoom);
                }

                Rooms.Add(newRoom);
            }
        }

        private void CreateRoom(Room room)
        {
            for (var x = room.X; x < room.X + room.Width; x++)
            for (var y = room.Y; y < room.Y + room.Height; y++)
            {
                Map[x, y] = TileType.Floor;
            }
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
                PlaceDoor(Math.Min(x1, x2), y1, roomA);
                PlaceDoor(x2, Math.Min(y1, y2), roomB);
            }
            else
            {
                CreateVerticalCorridor(y1, y2, x1);
                CreateHorizontalCorridor(x1, x2, y2);
                PlaceDoor(x1, Math.Min(y1, y2), roomA);
                PlaceDoor(Math.Min(x1, x2), y2, roomB);
            }
        }

        private void PlaceDoor(int x, int y, Room room)
        {
            if (x >= room.X && x < room.X + room.Width && y >= room.Y && y < room.Y + room.Height)
            {
                Map[x, y] = TileType.Door;
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
                {
                    Map[x, ny] = TileType.Floor;
                }
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
                {
                    Map[nx, y] = TileType.Floor;
                }
            }
        }
    }
}