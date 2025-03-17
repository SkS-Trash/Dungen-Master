using System;
using System.Collections.Generic;

namespace ProceduralDungeon
{
    public enum TileType
    {
        Empty,
        Wall,
        Floor,
        Door
    }

    public enum EnemyType
    {
        None,
        EnemyIsCloseCombat,
        EnemyRangedCombat
    }

    public enum DecorType
    {
        None,
        Chest,
        Barrel,
        PressurePlate, // ловушка с нажимной плитой
        Column
    }

    public class Room
    {
        public int X, Y, Width, Height;
        public int CenterX => X + Width / 2;
        public int CenterY => Y + Height / 2;

        public Room(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public bool Intersects(Room other)
        {
            return (X <= other.X + other.Width && X + Width >= other.X &&
                    Y <= other.Y + other.Height && Y + Height >= other.Y);
        }
    }

    // ----- Генератор карты (с "толстыми" коридорами) -----
    public class MapGenerator
    {
        public int MapWidth;
        public int MapHeight;
        public TileType[,] Map;
        public List<Room> Rooms;
        private Random random = new Random();

        public MapGenerator(int width, int height)
        {
            MapWidth = width;
            MapHeight = height;
            Map = new TileType[MapWidth, MapHeight];
            Rooms = new List<Room>();

            // Изначально заполняем карту стенами
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    Map[x, y] = TileType.Wall;
                }
            }
        }

