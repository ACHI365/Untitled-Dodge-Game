using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Enemy
{
    // new class for casting ultimate
    public class Nme
    {
        public readonly GameObject Enemy;
        public readonly GameObject SlicePrefab;
        public Vector3 Position;
        public Quaternion Rotation;
        
        public Nme(GameObject enemy, GameObject slicePrefab, Vector3 position, Quaternion rotation)
        {
            Enemy = enemy;
            SlicePrefab = slicePrefab;
            Position = position;
            Rotation = rotation;
        }
    }
}