using System;
using UnityEngine;

namespace Progress
{
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
        
        public static implicit operator Quaternion(Vector3Serializable vector)
        {
            return Quaternion.Euler(vector.x, vector.y, vector.z);
        }
        
        public static implicit operator Vector3Serializable(Quaternion quaternion)
        {
            return new Vector3Serializable(quaternion.eulerAngles);
        }
    }
}