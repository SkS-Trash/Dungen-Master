using System;
using System.Collections.Generic;

namespace ProceduralDungeon.Decor
{
    public class DecorGenerator : IDecorGenerator
    {
        public DecorType[,] DecorLayer { get; }

        private const int MIN_DISTANCE_BETWEEN_OBJECTS = 2;

        private readonly Random _random;

        public DecorGenerator(int width, int height, int seed)
        {
            DecorLayer = new DecorType[width, height];

            _random = new Random(seed);
        }

        public void GenerateDecor(TileType[,] map, List<Room> rooms)
        {
            foreach (var room in rooms)
            {
                GenerateRoomDecor(map, room);
                // GenerateWallDecor(map, room);
            }

            GenerateCorridorDecor(map, rooms);
        }

        private void GenerateRoomDecor(TileType[,] map, Room room)
        {
            var (baseDensity, specialObjects) = GetRoomDecorProfile(room.Type);
            var attempts = CalculateDecorAttempts(room, baseDensity);

            for (var i = 0; i < attempts; i++)
            {
                var (x, y) = FindValidPosition(room, map, 1);
                if (x < 0 || y < 0) continue;

                var decor = SelectDecorType(room.Type, specialObjects);
                if (decor != DecorType.None && IsPositionValid(x, y, map))
                {
                    PlaceDecorWithSize(x, y, decor, map);
                }
            }
        }

        private int CalculateDecorAttempts(Room room, int baseDensity)
        {
            var roomArea = room.Width * room.Height;
            return (int)(roomArea * baseDensity * 0.1);
        }

        private (int baseDensity, List<DecorType> specialObjects) GetRoomDecorProfile(RoomType type)
        {
            return type switch
            {
                RoomType.Treatment => (4, new List<DecorType> { DecorType.Altar }),
                RoomType.Trap => (3, new List<DecorType> { DecorType.Spikes, DecorType.PressurePlate }),
                RoomType.Hard => (2, new List<DecorType> { DecorType.Campfire }),
                _ => (2, new List<DecorType> { DecorType.Barrel, DecorType.Column })
            };
        }

        private DecorType SelectDecorType(RoomType roomType, List<DecorType> specialObjects)
        {
            if (_random.NextDouble() < 0.3 && specialObjects.Count > 0)
            {
                return specialObjects[_random.Next(specialObjects.Count)];
            }

            return GetWeightedDecor(roomType);
        }

        private DecorType GetWeightedDecor(RoomType roomType)
        {
            var weights = new Dictionary<DecorType, int>
            {
                [DecorType.None] = 0,
                [DecorType.Chest] = roomType == RoomType.Trap ? 5 : 15,
                [DecorType.Barrel] = 30,
                [DecorType.Column] = 20,
                [DecorType.Campfire] = 10
            };

            return WeightedRandomizer.GetRandom(weights);
        }

        private void PlaceDecorWithSize(int x, int y, DecorType decor, TileType[,] map)
        {
            if (CanPlaceDecor(x, y, (1, 1), map))
            {
                for (var dx = 0; dx < 1; dx++)
                for (var dy = 0; dy < 1; dy++)
                {
                    DecorLayer[x + dx, y + dy] = decor;
                }
            }
        }

        private bool CanPlaceDecor(int x, int y, (int W, int H) size, TileType[,] map)
        {
            for (var dx = 0; dx < size.W; dx++)
            {
                for (var dy = 0; dy < size.H; dy++)
                {
                    if (x + dx >= map.GetLength(0) ||
                        y + dy >= map.GetLength(1) ||
                        map[x + dx, y + dy] != TileType.Floor ||
                        DecorLayer[x + dx, y + dy] != DecorType.None)
                        return false;
                }
            }

            return true;
        }

        private void GenerateCorridorDecor(TileType[,] map, List<Room> rooms)
        {
            foreach (var room in rooms)
            {
                if (_random.NextDouble() < 0.25)
                {
                    var (x, y) = FindValidPosition(room, map, 0);
                    if (x > 0 && y > 0)
                    {
                        DecorLayer[x, y] = DecorType.Barrel;
                    }
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
            {
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
            }

            return false;
        }

        private bool IsPositionValid(int x, int y, TileType[,] map)
        {
            return map[x, y] == TileType.Floor;
        }
    }
}