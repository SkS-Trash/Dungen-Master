namespace ProceduralDungeon.Data.Configs
{
    [Serializable]
    public class EnemyRoomProfile
    {
        public int MinCount;
        public int MaxCount;
        public Dictionary<string, int> EnemyWeights; // EnemyType -> вес
    }

    [Serializable]
    public class EnemyConfig
    {
        public Dictionary<string, EnemyRoomProfile> RoomProfiles; // RoomType -> профиль врагов
    }
} 