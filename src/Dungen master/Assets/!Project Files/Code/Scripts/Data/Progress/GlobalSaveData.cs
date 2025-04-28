using System;

namespace Progress
{
    [Serializable]
    public class GlobalSaveData
    {
        public const int VERSION = 1;

        public bool IsInDungeon => currentLevelIndex > -1;

        public uint version = VERSION;
        public bool isFirstLaunch = true;
        public int currentLevelIndex = -1;
    }
}