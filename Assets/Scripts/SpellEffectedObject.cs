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
            var spellBase = other.GetComponent<SpellBase>();
            // TODO : spellBase.damage

            _rb.AddExplosionForcev2(explosionSettings.ExplosionForce, other.transform.position,
                explosionSettings.ExplosionRadius);
            
            //_rb.AddForce(Vector2.left*1000);
            
            LeanPool.Despawn(other.gameObject);
        }
    }
}
