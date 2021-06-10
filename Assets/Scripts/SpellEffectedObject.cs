using DefaultNamespace;
using Lean.Pool;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class SpellEffectedObject : MonoBehaviour
{
    [SerializeField] private ExplosionSettings explosionSettings;
    
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spell"))
        {
            var spellBase = other.GetComponent<AttackSpell>();
            // TODO : spellBase.damage

            _rb.AddExplosionForce(explosionSettings.ExplosionForce, other.transform.position,
                explosionSettings.ExplosionRadius);
            
            spellBase.Explode(false);

            if (TryGetComponent(out PlayerView playerView))
            {
                playerView.GetDamaged(spellBase.damage);
            }
        }
    }
}
