namespace ProceduralDungeon.Data.Configs
{
    [Serializable]
    public class MapGeneratorConfig
    {
        public int Width;
        public int Height;
        public int RoomCount;
        public int RoomMinSize;
        public int RoomMaxSize;
        public string GenerationMode; // Rectangular, Cavern, BSP
    }
} 