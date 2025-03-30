using System.Drawing;
using System.Drawing.Imaging;

namespace ProceduralDungeon
{
    public class CompositeImageDungeonRenderer : ImageDungeonRenderer
    {
        private TileType[,] _map;
        private DecorType[,] _decor;
        private EnemyType[,] _enemies;
        private int _tileSize;

        public override void RenderDungeon(DungeonGenerator generator, int cellSize)
        {
            _map = generator.MapLayer;
            _decor = generator.DecorLayer;
            _enemies = generator.EnemyLayer;
            _tileSize = cellSize;

            var outputPath = "dungeon.png";

            RenderCompositeImage(outputPath);

            OpenImage(outputPath);
        }

        private void RenderCompositeImage(string outputPath)
        {
            var width = _map.GetLength(0);
            var height = _map.GetLength(1);

            using var bitmap = new Bitmap(width * _tileSize, height * _tileSize);
            using var graphics = Graphics.FromImage(bitmap);

            graphics.Clear(Color.Black);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    DrawTile(graphics, x, y);
                }
            }

            bitmap.Save(outputPath, ImageFormat.Png);
        }

        private void DrawTile(Graphics graphics, int x, int y)
        {
            var baseColor = GetMapColor(_map[x, y]);
            var decor = _decor[x, y];
            var enemy = _enemies[x, y];

            DrawRectangle(graphics, x, y, baseColor);

            if (decor != DecorType.None)
            {
                var decorColor = Color.FromArgb(128, GetDecorColor(decor));
                DrawCircle(graphics, x, y, decorColor);
            }

            if (enemy != EnemyType.None)
            {
                var enemyColor = GetEnemyColor(enemy);
                DrawTriangle(graphics, x, y, enemyColor);
            }
        }

        private void DrawRectangle(Graphics g, int x, int y, Color color)
        {
            var rect = new Rectangle(
                x * _tileSize,
                y * _tileSize,
                _tileSize,
                _tileSize
            );

            using var brush = new SolidBrush(color);
            g.FillRectangle(brush, rect);
        }

        private void DrawCircle(Graphics g, int x, int y, Color color)
        {
            var rect = new Rectangle(
                x * _tileSize + _tileSize / 4,
                y * _tileSize + _tileSize / 4,
                _tileSize / 2,
                _tileSize / 2
            );

            using var brush = new SolidBrush(color);
            g.FillEllipse(brush, rect);
        }

        private void DrawTriangle(Graphics g, int x, int y, Color color)
        {
            var points = new[]
            {
                new Point(x * _tileSize + _tileSize / 2, y * _tileSize),
                new Point(x * _tileSize + _tileSize, y * _tileSize + _tileSize),
                new Point(x * _tileSize, y * _tileSize + _tileSize)
            };

            using var brush = new SolidBrush(color);
            g.FillPolygon(brush, points);
        }
    }
}