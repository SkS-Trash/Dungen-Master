namespace ProceduralDungeon
{
    public class DecorGenerator : IDecorGenerator
    {
        public DecorType[,] DecorLayer { get; }
        private const int MIN_DISTANCE_BETWEEN_OBJECTS = 2;
        private readonly Random _random;
        private readonly DecorProfileProvider _profileProvider;
        private readonly DecorPositionSelector _positionSelector;
        private readonly DecorRandomizer _randomizer;
        private readonly DecorDistanceChecker _distanceChecker;

        public DecorGenerator(int width, int height, Random random)
        {
            DecorLayer = new DecorType[width, height];
            _random = random;
            _profileProvider = new DecorProfileProvider();
            _distanceChecker = new DecorDistanceChecker();
            _positionSelector = new DecorPositionSelector();
            _randomizer = new DecorRandomizer(random);
        }

        public void GenerateDecor(TileType[,] map, List<Room> rooms)
        {
            foreach (var room in rooms)
                GenerateRoomDecor(map, room);

            GenerateCorridorDecor(map, rooms);
        }

        private void GenerateRoomDecor(TileType[,] map, Room room)
        {
            var (baseDensity, specialObjects) = _profileProvider.GetRoomDecorProfile(room.Type);
            var attempts = CalculateDecorAttempts(room, baseDensity);

            var validPositions = _positionSelector.FindValidDecorPositions(room, map, DecorLayer, MIN_DISTANCE_BETWEEN_OBJECTS, _distanceChecker);
            _positionSelector.SortPositionsByDensity(validPositions);
            PlaceDecorInRoom(validPositions, attempts, room.Type, specialObjects, map);
        }

        private void PlaceDecorInRoom(List<(int x, int y, int density)> positions, int attempts, RoomType roomType,
            List<DecorType> specialObjects, TileType[,] map)
        {
            var placed = 0;
            foreach (var pos in positions)
            {
                if (placed >= attempts) break;
                var decor = _randomizer.SelectDecorType(roomType, specialObjects);
                if (decor != DecorType.None && map[pos.x, pos.y] == TileType.Floor &&
                    !_distanceChecker.HasNearbyDecor(pos.x, pos.y, MIN_DISTANCE_BETWEEN_OBJECTS, DecorLayer))
                {
                    PlaceDecorWithSize(pos.x, pos.y, decor, map);
                    placed++;
                }
            }
        }

        private int CalculateDecorAttempts(Room room, int baseDensity)
        {
            var roomArea = room.Width * room.Height;
            return (int)(roomArea * baseDensity * 0.1);
        }

        private void PlaceDecorWithSize(int x, int y, DecorType decor, TileType[,] map)
        {
            if (!CanPlaceDecor(x, y, (1, 1), map)) return;

            for (var dx = 0; dx < 1; dx++)
            for (var dy = 0; dy < 1; dy++)
            {
                DecorLayer[x + dx, y + dy] = decor;
            }
        }

        private bool CanPlaceDecor(int x, int y, (int W, int H) size, TileType[,] map)
        {
            for (var dx = 0; dx < size.W; dx++)
            for (var dy = 0; dy < size.H; dy++)
            {
                if (x + dx >= map.GetLength(0) ||
                    y + dy >= map.GetLength(1) ||
                    map[x + dx, y + dy] != TileType.Floor ||
                    DecorLayer[x + dx, y + dy] != DecorType.None)
                    return false;
            }

            return true;
        }

        private void GenerateCorridorDecor(TileType[,] map, List<Room> rooms)
        {
            foreach (var room in rooms.Where(room => _random.NextDouble() < 0.25))
            {
                var (x, y) = FindValidPosition(room, map, 0);
                if (x > 0 && y > 0)
                {
                    DecorLayer[x, y] = DecorType.Barrel;
                }
            }
        }

        private (int x, int y) FindValidPosition(Room room, TileType[,] map, int border)
        {
            for (var attempt = 0; attempt < 50; attempt++)
            {
                var x = _random.Next(room.X + border, room.X + room.Width - border);
                var y = _random.Next(room.Y + border, room.Y + room.Height - border);

                if (IsPositionValid(x, y, map) &&
                    !HasNearbyDecor(x, y, MIN_DISTANCE_BETWEEN_OBJECTS))
                {
                    return (x, y);
                }
            }

            return (-1, -1);
        }

        private bool HasNearbyDecor(int x, int y, int radius)
        {
            for (var dx = -radius; dx <= radius; dx++)
            for (var dy = -radius; dy <= radius; dy++)
            {
                var nx = x + dx;
                var ny = y + dy;
                if (nx >= 0 && nx < DecorLayer.GetLength(0) &&
                    ny >= 0 && ny < DecorLayer.GetLength(1) &&
                    DecorLayer[nx, ny] != DecorType.None)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsPositionValid(int x, int y, TileType[,] map)
        {
            return map[x, y] == TileType.Floor;
        }
    }
}