using System.Buffers;

namespace ProceduralDungeon
{
    public class RoomPlacerGreedy
    {
        private readonly int _mapWidth;
        private readonly int _mapHeight;
        private readonly Random _random;

        public RoomPlacerGreedy(int mapWidth, int mapHeight, Random random)
        {
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
            _random = random;
        }

        public List<Room> GenerateRooms(int roomCount, int roomMinSize, int roomMaxSize)
        {
            var rooms = new List<Room>();
            var attempts = 0;
            var maxAttempts = roomCount * 20;
            while (rooms.Count < roomCount && attempts < maxAttempts)
            {
                var width = _random.Next(roomMinSize, roomMaxSize + 1);
                var height = _random.Next(roomMinSize, roomMaxSize + 1);
                var x = _random.Next(1, _mapWidth - width - 1);
                var y = _random.Next(1, _mapHeight - height - 1);
                var room = new Room(x, y, width, height, RoomType.Normal);
                var intersects = rooms.Any(other => room.Intersects(other));
                if (!intersects)
                {
                    rooms.Add(room);
                }

                attempts++;
            }

            var result = new List<Room>(rooms);
            return result;
        }
    }
}