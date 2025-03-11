using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ProgressGameDataHolder),
    menuName = "Data/Progress/" + nameof(ProgressGameDataHolder))]
public class ProgressGameDataHolder : ScriptableObject
{
    [field: BoxGroup("Progress"), HideLabel, ShowInInspector]
    [field: SerializeField]
    public ProgressGameData ProgressGameData { get; private set; }

    public void ResetProgress()
    {
        ProgressGameData = new ProgressGameData();
    }
}