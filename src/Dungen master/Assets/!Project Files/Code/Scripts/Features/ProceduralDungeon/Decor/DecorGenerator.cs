using System;
using System.Collections.Generic;
using System.Linq;
using ProceduralDungeon.Data.Configs.Decor;
using ProceduralDungeon.Data.Types;

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

        public DecorGenerator(DecorGeneratorConfig config, int width, int height, Random random)
        {
            DecorLayer = new DecorType[width, height];
            _random = random;
            _profileProvider = new DecorProfileProvider(config);
            _distanceChecker = new DecorDistanceChecker();
            _positionSelector = new DecorPositionSelector();
            _randomizer = new DecorRandomizer(_random, config);
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

            var validPositions = _positionSelector.FindValidDecorPositions(room, map, DecorLayer,
                MIN_DISTANCE_BETWEEN_OBJECTS, _distanceChecker, out var count);

            _positionSelector.SortPositionsByDensity(validPositions, count);

            PlaceDecorInRoom(validPositions, count, attempts, room.Type, specialObjects, map);
        }

        private void PlaceDecorInRoom((int x, int y, int density)[] positions, int count, float attempts,
            RoomType roomType, List<DecorType> specialObjects, TileType[,] map)
        {
            var placed = 0;
            var used = new HashSet<(int, int)>();
            for (var i = 0; i < count; i++)
            {
                var pos = positions[i];
                if (placed >= attempts) break;
                if (used.Contains((pos.x, pos.y))) continue;
                var decor = _randomizer.SelectDecorType(roomType, specialObjects);
                if (decor is DecorType.Chest or DecorType.Altar && _random.NextDouble() < 0.7)
                {
                    var clusterSize = _random.Next(2, 5);
                    var cluster = GenerateClusterPositions(pos.x, pos.y, map, DecorLayer, clusterSize);
                    var actuallyPlaced = 0;
                    foreach (var (cx, cy) in cluster)
                    {
                        if (map[cx, cy] != TileType.Floor ||
                            _distanceChecker.HasNearbyDecor(cx, cy, MIN_DISTANCE_BETWEEN_OBJECTS, DecorLayer) ||
                            used.Contains((cx, cy)))
                            continue;

                        PlaceDecorWithSize(cx, cy, decor, map);
                        used.Add((cx, cy));
                        actuallyPlaced++;
                        placed++;

                        if (placed >= attempts) break;
                    }

                    if (actuallyPlaced > 0) continue;
                }

                if (decor != DecorType.None &&
                    map[pos.x, pos.y] == TileType.Floor &&
                    !_distanceChecker.HasNearbyDecor(pos.x, pos.y, MIN_DISTANCE_BETWEEN_OBJECTS, DecorLayer))
                {
                    PlaceDecorWithSize(pos.x, pos.y, decor, map);
                    used.Add((pos.x, pos.y));
                    placed++;
                }
            }
        }

        private List<(int x, int y)> GenerateClusterPositions(int x, int y, TileType[,] map, DecorType[,] decorLayer,
            int clusterSize)
        {
            var cluster = new List<(int, int)> { (x, y) };
            var candidates = new List<(int, int)> { (x, y) };
            var visited = new HashSet<(int, int)> { (x, y) };
            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };
            while (cluster.Count < clusterSize && candidates.Count > 0)
            {
                var idx = _random.Next(candidates.Count);
                var (cx, cy) = candidates[idx];
                candidates.RemoveAt(idx);
                for (var dir = 0; dir < 4; dir++)
                {
                    var nx = cx + dx[dir];
                    var ny = cy + dy[dir];
                    if (nx < 0 || nx >= map.GetLength(0) ||
                        ny < 0 || ny >= map.GetLength(1) ||
                        map[nx, ny] != TileType.Floor ||
                        decorLayer[nx, ny] != DecorType.None ||
                        visited.Contains((nx, ny)))
                        continue;

                    cluster.Add((nx, ny));
                    candidates.Add((nx, ny));
                    visited.Add((nx, ny));

                    if (cluster.Count >= clusterSize) break;
                }
            }

            return cluster;
        }

        private float CalculateDecorAttempts(Room room, float baseDensity)
        {
            var roomArea = room.Width * room.Height;
            return (float)(roomArea * baseDensity * 0.1);
        }

        private void PlaceDecorWithSize(int x, int y, DecorType decor, TileType[,] map)
        {
            if (!CanPlaceDecor(x, y, (1, 1), map)) return;

            for (var dx = 0; dx < 1; dx++)
            for (var dy = 0; dy < 1; dy++)
                DecorLayer[x + dx, y + dy] = decor;
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
                    return (x, y);
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