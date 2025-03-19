using System;
using System.Collections.Generic;
using Dungeon;

namespace Features.ProceduralDungeon
{
    public class MapGenerator
    {
        public readonly int MapWidth;
        public readonly int MapHeight;
        public readonly TileType[,] Map;
        public readonly List<Room> Rooms;
        private readonly Random _random = new();

        public MapGenerator(int width, int height)
        {
            MapWidth = width;
            MapHeight = height;
            Map = new TileType[MapWidth, MapHeight];
            Rooms = new List<Room>();

            // Изначально заполняем карту стенами
            for (var x = 0; x < MapWidth; x++)
            {
                for (var y = 0; y < MapHeight; y++)
                {
                    Map[x, y] = TileType.Wall;
                }
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

                var newRoom = new Room(roomX, roomY, roomWidth, roomHeight);

                // Проверяем пересечения с уже созданными комнатами
                var overlaps = false;
                foreach (var otherRoom in Rooms)
                {
                    if (newRoom.Intersects(otherRoom))
                    {
                        overlaps = true;
                        break;
                    }
                }

                // Если не пересекается, "вырезаем" комнату
                if (!overlaps)
                {
                    CreateRoom(newRoom);

                    // Соединяем с предыдущей комнатой коридором
                    if (Rooms.Count > 0)
                    {
                        var prevRoom = Rooms[Rooms.Count - 1];
                        CreateCorridor(prevRoom, newRoom);
                    }

                    Rooms.Add(newRoom);
                }
            }
        }

        private void CreateRoom(Room room)
        {
            for (var x = room.X; x < room.X + room.Width; x++)
            {
                for (var y = room.Y; y < room.Y + room.Height; y++)
                {
                    Map[x, y] = TileType.Floor;
                }
            }
        }

        // Соединяем центры двух комнат "толстым" коридором
        private void CreateCorridor(Room roomA, Room roomB)
        {
            var x1 = roomA.CenterX;
            var y1 = roomA.CenterY;
            var x2 = roomB.CenterX;
            var y2 = roomB.CenterY;

            // Случайный порядок прокладывания коридора
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

        // Горизонтальный "толстый" коридор
        private void CreateHorizontalCorridor(int x1, int x2, int y)
        {
            var start = Math.Min(x1, x2);
            var end = Math.Max(x1, x2);

            for (var x = start; x <= end; x++)
            {
                // Делаем коридор шириной 3 клетки (y-1, y, y+1)
                for (var dy = -1; dy <= 1; dy++)
                {
                    var ny = y + dy;
                    if (ny >= 0 && ny < MapHeight)
                    {
                        Map[x, ny] = TileType.Floor;
                    }
                }
            }
        }

        // Вертикальный "толстый" коридор
        private void CreateVerticalCorridor(int y1, int y2, int x)
        {
            var start = Math.Min(y1, y2);
            var end = Math.Max(y1, y2);

            for (var y = start; y <= end; y++)
            {
                // Делаем коридор шириной 3 клетки (x-1, x, x+1)
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
}