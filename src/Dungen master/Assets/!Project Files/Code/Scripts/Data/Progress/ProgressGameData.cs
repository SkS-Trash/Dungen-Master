using System;
using ProceduralDungeon;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Progress
{
    [Serializable]
    public class ProgressGameData
    {
        [BoxGroup("Game"), HideLabel] public GameProgressData GameProgress = new();
        [Space] [BoxGroup("Level"), HideLabel] public LevelProgressData LevelProgress = new();
    }

    #region Game progress

    [Serializable]
    public class GameProgressData
    {
        public GameState GameState;
    }

    #endregion

    #region Level progress

    [Serializable]
    public class LevelProgressData
    {
        public int LevelIndex;
        [Space] public PlayerLevelData PlayerLevelData = new();
        [Space] public EnemyLevelData[] EnemiesLevelData;
    }

    [Serializable]
    public class PlayerLevelData
    {
        public int Health;
        [Space] public Vector3Serializable Position;
        public Vector3Serializable Rotation;
    }

    [Serializable]
    public class EnemyLevelData
    {
        public int EnemyId;
        public EnemyType EnemyType;
        [Space] public int Health;
        [Space] public Vector3Serializable Position;
        public Vector3Serializable Rotation;
    }

    #endregion

    #region Other data

    [Serializable]
    public enum GameState
    {
        None,
        MainMenu,
        Game,
        Pause,
        GameOver
    }

    [Serializable]
    public class Vector3Serializable
    {
        public float x;
        public float y;
        public float z;

        public Vector3Serializable(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public static implicit operator Vector3(Vector3Serializable vector)
        {
            return new Vector3(vector.x, vector.y, vector.z);
        }

        public static implicit operator Vector3Serializable(Vector3 vector)
        {
            return new Vector3Serializable(vector);
        }
    }

    #endregion
}