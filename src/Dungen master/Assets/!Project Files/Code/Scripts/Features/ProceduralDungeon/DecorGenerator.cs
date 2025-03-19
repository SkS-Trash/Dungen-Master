using System;
using System.Collections.Generic;
using Dungeon;

namespace Features.ProceduralDungeon
{
    public class DecorGenerator
    {
        public readonly DecorType[,] DecorLayer;

        private readonly Random _random = new();

        public DecorGenerator(int width, int height)
        {
            DecorLayer = new DecorType[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
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
                var decorAttempts = 5;
                for (var i = 0; i < decorAttempts; i++)
                {
                    // Случайная точка внутри комнаты
                    var x = _random.Next(room.X + 1, room.X + room.Width - 1);
                    var y = _random.Next(room.Y + 1, room.Y + room.Height - 1);

                    // Выбираем случайный тип декора с большей вероятностью
                    var roll = _random.NextDouble();
                    
                    DecorLayer[x, y] = roll switch
                    {
                        < 0.3 => DecorType.Chest,
                        < 0.6 => DecorType.Column,
                        < 0.8 => DecorType.PressurePlate,
                        _ => DecorType.Barrel
                    };
                }
            }
        }
    }
}