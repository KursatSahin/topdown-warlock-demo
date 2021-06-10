using System;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using Utils;

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

    private void OnDisable()
    {
        transform.DOKill();
    }

    void FixedUpdate ()
    {
        /*var diff = Time.deltaTime * speed;
        _distance += diff;
        transform.position += (Vector3)direction * diff;

        if(_distance > range)
        {
            Explode(true);
        }*/
    }

    public void ThrowSpell(Transform playerTransform)
    {
        EventManager.GetInstance().Notify(Events.ThrowSpellStart, (Vector3)direction.normalized);

        gameObject.SetActive(false);
        
        // Move forward Rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        playerTransform.GetComponent<Rigidbody2D>().DORotate(angle, .2f).OnComplete((() =>
        {
            // Set position of projectile
            transform.position = playerTransform.GetComponent<PlayerView>().projectileSpawnPosition.position;
            // Set rotation of projectile
            var temp = (playerTransform.GetComponent<PlayerView>().projectileSpawnPosition.position - playerTransform.transform.position);
            float angle = Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            // Set active true
            gameObject.SetActive(true);
            
            EventManager.GetInstance().Notify(Events.ThrowSpellEnd);
            transform.DOMove(transform.position + (Vector3)direction.normalized * range, range/speed).OnComplete((() =>
            {
                Explode(true);
            }));
        }));
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