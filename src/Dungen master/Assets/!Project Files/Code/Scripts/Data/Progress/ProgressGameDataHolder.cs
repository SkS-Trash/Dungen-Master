using Sirenix.OdinInspector;
using UnityEngine;

namespace Progress
{
    [CreateAssetMenu(fileName = nameof(ProgressGameDataHolder),
        menuName = "Data/Progress/" + nameof(ProgressGameDataHolder))]
    public class ProgressGameDataHolder : ScriptableObject
    {
        [field: BoxGroup("Progress"), HideLabel, SerializeField]
        public ProgressGameData ProgressGameData { get; private set; } = new();

        [Button("Reset progress")]
        public void ResetProgress()
        {
            ProgressGameData = new ProgressGameData();
        }
    }
}