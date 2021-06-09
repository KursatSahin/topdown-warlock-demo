using Lean.Pool;
using UnityEngine;

public abstract class SpellBase : MonoBehaviour
{
    [HideInInspector] public float speed;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public float range;
    [HideInInspector] public float damage;

    protected virtual void Explode()
    {
        LeanPool.Despawn(gameObject);
    }
    
}