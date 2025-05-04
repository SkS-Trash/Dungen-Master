using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralDungeon.Data.Configs.Map
{
    [Serializable]
    public class MapGeneratorConfig
    {
        public MapGeneratorConfig(
            int width,
            int height,
            int roomCount,
            int roomMinSize,
            int roomMaxSize,
            MapGenerationMode generationMode
        )
        {
            Width = width;
            Height = height;
            RoomCount = roomCount;
            RoomMinSize = roomMinSize;
            RoomMaxSize = roomMaxSize;
            GenerationMode = generationMode;
        }

        public MapGeneratorConfig()
        {
        }

        [field: SerializeField, LabelText("Ширина карты")]
        public int Width { get; private set; } = 50;

        [field: SerializeField, LabelText("Высота карты")]
        public int Height { get; private set; } = 50;

        [field: SerializeField, LabelText("Количество комнат")]
        public int RoomCount { get; private set; } = 10;

        [field: SerializeField, LabelText("Минимальный размер комнаты")]
        public int RoomMinSize { get; private set; } = 3;

        [field: SerializeField, LabelText("Максимальный размер комнаты")]
        public int RoomMaxSize { get; private set; } = 7;

        [field: SerializeField, LabelText("Режим генерации"), EnumToggleButtons]
        public MapGenerationMode GenerationMode { get; private set; } = MapGenerationMode.Rectangular;
    }
}