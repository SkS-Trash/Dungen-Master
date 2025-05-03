namespace ProceduralDungeon
{
    public class DecorProfileProvider
    {
        public (int baseDensity, List<DecorType> specialObjects) GetRoomDecorProfile(RoomType type)
        {
            return type switch
            {
                RoomType.Treatment => (4, new List<DecorType> { DecorType.Altar }),
                RoomType.Trap => (3, new List<DecorType> { DecorType.Spikes, DecorType.PressurePlate }),
                RoomType.Hard => (2, new List<DecorType> { DecorType.Campfire }),
                _ => (2, new List<DecorType> { DecorType.Barrel, DecorType.Column })
            };
        }
    }
} 