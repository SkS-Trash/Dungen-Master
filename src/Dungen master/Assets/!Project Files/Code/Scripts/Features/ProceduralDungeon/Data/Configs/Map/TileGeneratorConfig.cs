using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralDungeon.Data.Configs.Map
{
    [Serializable]
    public class TileGeneratorConfig
    {
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
        
        public void Validate()
        {
            if (Width <= 0) Width = 1;
            if (Height <= 0) Height = 1;

            if (RoomCount <= 0) RoomCount = 1;
            if (RoomMinSize <= 0) RoomMinSize = 1;
            if (RoomMaxSize <= 0) RoomMaxSize = 1;
            if (RoomMinSize > RoomMaxSize) RoomMinSize = RoomMaxSize;
            
            if (RoomCount > Width * Height) RoomCount = Width * Height;
            if (Width * Height < RoomCount) RoomCount = Width * Height;
            
            if (Width < RoomMaxSize) Width = RoomMaxSize;
            if (Height < RoomMaxSize) Height = RoomMaxSize;
            if (Width < RoomMinSize) Width = RoomMinSize;
            if (Height < RoomMinSize) Height = RoomMinSize;
        }
    }
}