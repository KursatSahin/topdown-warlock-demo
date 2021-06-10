using System;
using UnityEngine;

namespace WarlockBrawls.Utils
{
    [CreateAssetMenu(fileName = nameof(HeroDataContainer), menuName = "WarlockBrawl/HeroDataContainer")]
    public class HeroDataContainer : SingletonScriptableObject<HeroDataContainer>
    {
        [Header("Zeus Dependencies")] 
        public HeroData zeus;
        
        [Header("Poseidon Dependencies")] 
        public HeroData poseidon;
        
        [Header("Hades Dependencies")] 
        public HeroData hades;
        
    }

    [Serializable]
    public class HeroData
    {
        public Sprite firstFrame;
        public GameObject prefab;
        public GameObject spellVfx;
        public GameObject spellImpactVfx;
        public RuntimeAnimatorController animatorController;
    }
}
