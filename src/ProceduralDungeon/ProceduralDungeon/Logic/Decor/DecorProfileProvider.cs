namespace ProceduralDungeon
{
    public class DecorProfileProvider
    {
        public (int baseDensity, List<DecorType> specialObjects) GetRoomDecorProfile(RoomType type)
        {
            return type switch
            {
                RoomType.Treatment => (4, [DecorType.Altar]),
                RoomType.Trap => (3, [DecorType.Spikes, DecorType.PressurePlate]),
                RoomType.Hard => (2, [DecorType.Campfire]),
                _ => (2, new List<DecorType> { DecorType.Barrel, DecorType.Column })
            };
        }
    }
}