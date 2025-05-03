using ProceduralDungeon.Data.Configs;

namespace ProceduralDungeon.Data.Repositories
{
    public static class DecorConfigRepository
    {
        public static DecorConfig GetTestConfig()
        {
            return new DecorConfig
            {
                SpecialObjectChance = 0.3f,
                RoomProfiles = new Dictionary<string, DecorRoomProfile>
                {
                    ["Treatment"] = new()
                    {
                        BaseDensity = 4,
                        SpecialObjects = new List<string> { "Altar" },
                        DecorWeights = new Dictionary<string, int>
                        {
                            ["Chest"] = 10,
                            ["Barrel"] = 20,
                            ["Column"] = 10,
                            ["Campfire"] = 5
                        }
                    },
                    ["Trap"] = new()
                    {
                        BaseDensity = 3,
                        SpecialObjects = new List<string> { "Spikes", "PressurePlate" },
                        DecorWeights = new Dictionary<string, int>
                        {
                            ["Chest"] = 5,
                            ["Barrel"] = 10,
                            ["Column"] = 10,
                            ["Campfire"] = 2
                        }
                    },
                    ["Hard"] = new()
                    {
                        BaseDensity = 2,
                        SpecialObjects = new List<string> { "Campfire" },
                        DecorWeights = new Dictionary<string, int>
                        {
                            ["Chest"] = 10,
                            ["Barrel"] = 10,
                            ["Column"] = 10,
                            ["Campfire"] = 10
                        }
                    },
                    ["Default"] = new()
                    {
                        BaseDensity = 2,
                        SpecialObjects = new List<string> { "Barrel", "Column" },
                        DecorWeights = new Dictionary<string, int>
                        {
                            ["Chest"] = 15,
                            ["Barrel"] = 30,
                            ["Column"] = 20,
                            ["Campfire"] = 10
                        }
                    }
                }
            };
        }
    }
} 