        public void GenerateMap(int roomCount, int roomMinSize, int roomMaxSize)
        {
            for (int i = 0; i < roomCount; i++)
            {
                int roomWidth = random.Next(roomMinSize, roomMaxSize + 1);
                int roomHeight = random.Next(roomMinSize, roomMaxSize + 1);
                int roomX = random.Next(1, MapWidth - roomWidth - 1);
                int roomY = random.Next(1, MapHeight - roomHeight - 1);

                Room newRoom = new Room(roomX, roomY, roomWidth, roomHeight);

                // Проверяем пересечения с уже созданными комнатами
                bool overlaps = false;
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
                        Room prevRoom = Rooms[Rooms.Count - 1];
                        CreateCorridor(prevRoom, newRoom);
                    }

                    Rooms.Add(newRoom);
                }
            }
        }

        private void CreateRoom(Room room)
        {
            for (int x = room.X; x < room.X + room.Width; x++)
            {
                for (int y = room.Y; y < room.Y + room.Height; y++)
                {
                    Map[x, y] = TileType.Floor;
                }
            }
        }

        // Соединяем центры двух комнат "толстым" коридором
        private void CreateCorridor(Room roomA, Room roomB)
        {
            int x1 = roomA.CenterX;
            int y1 = roomA.CenterY;
            int x2 = roomB.CenterX;
            int y2 = roomB.CenterY;

            // Случайный порядок прокладывания коридора
            if (random.Next(0, 2) == 0)
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
            int start = Math.Min(x1, x2);
            int end = Math.Max(x1, x2);

            for (int x = start; x <= end; x++)
            {
                // Делаем коридор шириной 3 клетки (y-1, y, y+1)
                for (int dy = -1; dy <= 1; dy++)
                {
                    int ny = y + dy;
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
            int start = Math.Min(y1, y2);
            int end = Math.Max(y1, y2);

            for (int y = start; y <= end; y++)
            {
                // Делаем коридор шириной 3 клетки (x-1, x, x+1)
                for (int dx = -1; dx <= 1; dx++)
                {
                    int nx = x + dx;
                    if (nx >= 0 && nx < MapWidth)
                    {
                        Map[nx, y] = TileType.Floor;
                    }
                }
            }
        }
    }

    // ----- Генератор декора (увеличенная вероятность) -----
    public class DecorGenerator
    {
        public DecorType[,] DecorLayer;
        private Random random = new Random();

        public DecorGenerator(int width, int height)
        {
            DecorLayer = new DecorType[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    DecorLayer[x, y] = DecorType.None;
                }
            }
        }

        public void GenerateDecor(TileType[,] map, List<Room> rooms)
        {
            foreach (var room in rooms)
            {
                // Чтобы декор встречался чаще, делаем несколько попыток на комнату
                // Например, 5 попыток разместить что-то
                int decorAttempts = 5;
                for (int i = 0; i < decorAttempts; i++)
                {
                    // Случайная точка внутри комнаты
                    int x = random.Next(room.X + 1, room.X + room.Width - 1);
                    int y = random.Next(room.Y + 1, room.Y + room.Height - 1);

                    // Выбираем случайный тип декора с большей вероятностью
                    double roll = random.NextDouble();
                    if (roll < 0.3)
                    {
                        DecorLayer[x, y] = DecorType.Chest;
                    }
                    else if (roll < 0.6)
                    {
                        DecorLayer[x, y] = DecorType.Column;
                    }
                    else if (roll < 0.8)
                    {
                        DecorLayer[x, y] = DecorType.PressurePlate;
                    }
                    else
                    {
                        DecorLayer[x, y] = DecorType.Barrel;
                    }
                }
            }
        }
    }

    // ----- Генератор врагов -----
    public class EnemySpawner
    {
        public EnemyType[,] EnemyLayer;
        private Random random = new Random();

        public EnemySpawner(int width, int height)
        {
            EnemyLayer = new EnemyType[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    EnemyLayer[x, y] = EnemyType.None;
                }
            }
        }

        public void SpawnEnemies(TileType[,] map, List<Room> rooms)
        {
            foreach (var room in rooms)
            {
                // Случайное количество врагов (0..2) в каждой комнате
                int enemyCount = random.Next(0, 3);
                for (int i = 0; i < enemyCount; i++)
                {
                    int x = random.Next(room.X + 1, room.X + room.Width - 1);
                    int y = random.Next(room.Y + 1, room.Y + room.Height - 1);

                    // 50% шанс ближнего боя, 50% – дальнего
                    EnemyType enemy = (random.NextDouble() < 0.5)
                        ? EnemyType.EnemyIsCloseCombat
                        : EnemyType.EnemyRangedCombat;

                    EnemyLayer[x, y] = enemy;
                }
            }
        }
    }

    // ----- Контроллер генерации подземелья -----
    public class DungeonGenerator
    {
        public TileType[,] MapLayer;
        public DecorType[,] DecorLayer;
        public EnemyType[,] EnemyLayer;
        public List<Room> Rooms;
        public int Width, Height;

        public DungeonGenerator(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void GenerateDungeon(int roomCount, int roomMinSize, int roomMaxSize)
        {
            // Генерация базовой карты
            MapGenerator mapGen = new MapGenerator(Width, Height);
            mapGen.GenerateMap(roomCount, roomMinSize, roomMaxSize);
            MapLayer = mapGen.Map;
            Rooms = mapGen.Rooms;

            // Генерация декора
            DecorGenerator decorGen = new DecorGenerator(Width, Height);
            decorGen.GenerateDecor(MapLayer, Rooms);
            DecorLayer = decorGen.DecorLayer;

            // Расстановка врагов
            EnemySpawner enemySpawner = new EnemySpawner(Width, Height);
            enemySpawner.SpawnEnemies(MapLayer, Rooms);
            EnemyLayer = enemySpawner.EnemyLayer;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Параметры генерации
            int mapWidth = 50;
            int mapHeight = 30;
            int roomCount = 8;   // Кол-во комнат
            int roomMinSize = 5;
            int roomMaxSize = 10;

            // Генерируем подземелье
            DungeonGenerator dungeonGen = new DungeonGenerator(mapWidth, mapHeight);
            dungeonGen.GenerateDungeon(roomCount, roomMinSize, roomMaxSize);

            // 1) Печать базовой карты (теперь коридоры шире)
            Console.WriteLine("=== БАЗОВАЯ КАРТА (TileType) ===");
            PrintMapLayer(dungeonGen.MapLayer);

            // 2) Печать слоя декора (с повышенной вероятностью)
            Console.WriteLine("\n=== СЛОЙ ДЕКОРА (DecorType) ===");
            PrintDecorLayer(dungeonGen.DecorLayer);

            // 3) Печать слоя врагов
            Console.WriteLine("\n=== СЛОЙ ВРАГОВ (EnemyType) ===");
            PrintEnemyLayer(dungeonGen.EnemyLayer);

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void PrintMapLayer(TileType[,] map)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    char c = ' ';
                    switch (map[x, y])
                    {
                        case TileType.Wall:  c = '#'; break;
                        case TileType.Floor: c = '.'; break;
                        case TileType.Door:  c = 'D'; break;
                        default:             c = ' '; break;
                    }
                    Console.Write(c);
                }
                Console.WriteLine();
            }
        }

        static void PrintDecorLayer(DecorType[,] decor)
        {
            int width = decor.GetLength(0);
            int height = decor.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    char c = ' ';
                    switch (decor[x, y])
                    {
                        case DecorType.None:          c = ' '; break;
                        case DecorType.Chest:         c = 'C'; break;
                        case DecorType.Barrel:        c = 'B'; break;
                        case DecorType.PressurePlate: c = 'P'; break;
                        case DecorType.Column:        c = 'O'; break;
                        default:                      c = '?'; break;
                    }
                    Console.Write(c);
                }
                Console.WriteLine();
            }
        }

        static void PrintEnemyLayer(EnemyType[,] enemies)
        {
            int width = enemies.GetLength(0);
            int height = enemies.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    char c = ' ';
                    switch (enemies[x, y])
                    {
                        case EnemyType.None:
                            c = ' ';
                            break;
                        case EnemyType.EnemyIsCloseCombat:
                            c = 'M'; // M – Melee
                            break;
                        case EnemyType.EnemyRangedCombat:
                            c = 'R'; // R – Ranged
                            break;
                        default:
                            c = '?';
                            break;
                    }
                    Console.Write(c);
                }
                Console.WriteLine();
            }
        }
    }
}
