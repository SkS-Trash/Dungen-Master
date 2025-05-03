using System.Diagnostics;
using System.Drawing;
using ProceduralDungeon.Data;

namespace ProceduralDungeon;

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
            DecorType.Campfire => Color.OrangeRed,
            DecorType.Spikes => Color.DarkRed,
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