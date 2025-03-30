namespace ProceduralDungeon
{
    public class DecorGenerator : IDecorGenerator
    {
        public DecorType[,] DecorLayer { get; }

        private readonly Random _random = new();

        public DecorGenerator(int width, int height)
        {
            DecorLayer = new DecorType[width, height];
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
            {
                DecorLayer[x, y] = DecorType.None;
            }
        }

        public void GenerateDecor(TileType[,] map, List<Room> rooms)
        {
            foreach (var room in rooms)
            {
                int decorAttempts;
                switch (room.Type)
                {
                    case RoomType.Treatment:
                        decorAttempts = _random.Next(3, 6); // Больше сундуков
                        break;
                    case RoomType.Trap:
                        decorAttempts = _random.Next(2, 4); // Больше ловушек
                        break;
                    case RoomType.Hard:
                        decorAttempts = _random.Next(0, 2); // Минимум декораций
                        break;
                    default:
                        decorAttempts = _random.Next(0, 3); // Обычное количество
                        break;
                }

                for (var i = 0; i < decorAttempts; i++)
                {
                    var x = _random.Next(room.X + 1, room.X + room.Width - 1);
                    var y = _random.Next(room.Y + 1, room.Y + room.Height - 1);

                    if (DecorLayer[x, y] != DecorType.None) continue; // Не перезаписываем

                    DecorLayer[x, y] = room.Type switch
                    {
                        RoomType.Treatment => DecorType.Chest,
                        RoomType.Trap => DecorType.PressurePlate,
                        _ => _random.NextDouble() switch
                        {
                            < 0.3 => DecorType.Chest,
                            < 0.6 => DecorType.Column,
                            < 0.8 => DecorType.PressurePlate,
                            _ => DecorType.Barrel
                        }
                    };
                }
            }
        }
    }
}