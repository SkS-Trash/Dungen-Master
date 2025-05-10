public static class DataPaths
{
    private const string DATA_PATH = "Data/";
    private const string SINGLETONS_PATH = DATA_PATH + "SingleInstance/";
    private const string DUNGEON_PATH = DATA_PATH + "Dungeon/";

    public const string PROGRESS_GAME_DATA_HOLDER = SINGLETONS_PATH + "ProgressGameDataHolder";
    
    public const string LEVEL_STYLE_CONFIGS_PATH = DUNGEON_PATH + "LevelStyle/";
    public const string BASE_GENERATOR_CONFIG = DUNGEON_PATH + "BaseGeneratorConfig";
    
    public const string GAME_EVENT_OBSERVER_COLLECTION = DATA_PATH + "GameEventObserver/GameEventObserverCollection";
}