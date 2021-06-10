using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "New Explosion Settings", menuName = "WarlockBrawl/Explosion Settings")]
    public class ExplosionSettings : ScriptableObject
    {
        public float ExplosionForce;
        public float ExplosionRadius;
    }
}