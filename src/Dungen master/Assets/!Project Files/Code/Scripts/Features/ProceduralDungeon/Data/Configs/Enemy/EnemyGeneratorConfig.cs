using System;
using System.Collections.Generic;
using ProceduralDungeon.Data.Types;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralDungeon.Data.Configs.Enemy
{
    [Serializable]
    public class EnemyGeneratorConfig
    {
        [field: SerializeField, HideLabel, Title("Профили врагов", titleAlignment: TitleAlignments.Centered),
                DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine, IsReadOnly = true,
                    KeyLabel = "Тип", KeyColumnWidth = 40, ValueLabel = "Профиль")]
        public SerializableDictionary<RoomType, EnemyRoomProfile> RoomProfiles { get; private set; } = new();

        public void Validate()
        {
            RoomProfilesValidate();
            RemoveTreatmentRoomProfile();

            foreach (var profile in RoomProfiles)
                profile.Value.Validate();
        }

        private void RoomProfilesValidate()
        {
            var roomTypes = Enum.GetValues(typeof(RoomType));
            foreach (RoomType roomType in roomTypes)
            {
                if (roomType == RoomType.Treatment) continue;

                RoomProfiles.TryAdd(roomType, new EnemyRoomProfile());
            }
        }

        private void RemoveTreatmentRoomProfile()
        {
            RoomProfiles.Remove(RoomType.Treatment);
        }
    }
}