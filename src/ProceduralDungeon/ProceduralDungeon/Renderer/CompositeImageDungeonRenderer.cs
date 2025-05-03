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
            var legendPath = "legend.png";

            RenderCompositeImage(outputPath);
            OpenImage(outputPath);

            GenerateLegend(legendPath);
            OpenImage(legendPath);
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
            var baseColor = GetBrightMapColor(_map[x, y]);
            var decor = _decor[x, y];
            var enemy = _enemies[x, y];

            DrawRectangle(graphics, x, y, baseColor);

            if (decor != DecorType.None)
            {
                DrawDecor(graphics, x, y, decor);
            }

            if (enemy != EnemyType.None)
            {
                DrawEnemy(graphics, x, y, enemy);
            }
        }

        private static Color GetBrightMapColor(TileType tile)
        {
            return tile switch
            {
                TileType.Wall => Color.FromArgb(60, 60, 60),
                TileType.Floor => Color.FromArgb(220, 220, 220),
                TileType.Empty => Color.Black,
                TileType.Start => Color.FromArgb(255, 0, 0),
                TileType.Exit => Color.FromArgb(0, 255, 0),
                _ => Color.Magenta
            };
        }

        private static Color GetBrightDecorColor(DecorType decor)
        {
            return decor switch
            {
                DecorType.None => Color.Transparent,
                DecorType.Chest => Color.FromArgb(255, 215, 0),
                DecorType.Barrel => Color.FromArgb(139, 69, 19),
                DecorType.PressurePlate => Color.FromArgb(128, 128, 128),
                DecorType.Column => Color.FromArgb(0, 191, 255),
                DecorType.Altar => Color.FromArgb(255, 0, 255),
                DecorType.Campfire => Color.FromArgb(255, 140, 0),
                DecorType.Spikes => Color.FromArgb(255, 0, 0),
                _ => Color.Magenta
            };
        }

        private static Color GetBrightEnemyColor(EnemyType enemy)
        {
            return enemy switch
            {
                EnemyType.None => Color.Transparent,
                EnemyType.EnemyIsCloseCombat => Color.FromArgb(255, 69, 0),
                EnemyType.EnemyRangedCombat => Color.FromArgb(0, 0, 255),
                EnemyType.Boss => Color.FromArgb(128, 0, 128),
                EnemyType.FlyingEnemy => Color.FromArgb(0, 255, 255),
                EnemyType.Mimic => Color.FromArgb(255, 20, 147),
                _ => Color.Magenta
            };
        }

        private void DrawDecor(Graphics g, int x, int y, DecorType decor)
        {
            var color = Color.FromArgb(180, GetBrightDecorColor(decor));
            switch (decor)
            {
                case DecorType.Chest:
                    DrawDiamond(g, x, y, color);
                    break;
                case DecorType.Barrel:
                    DrawCircle(g, x, y, color);
                    break;
                case DecorType.PressurePlate:
                    DrawRectangle(g, x, y, color, 0.5f);
                    break;
                case DecorType.Column:
                    DrawVerticalRect(g, x, y, color);
                    break;
                case DecorType.Altar:
                    DrawCross(g, x, y, color);
                    break;
                case DecorType.Campfire:
                    DrawStar(g, x, y, color);
                    break;
                case DecorType.Spikes:
                    DrawTriangle(g, x, y, color, true);
                    break;
            }
        }

        private void DrawEnemy(Graphics g, int x, int y, EnemyType enemy)
        {
            var color = GetBrightEnemyColor(enemy);
            switch (enemy)
            {
                case EnemyType.EnemyIsCloseCombat:
                    DrawTriangle(g, x, y, color, false);
                    break;
                case EnemyType.EnemyRangedCombat:
                    DrawCircle(g, x, y, color);
                    break;
                case EnemyType.Boss:
                    DrawStar(g, x, y, color);
                    break;
                case EnemyType.FlyingEnemy:
                    DrawDiamond(g, x, y, color);
                    break;
                case EnemyType.Mimic:
                    DrawCross(g, x, y, color);
                    break;
            }
        }

        private void DrawDiamond(Graphics g, int x, int y, Color color)
        {
            var cx = x * _tileSize + _tileSize / 2;
            var cy = y * _tileSize + _tileSize / 2;
            var r = _tileSize / 3;
            var points = new[]
            {
                new PointF(cx, cy - r),
                new PointF(cx + r, cy),
                new PointF(cx, cy + r),
                new PointF(cx - r, cy)
            };
            using var brush = new SolidBrush(color);
            g.FillPolygon(brush, points);
        }

        private void DrawCross(Graphics g, int x, int y, Color color)
        {
            var cx = x * _tileSize + _tileSize / 2;
            var cy = y * _tileSize + _tileSize / 2;
            var r = _tileSize / 4;
            using var pen = new Pen(color, _tileSize / 8f);
            g.DrawLine(pen, cx - r, cy, cx + r, cy);
            g.DrawLine(pen, cx, cy - r, cx, cy + r);
        }

        private void DrawStar(Graphics g, int x, int y, Color color)
        {
            var cx = x * _tileSize + _tileSize / 2;
            var cy = y * _tileSize + _tileSize / 2;
            var r = _tileSize / 3;
            var points = new PointF[10];
            for (int i = 0; i < 10; i++)
            {
                var angle = Math.PI / 5 * i;
                var len = (i % 2 == 0) ? r : r / 2;
                points[i] = new PointF(
                    (int)(cx + len * Math.Sin(angle)),
                    (int)(cy - len * Math.Cos(angle))
                );
            }

            using var brush = new SolidBrush(color);
            g.FillPolygon(brush, points);
        }

        private void DrawVerticalRect(Graphics g, int x, int y, Color color)
        {
            var rect = new Rectangle(
                x * _tileSize + _tileSize / 3,
                y * _tileSize,
                _tileSize / 3,
                _tileSize
            );
            using var brush = new SolidBrush(color);
            g.FillRectangle(brush, rect);
        }

        private void DrawRectangle(Graphics g, int x, int y, Color color, float scale = 1f)
        {
            var size = (int)(_tileSize * scale);
            var offset = (_tileSize - size) / 2;
            var rect = new Rectangle(
                x * _tileSize + offset,
                y * _tileSize + offset,
                size,
                size
            );
            using var brush = new SolidBrush(color);
            g.FillRectangle(brush, rect);
        }

        private void DrawTriangle(Graphics g, int x, int y, Color color, bool inverted = false)
        {
            int top = inverted ? (y * _tileSize + _tileSize) : (y * _tileSize);
            int bottom = inverted ? (y * _tileSize) : (y * _tileSize + _tileSize);
            var points = new[]
            {
                new PointF(x * _tileSize + _tileSize / 2, top),
                new PointF(x * _tileSize + _tileSize, bottom),
                new PointF(x * _tileSize, bottom)
            };
            using var brush = new SolidBrush(color);
            g.FillPolygon(brush, points);
        }

        private void DrawCircle(Graphics g, int x, int y, Color color)
        {
            var cx = x * _tileSize + _tileSize / 2;
            var cy = y * _tileSize + _tileSize / 2;
            var r = _tileSize / 3;
            var rect = new Rectangle(cx - r, cy - r, r * 2, r * 2);
            using var brush = new SolidBrush(color);
            g.FillEllipse(brush, rect);
        }

        private void GenerateLegend(string outputPath)
        {
            int w = 400, h = 400, y = 20, step = 20;
            using var bmp = new Bitmap(w, h);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.DrawString("Легенда подземелья:", new Font("Arial", 16), Brushes.Black, 10, y);
            y += step;
            foreach (TileType t in Enum.GetValues(typeof(TileType)))
            {
                var color = GetBrightMapColor(t);
                DrawRectangle(g, 1, y / step, color, 0.5f);
                g.DrawString(t.ToString(), new Font("Arial", 12), Brushes.Black, 50, y);
                y += step;
            }

            foreach (DecorType d in Enum.GetValues(typeof(DecorType)))
            {
                if (d == DecorType.None) continue;
                DrawDecor(g, 1, y / step, d);
                g.DrawString(d.ToString(), new Font("Arial", 12), Brushes.Black, 50, y);
                y += step;
            }

            foreach (EnemyType e in Enum.GetValues(typeof(EnemyType)))
            {
                if (e == EnemyType.None) continue;
                DrawEnemy(g, 1, y / step, e);
                g.DrawString(e.ToString(), new Font("Arial", 12), Brushes.Black, 50, y);
                y += step;
            }

            bmp.Save(outputPath, ImageFormat.Png);
        }
    }
}