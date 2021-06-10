using System;
using Lean.Pool;
using UnityEngine;

public class AttackSpell : SpellBase
{
    [HideInInspector] public GameObject hitVfxPrefab;
    [HideInInspector] public float speed;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public float range;
    [HideInInspector] public float damage;

    private float _distance;

    #region Unity Events

    void OnEnable ()
    {
        _distance = 0.0f;
    }

    void FixedUpdate ()
    {
        var diff = Time.deltaTime * speed;
        _distance += diff;
        transform.position += (Vector3)direction * diff;

        if(_distance > range)
        {
            Explode(true);
        }
    }

    public void Explode(bool withoutExplosion)
    {
        if (withoutExplosion)
        {
            base.Explode();
        }
        else
        {
            Debug.Log("Explosion created");
            var explosion = LeanPool.Spawn(hitVfxPrefab);
            explosion.transform.position = transform.position;
        
            explosion.SetActive(true);

            base.Explode();
        }
    }

    #endregion
}