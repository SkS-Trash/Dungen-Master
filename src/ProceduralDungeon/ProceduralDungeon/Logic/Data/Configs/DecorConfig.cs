namespace ProceduralDungeon.Data.Configs
{
    [Serializable]
    public class DecorRoomProfile
    {
        public int BaseDensity;
        public List<string> SpecialObjects; // DecorType
        public Dictionary<string, int> DecorWeights; // DecorType -> вес
    }

    [Serializable]
    public class DecorConfig
    {
        public float SpecialObjectChance; // вероятность выпадения specialObjects
        public Dictionary<string, DecorRoomProfile> RoomProfiles; // RoomType -> профиль декора
    }
} 