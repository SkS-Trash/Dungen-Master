using System;

namespace Progress
{
    [Serializable]
    public class DynamicEntityData
    {
        public string guid;
        public string typeId;
        public DynamicSaveData data;
    }
}