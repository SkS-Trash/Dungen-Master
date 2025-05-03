using ProceduralDungeon.Data;

namespace ProceduralDungeon
{
    public class Room
    {
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }
        public int CenterX => X + Width / 2;
        public int CenterY => Y + Height / 2;
        public RoomType Type { get; }

        public Room(int x, int y, int width, int height, RoomType type)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Type = type;
        }

        public bool Intersects(Room other) =>
            X <= other.X + other.Width && X + Width >= other.X &&
            Y <= other.Y + other.Height && Y + Height >= other.Y;
    }
}