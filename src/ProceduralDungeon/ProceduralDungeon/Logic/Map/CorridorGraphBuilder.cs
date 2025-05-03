namespace ProceduralDungeon
{
    public class CorridorGraphBuilder
    {
        private readonly Random _random;

        public CorridorGraphBuilder(Random random)
        {
            _random = random;
        }

        public List<Edge> BuildGraph(List<Room> rooms, int k = 3)
        {
            var edges = new List<Edge>();
            for (var i = 0; i < rooms.Count; i++)
            {
                var dists = new List<(int j, float dist)>();
                for (var j = 0; j < rooms.Count; j++)
                {
                    if (i == j) continue;
                    dists.Add((j, CalculateDistance(rooms[i], rooms[j])));
                }

                foreach (var (j, dist) in dists.OrderBy(t => t.dist).Take(k))
                {
                    if (!edges.Any(e => (e.RoomA == i && e.RoomB == j) || (e.RoomA == j && e.RoomB == i)))
                        edges.Add(new Edge(i, j, dist));
                }
            }

            return edges;
        }

        public List<Edge> KruskalMST(int roomCount, List<Edge> edges)
        {
            var parent = new int[roomCount];
            for (var i = 0; i < roomCount; i++) parent[i] = i;
            var mst = new List<Edge>();
            foreach (var edge in edges.OrderBy(e => e.Weight))
            {
                int a = FindSet(edge.RoomA, parent), b = FindSet(edge.RoomB, parent);
                if (a == b) continue;
                mst.Add(edge);
                UnionSets(a, b, parent);
            }

            return mst;
        }

        public List<Edge> SelectExtraEdges(List<Edge> edges, List<Edge> mstEdges, int extraCount = 2)
        {
            var extraEdges = new List<Edge>();
            var nonMstEdges = edges.Except(mstEdges).ToList();
            extraCount = Math.Min(extraCount, nonMstEdges.Count);
            for (var i = 0; i < extraCount; i++)
            {
                var idx = _random.Next(nonMstEdges.Count);
                extraEdges.Add(nonMstEdges[idx]);
                nonMstEdges.RemoveAt(idx);
            }

            return extraEdges;
        }

        private float CalculateDistance(Room a, Room b)
        {
            var dx = a.CenterX - b.CenterX;
            var dy = a.CenterY - b.CenterY;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        private int FindSet(int x, int[] parent)
        {
            return parent[x] == x ? x : parent[x] = FindSet(parent[x], parent);
        }

        private void UnionSets(int x, int y, int[] parent)
        {
            parent[FindSet(x, parent)] = FindSet(y, parent);
        }
    }

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