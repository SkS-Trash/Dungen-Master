using System;
using System.Collections.Generic;
using ProceduralDungeon.Data.Types;

namespace ProceduralDungeon
{
    public class BspRoomGenerator
    {
        private readonly int _width;
        private readonly int _height;
        private readonly Random _random;
        private readonly int _minRoomSize;
        private readonly int _maxRoomSize;
        private readonly int _maxDepth;

        public BspRoomGenerator(int width, int height, Random random, int minRoomSize, int maxRoomSize,
            int maxDepth = 5)
        {
            _width = width;
            _height = height;
            _random = random;
            _minRoomSize = minRoomSize;
            _maxRoomSize = maxRoomSize;
            _maxDepth = maxDepth;
        }

        public List<Room> GenerateRooms()
        {
            var rooms = new List<Room>();
            SplitArea(1, 1, _width - 2, _height - 2, 0, rooms);
            return rooms;
        }

        private void SplitArea(int x, int y, int width, int height, int depth, List<Room> rooms)
        {
            if (depth >= _maxDepth || width < _minRoomSize * 2 || height < _minRoomSize * 2)
            {
                var roomWidth = _random.Next(_minRoomSize, Math.Min(width, _maxRoomSize) + 1);
                var roomHeight = _random.Next(_minRoomSize, Math.Min(height, _maxRoomSize) + 1);
                var roomX = x + _random.Next(0, Math.Max(1, width - roomWidth + 1));
                var roomY = y + _random.Next(0, Math.Max(1, height - roomHeight + 1));
                rooms.Add(new Room(roomX, roomY, roomWidth, roomHeight, RoomType.Default));
                return;
            }

            var splitHorizontally = width < height || (width <= height && _random.Next(2) == 0);
            if (splitHorizontally)
            {
                var split = _random.Next(_minRoomSize, height - _minRoomSize + 1);
                SplitArea(x, y, width, split, depth + 1, rooms);
                SplitArea(x, y + split, width, height - split, depth + 1, rooms);
            }
            else
            {
                var split = _random.Next(_minRoomSize, width - _minRoomSize + 1);
                SplitArea(x, y, split, height, depth + 1, rooms);
                SplitArea(x + split, y, width - split, height, depth + 1, rooms);
            }
        }
    }
}