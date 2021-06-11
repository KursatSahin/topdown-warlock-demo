using UnityEngine;

namespace WarlockBrawls.Utils
{
    [CreateAssetMenu(fileName = nameof(SpellSettingsContainer), menuName = "WarlockBrawl/SpellSettings")]
    public class SpellSettingsContainer : SingletonScriptableObject<SpellSettingsContainer>
    {
        [Min(1)] public float attackSpellSpeed = 5f;
        [Min(1)] public float attackSpellRangeMultiplier = 10f;
        [Min(1)] public float attackSpellDamage = 10f;
        [Min(.5f)] public float attackExplosionDuration = 1.4f;
        
        [Min(1f)] public float poisonDamageFromOutOfCircle = 5f;
        [Min(.5f)] public float poisonInterval = 1f;
        

    }
}