namespace ProceduralDungeon
{
    public class Edge
    {
        public int RoomA { get; }
        public int RoomB { get; }
        public float Weight { get; }

        public Edge(int a, int b, float w)
        {
            RoomA = a;
            RoomB = b;
            Weight = w;
        }
    }
}