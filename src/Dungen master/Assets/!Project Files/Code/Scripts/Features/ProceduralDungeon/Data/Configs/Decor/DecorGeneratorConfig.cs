using System;
using ProceduralDungeon.Data.Types;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralDungeon.Data.Configs.Decor
{
    [Serializable]
    public class DecorGeneratorConfig
    {
        [field: SerializeField, HideLabel, Title("Шанс появления декора", titleAlignment: TitleAlignments.Centered)]
        public float SpecialObjectChance { get; private set; } = 0.1f;

        [field: SerializeField, HideLabel, Title("Профили декора", titleAlignment: TitleAlignments.Centered),
                DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout, IsReadOnly = true,
                    KeyLabel = "Тип", KeyColumnWidth = 100, ValueLabel = "Профиль")]
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
                if (!RoomProfiles.ContainsKey(roomType))
                {
                    RoomProfiles.Add(roomType, new DecorRoomProfile());
                }
            }
        }
    }
}