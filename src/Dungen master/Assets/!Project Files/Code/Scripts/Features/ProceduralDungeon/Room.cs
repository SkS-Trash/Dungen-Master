namespace Features.ProceduralDungeon
{
    public class Room
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Width;
        public readonly int Height;
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
}