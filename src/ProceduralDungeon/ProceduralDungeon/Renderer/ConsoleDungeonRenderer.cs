using System.Text;

namespace ProceduralDungeon
{
    public class ConsoleDungeonRenderer : IDungeonRenderer
    {
        public void RenderDungeon(DungeonGenerator generator, int cellSize)
        {
            Console.WriteLine("=== БАЗОВАЯ КАРТА (TileType) ===");
            PrintLayer(generator.MapLayer, TileToChar);

            Console.WriteLine("\n=== СЛОЙ ДЕКОРА (DecorType) ===");
            PrintLayer(generator.DecorLayer, DecorToChar);

            Console.WriteLine("\n=== СЛОЙ ВРАГОВ (EnemyType) ===");
            PrintLayer(generator.EnemyLayer, EnemyToChar);
        }

        private static void PrintLayer<T>(T[,] layer, Func<T, char> converter)
        {
            var width = layer.GetLength(0);
            var height = layer.GetLength(1);

            var sb = new StringBuilder();
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    sb.Append(converter(layer[x, y]));
                }

                sb.AppendLine();
            }

            Console.Write(sb);
        }

        private static char TileToChar(TileType tile) => tile switch
        {
            TileType.Wall => '#',
            TileType.Floor => '.',
            _ => ' '
        };

        private static char DecorToChar(DecorType decor) => decor switch
        {
            DecorType.Chest => 'C',
            DecorType.Barrel => 'B',
            DecorType.PressurePlate => 'P',
            DecorType.Column => 'O',
            DecorType.Altar => 'A',
            DecorType.Statue => 'S',
            DecorType.BrokenWall => 'W',
            _ => ' '
        };

        private static char EnemyToChar(EnemyType enemy) => enemy switch
        {
            EnemyType.EnemyIsCloseCombat => 'M',
            EnemyType.EnemyRangedCombat => 'R',
            EnemyType.Boss => 'B',
            EnemyType.FlyingEnemy => 'F',
            EnemyType.Mimic => 'C',
            _ => ' '
        };
    }
}