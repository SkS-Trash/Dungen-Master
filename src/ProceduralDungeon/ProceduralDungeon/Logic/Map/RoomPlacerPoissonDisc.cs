using System.Buffers;

namespace ProceduralDungeon
{
    public class RoomPlacerPoissonDisc
    {
        private readonly int _mapWidth;
        private readonly int _mapHeight;
        private readonly Random _random;

        public RoomPlacerPoissonDisc(int mapWidth, int mapHeight, Random random)
        {
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
            _random = random;
        }

        public List<Room> GenerateRooms(int roomCount, int roomMinSize, int roomMaxSize, int k = 30)
        {
            var rooms = ArrayPool<List<Room>>.Shared.Rent(1)[0] ?? new List<Room>();
            rooms.Clear();
            var candidates = ArrayPool<List<(int x, int y)>>.Shared.Rent(1)[0] ?? new List<(int x, int y)>();
            candidates.Clear();
            var minDist = roomMinSize + 2;

            var (startX, startY) = GetRandomStartPoint(roomMinSize);
            candidates.Add((startX, startY));

            var attempts = 0;
            while (rooms.Count < roomCount && candidates.Count > 0 && attempts < roomCount * k)
            {
                var idx = _random.Next(candidates.Count);
                var (cx, cy) = candidates[idx];
                candidates.RemoveAt(idx);

                var room = TryCreateRoom(cx, cy, roomMinSize, roomMaxSize);
                if (room == null || HasIntersection(room, rooms))
                {
                    attempts++;
                    continue;
                }

                rooms.Add(room);
                attempts = 0;

                AddNewCandidates(cx, cy, minDist, k, roomMinSize, candidates);
            }

            var result = new List<Room>(rooms);
            rooms.Clear();
            ArrayPool<List<Room>>.Shared.Return(new List<Room>[] { rooms });
            candidates.Clear();
            ArrayPool<List<(int x, int y)>>.Shared.Return(new List<(int x, int y)>[] { candidates });
            return result;
        }

        private (int x, int y) GetRandomStartPoint(int roomMinSize)
        {
            var x = _random.Next(roomMinSize, _mapWidth - roomMinSize);
            var y = _random.Next(roomMinSize, _mapHeight - roomMinSize);
            return (x, y);
        }

        private Room? TryCreateRoom(int cx, int cy, int roomMinSize, int roomMaxSize)
        {
            var width = _random.Next(roomMinSize, roomMaxSize + 1);
            var height = _random.Next(roomMinSize, roomMaxSize + 1);
            var x = Math.Clamp(cx - width / 2, 1, _mapWidth - width - 1);
            var y = Math.Clamp(cy - height / 2, 1, _mapHeight - height - 1);
            return new Room(x, y, width, height, RoomType.Normal);
        }

        private bool HasIntersection(Room room, List<Room> rooms)
        {
            return rooms.Any(other => room.Intersects(other));
        }

        private void AddNewCandidates(int cx, int cy, int minDist, int k, int roomMinSize,
            List<(int x, int y)> candidates)
        {
            for (var i = 0; i < k; i++)
            {
                var angle = 2 * Math.PI * _random.NextDouble();
                var dist = minDist + _random.NextDouble() * minDist;
                var nx = (int)(cx + Math.Cos(angle) * dist);
                var ny = (int)(cy + Math.Sin(angle) * dist);
                if (nx > roomMinSize && nx < _mapWidth - roomMinSize && ny > roomMinSize &&
                    ny < _mapHeight - roomMinSize)
                    candidates.Add((nx, ny));
            }
        }
    }
}