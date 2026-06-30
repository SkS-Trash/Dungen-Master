using Sirenix.OdinInspector;
using UnityEngine;

namespace Progress
{
    [CreateAssetMenu(fileName = nameof(ProgressGameDataHolder),
        menuName = "Data/Progress/" + nameof(ProgressGameDataHolder))]
    public class ProgressGameDataHolder : ScriptableObject
    {
        [field: BoxGroup(nameof(GlobalSaveData)), HideLabel, SerializeField]
        public GlobalSaveData GlobalSaveData { get; private set; } = new();

        [field: BoxGroup(nameof(LevelSaveData)), HideLabel, SerializeField]
        public LevelSaveData LevelSaveData { get; private set; } = new();

        [Button("Reset progress")]
        public void ResetProgress()
        {
            GlobalSaveData = new GlobalSaveData
            {
                isFirstLaunch = true,
                version = GlobalSaveData.VERSION
            };

            LevelSaveData = new LevelSaveData();
        }
    }
}