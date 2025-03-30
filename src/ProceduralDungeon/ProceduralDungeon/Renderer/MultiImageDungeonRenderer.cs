using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace ProceduralDungeon
{
    public abstract class ImageDungeonRenderer : IDungeonRenderer
    {
        public abstract void RenderDungeon(DungeonGenerator generator, int cellSize);

        protected static Color GetMapColor(TileType tile)
        {
            return tile switch
            {
                TileType.Wall => Color.Gray,
                TileType.Floor => Color.LightGray,
                TileType.Empty => Color.Black,
                TileType.Start => Color.Red,
                TileType.Exit => Color.Green,
                _ => Color.Magenta
            };
        }

        protected static Color GetDecorColor(DecorType decor)
        {
            return decor switch
            {
                DecorType.None => Color.Black,
                DecorType.Chest => Color.Gold,
                DecorType.Barrel => Color.SaddleBrown,
                DecorType.PressurePlate => Color.DarkGray,
                DecorType.Column => Color.DimGray,
                DecorType.Altar => Color.DarkRed,
                DecorType.Torch => Color.Orange,
                DecorType.Bones => Color.White,
                DecorType.BookShelf => Color.SandyBrown,
                DecorType.Campfire => Color.OrangeRed,
                DecorType.Spikes => Color.DarkRed,
                DecorType.MedicalTable => Color.White,
                _ => Color.Magenta
            };
        }

        protected static Color GetEnemyColor(EnemyType enemy)
        {
            return enemy switch
            {
                EnemyType.None => Color.Black,
                EnemyType.EnemyIsCloseCombat => Color.Red,
                EnemyType.EnemyRangedCombat => Color.Orange,
                EnemyType.Boss => Color.Purple,
                EnemyType.FlyingEnemy => Color.Blue,
                EnemyType.Mimic => Color.HotPink,
                _ => Color.Magenta
            };
        }

        protected static void OpenImage(string filePath) => new Process
        {
            StartInfo = new ProcessStartInfo(filePath)
            {
                UseShellExecute = true
            }
        }.Start();
    }

    public class MultiImageDungeonRenderer : ImageDungeonRenderer
    {
        public override void RenderDungeon(DungeonGenerator generator, int cellSize)
        {
            RenderLayerToImage(generator.MapLayer, "MapLayer.png", cellSize, GetMapColor);
            RenderLayerToImage(generator.DecorLayer, "DecorLayer.png", cellSize, GetDecorColor);
            RenderLayerToImage(generator.EnemyLayer, "EnemyLayer.png", cellSize, GetEnemyColor);

            OpenImage("MapLayer.png");
            OpenImage("DecorLayer.png");
            OpenImage("EnemyLayer.png");
        }

        private static void RenderLayerToImage<T>(T[,] layer, string filePath, int tileSize, Func<T, Color> getColor)
        {
            var width = layer.GetLength(0);
            var height = layer.GetLength(1);

            using var bitmap = new Bitmap(width * tileSize, height * tileSize);
            using var graphics = Graphics.FromImage(bitmap);

            graphics.Clear(Color.Black);

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var color = getColor(layer[x, y]);
                    using var brush = new SolidBrush(color);
                    graphics.FillRectangle(brush,
                        x * tileSize,
                        y * tileSize,
                        tileSize,
                        tileSize);
                }
            }

            bitmap.Save(filePath, ImageFormat.Png);
        }
    }
}