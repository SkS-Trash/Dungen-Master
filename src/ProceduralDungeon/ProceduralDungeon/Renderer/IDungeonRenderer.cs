namespace ProceduralDungeon
{
    public interface IDungeonRenderer
    {
        void RenderDungeon(DungeonGenerator generator, int cellSize);
    }
}