using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ProceduralDungeon
{
    [System.Serializable]
    public class CellConfig<TTile> where TTile : System.Enum
    {
        public CellConfig()
        {
        }

        public CellConfig(TTile type)
        {
            Type = type;
        }

        [field: SerializeField, HideInInspector]
        public TTile Type { get; private set; }

        [field: SerializeField, HideLabel]
        [field: ListDrawerSettings(ShowFoldout = false, ShowPaging = false, ShowItemCount = false)]
        public AssetReference[] Prefabs { get; private set; } = new AssetReference[1];
    }
}