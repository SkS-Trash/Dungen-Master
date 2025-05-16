using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Items
{
    [CreateAssetMenu(fileName = nameof(ItemConfig), menuName = "Data/Items/" + nameof(ItemConfig))]
    public class ItemConfig : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; } = "New Item";
        [field: SerializeField] public AssetReference ObjectReference { get; private set; }
    }
}