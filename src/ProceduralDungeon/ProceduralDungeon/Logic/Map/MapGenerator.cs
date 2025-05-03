namespace ProceduralDungeon
{
    public class MapGenerator : IMapGenerator
    {
        public TileType[,] Map { get; }
        public List<Room> Rooms { get; } = new();

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

            RoomPlacement(roomCount, roomMinSize, roomMaxSize);

            BuildingGraphRoomsAndMST(out var extraEdges, out var mstEdges);

            BuildingCorridors(mstEdges, extraEdges);

            PlaceStartAndExit();

            Cleaning();

            EditingWalls();
        }

        private void EditingWalls()
        {
            var wallFixer = new WallFixer(Map, _mapWidth, _mapHeight);
            wallFixer.CleanDungeon();
        }

        private void Cleaning()
        {
            var cleaner = new MapCleaner(Map, _mapWidth, _mapHeight);
            cleaner.CleanUnreachableFloor(_startPoint);
        }

        private void BuildingCorridors(List<Edge> mstEdges, List<Edge> extraEdges)
        {
            var corridorBuilder = new CorridorBuilder(Map, _mapWidth, _mapHeight, _random);
            corridorBuilder.CreateCorridors(Rooms, mstEdges.Concat(extraEdges));
        }

        private void BuildingGraphRoomsAndMST(out List<Edge> extraEdges, out List<Edge> mstEdges)
        {
            var graphBuilder = new CorridorGraphBuilder(_random);
            var edges = graphBuilder.BuildGraph(Rooms);
            mstEdges = graphBuilder.KruskalMST(Rooms.Count, edges);
            extraEdges = graphBuilder.SelectExtraEdges(edges, mstEdges);
        }

        private void RoomPlacement(int roomCount, int roomMinSize, int roomMaxSize)
        {
            var placer = new RoomPlacerPoissonDisc(_mapWidth, _mapHeight, _random);
            var candidateRooms = placer.GenerateRooms(roomCount, roomMinSize, roomMaxSize);
            if (candidateRooms.Count < roomCount)
            {
                var greedyPlacer = new RoomPlacerGreedy(_mapWidth, _mapHeight, _random);
                candidateRooms = greedyPlacer.GenerateRooms(roomCount, roomMinSize, roomMaxSize);
            }

            var fallbackRoomCount = roomCount;
            var minRoomCount = Math.Max(3, roomCount / 2);
            while (candidateRooms.Count < fallbackRoomCount && fallbackRoomCount > minRoomCount)
            {
                fallbackRoomCount--;
                var placer2 = new RoomPlacerPoissonDisc(_mapWidth, _mapHeight, _random);
                candidateRooms = placer2.GenerateRooms(fallbackRoomCount, roomMinSize, roomMaxSize);
                if (candidateRooms.Count < fallbackRoomCount)
                {
                    var greedyPlacer2 = new RoomPlacerGreedy(_mapWidth, _mapHeight, _random);
                    candidateRooms = greedyPlacer2.GenerateRooms(fallbackRoomCount, roomMinSize, roomMaxSize);
                }
            }

            if (candidateRooms.Count < minRoomCount)
                throw new InvalidOperationException(
                    $"Не удалось разместить даже fallback-комнаты: {candidateRooms.Count} из {roomCount}. Попробуйте уменьшить roomCount или размеры комнат.");

            foreach (var room in candidateRooms)
                CreateRoom(room);
            Rooms.AddRange(candidateRooms);
        }

        private void CreateRoom(Room room)
        {
            for (var x = room.X; x < room.X + room.Width; x++)
            for (var y = room.Y; y < room.Y + room.Height; y++)
                Map[x, y] = TileType.Floor;
        }

        private void PlaceStartAndExit()
        {
            if (Rooms.Count <= 0)
                throw new InvalidOperationException("Комнаты не сгенерированы.");

            Room? startRoom = null;
            Room? exitRoom = null;
            float maxDistance = 0;
            foreach (var roomA in Rooms)
            foreach (var roomB in Rooms)
            {
                if (roomA == roomB) continue;
                var dx = roomA.CenterX - roomB.CenterX;
                var dy = roomA.CenterY - roomB.CenterY;
                var dist = dx * dx + dy * dy;
                if (dist > maxDistance)
                {
                    maxDistance = dist;
                    startRoom = roomA;
                    exitRoom = roomB;
                }
            }

            if (startRoom == null || exitRoom == null)
                throw new InvalidOperationException("Не удалось выбрать стартовую и выходную комнату.");
            _startPoint = new Point(startRoom.CenterX, startRoom.CenterY);
            _exitPoint = new Point(exitRoom.CenterX, exitRoom.CenterY);
            Map[_startPoint.X, _startPoint.Y] = TileType.Start;
            Map[_exitPoint.X, _exitPoint.Y] = TileType.Exit;
        }
    }
}