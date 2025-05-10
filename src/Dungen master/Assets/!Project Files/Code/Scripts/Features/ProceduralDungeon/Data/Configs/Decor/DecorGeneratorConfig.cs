using System;
using System.Collections.Generic;
using ProceduralDungeon.Data.Types;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralDungeon.Data.Configs.Decor
{
    [Serializable]
    public class DecorGeneratorConfig
    {
        [field: SerializeField, HideLabel, Title("Профили декора", titleAlignment: TitleAlignments.Centered),
                DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine, IsReadOnly = true,
                    KeyLabel = "Тип", KeyColumnWidth = 40, ValueLabel = "Профиль")]
        public SerializableDictionary<RoomType, DecorRoomProfile> RoomProfiles { get; private set; } = new();

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
                RoomProfiles.TryAdd(roomType, new DecorRoomProfile());
            }
        }
    }
}