using System;
using System.Collections.Generic;
using System.Linq;
using ProceduralDungeon.Data.Configs.Map;
using ProceduralDungeon.Data.Types;

namespace ProceduralDungeon
{
    public enum MapGenerationMode
    {
        Rectangular,
        Cavern,
        BSP,
    }

    public class MapGenerator : IMapGenerator
    {
        public TileType[,] Map { get; private set; }
        public List<Room> Rooms { get; } = new();

        private readonly int _mapWidth;
        private readonly int _mapHeight;
        private readonly Random _random;
        private readonly MapGenerationMode _generationMode;
        private readonly TileGeneratorConfig _generatorConfig;

        private Point _startPoint = new(0, 0);
        private Point _exitPoint = new(0, 0);

        public MapGenerator(TileGeneratorConfig generatorConfig, Random random)
        {
            _generatorConfig = generatorConfig;
            _mapWidth = generatorConfig.Width;
            _mapHeight = generatorConfig.Height;
            _random = random;
            _generationMode = generatorConfig.GenerationMode;
            Map = new TileType[_mapWidth, _mapHeight];
        }

        public void GenerateMap()
        {
            Rooms.Clear();
            switch (_generationMode)
            {
                case MapGenerationMode.Cavern:
                    MapGenerationModeCavern();
                    break;
                case MapGenerationMode.BSP:
                    MapGenerationModeBSP(_generatorConfig.RoomMinSize, _generatorConfig.RoomMaxSize);
                    break;
                case MapGenerationMode.Rectangular:
                    MapGenerationModeRectangular(_generatorConfig.RoomCount, _generatorConfig.RoomMinSize,
                        _generatorConfig.RoomMaxSize);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_generationMode),
                        $"Неизвестный режим генерации карты: {_generationMode}");
            }
        }

        private void MapGenerationModeCavern()
        {
            var cavernGen = new CavernGenerator(_mapWidth, _mapHeight, _random);
            Map = cavernGen.GenerateCavern();
            var floorPoints = new List<Point>();
            for (var x = 0; x < _mapWidth; x++)
            for (var y = 0; y < _mapHeight; y++)
                if (Map[x, y] == TileType.Floor)
                    floorPoints.Add(new Point(x, y));

            if (floorPoints.Count >= 2)
            {
                _startPoint = floorPoints[_random.Next(floorPoints.Count)];
                do
                {
                    _exitPoint = floorPoints[_random.Next(floorPoints.Count)];
                } while (_exitPoint.Equals(_startPoint));

                Map[_startPoint.X, _startPoint.Y] = TileType.Start;
                Map[_exitPoint.X, _exitPoint.Y] = TileType.Exit;
            }

            Rooms.Clear();
            var visited = new bool[_mapWidth, _mapHeight];
            int[] dx = { 0, 1, 0, -1 };
            int[] dy = { -1, 0, 1, 0 };
            for (var x = 0; x < _mapWidth; x++)
            for (var y = 0; y < _mapHeight; y++)
            {
                if (visited[x, y]) continue;
                if (Map[x, y] is not (TileType.Floor or TileType.Start or TileType.Exit)) continue;
                var queue = new Queue<Point>();
                var component = new List<Point>();
                queue.Enqueue(new Point(x, y));
                visited[x, y] = true;
                while (queue.Count > 0)
                {
                    var p = queue.Dequeue();
                    component.Add(p);
                    for (var dir = 0; dir < 4; dir++)
                    {
                        int nx = p.X + dx[dir], ny = p.Y + dy[dir];
                        if (nx < 0 || ny < 0 || nx >= _mapWidth || ny >= _mapHeight) continue;
                        if (visited[nx, ny]) continue;
                        if (Map[nx, ny] is not (TileType.Floor or TileType.Start or TileType.Exit)) continue;
                        visited[nx, ny] = true;
                        queue.Enqueue(new Point(nx, ny));
                    }
                }

                var minX = component.Min(p => p.X);
                var minY = component.Min(p => p.Y);
                var maxX = component.Max(p => p.X);
                var maxY = component.Max(p => p.Y);
                var width = maxX - minX + 1;
                var height = maxY - minY + 1;
                Rooms.Add(new Room(minX, minY, width, height, RoomType.Default));
            }

            Cleaning();
            EditingWalls();
        }

        private void MapGenerationModeBSP(int roomMinSize, int roomMaxSize)
        {
            var bspGen = new BspRoomGenerator(_mapWidth, _mapHeight, _random, roomMinSize, roomMaxSize);
            var bspRooms = bspGen.GenerateRooms();
            foreach (var room in bspRooms)
                CreateRoom(room);
            Rooms.AddRange(bspRooms);
            BuildingGraphRoomsAndMST(out var extraEdges, out var mstEdges);
            BuildingCorridors(mstEdges, extraEdges);
            PlaceStartAndExit();
            Cleaning();
            EditingWalls();
        }

        private void MapGenerationModeRectangular(int roomCount, int roomMinSize, int roomMaxSize)
        {
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
                    $"Не удалось разместить даже fallback-комнаты: {candidateRooms.Count} из {roomCount}. " +
                    "Попробуйте уменьшить roomCount или размеры комнат.");

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