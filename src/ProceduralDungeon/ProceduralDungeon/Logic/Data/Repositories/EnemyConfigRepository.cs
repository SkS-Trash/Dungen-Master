using ProceduralDungeon.Data.Configs;

namespace ProceduralDungeon.Data.Repositories
{
    public static class EnemyConfigRepository
    {
        public static EnemyConfig GetTestConfig()
        {
            return new EnemyConfig
            {
                RoomProfiles = new Dictionary<string, EnemyRoomProfile>
                {
                    ["Hard"] = new()
                    {
                        MinCount = 1,
                        MaxCount = 1,
                        EnemyWeights = new Dictionary<string, int>
                        {
                            ["Boss"] = 100
                        }
                    },
                    ["Trap"] = new()
                    {
                        MinCount = 0,
                        MaxCount = 0,
                        EnemyWeights = new Dictionary<string, int>()
                    },
                    ["Default"] = new()
                    {
                        MinCount = 0,
                        MaxCount = 2,
                        EnemyWeights = new Dictionary<string, int>
                        {
                            ["EnemyIsCloseCombat"] = 50,
                            ["EnemyRangedCombat"] = 50
                        }
                    }
                }
            };
        }
    }
}