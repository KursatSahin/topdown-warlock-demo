using Lean.Pool;
using UnityEngine;

public abstract class SpellBase : MonoBehaviour
{
    [HideInInspector] public float cooldown;

    protected virtual void Explode()
    {
        LeanPool.Despawn(gameObject);
    }
}