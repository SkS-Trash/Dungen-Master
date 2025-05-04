using System;
using ProceduralDungeon.Data.Types;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralDungeon.Data.Configs.Enemy
{
    [Serializable]
    public class EnemyGeneratorConfig
    {
        [field: SerializeField, HideLabel, Title("Профили врагов", titleAlignment: TitleAlignments.Centered),
                DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout, IsReadOnly = true,
                    KeyLabel = "Тип", KeyColumnWidth = 100, ValueLabel = "Профиль")]
        public SerializableDictionary<RoomType, EnemyRoomProfile> RoomProfiles { get; private set; } = new();

        public EnemyGeneratorConfig()
        {
        }

        public EnemyGeneratorConfig(SerializableDictionary<RoomType, EnemyRoomProfile> roomProfiles)
        {
            RoomProfiles = roomProfiles;
        }

        public void Validate()
        {
            RoomProfilesValidate();
            foreach (var profile in RoomProfiles)
                profile.Value.Validate();
        }

        private void RoomProfilesValidate()
        {
            var roomTypes = Enum.GetValues(typeof(RoomType));
            foreach (RoomType roomType in roomTypes)
            {
                if (roomType == RoomType.Treatment) continue;
                if (!RoomProfiles.ContainsKey(roomType))
                {
                    RoomProfiles.Add(roomType, new EnemyRoomProfile());
                }
            }
        }

        private void RemoveTreatmentRoomProfile()
        {
            RoomProfiles.Remove(RoomType.Treatment);
        }
    }
}