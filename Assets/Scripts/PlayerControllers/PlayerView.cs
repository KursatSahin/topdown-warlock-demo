using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Lean.Pool;
using UnityEngine;
using WarlockBrawls.Utils;
using static Utils.ContainerFacade;

public class PlayerView : MonoBehaviour
{
    public Transform playerTransform;
    public Transform projectileSpawnPosition;
    public SpellIndicatorView spellIndicator;
    public HealthBar healthBar;

    [SerializeField] private Hero inGameData;
    
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    public float distanceFromOrigin;
    
    public float poisonTime = 0;
    
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        inGameData = new Hero();
        
        inGameData.vitals.health = 100;
        healthBar.SetMaxHealth(100);
    }

    private void Update()
    {
        distanceFromOrigin = Vector2.Distance(transform.position, Vector2.zero);
        
        if (distanceFromOrigin > GameManager.circleAreaRadius)
        {
            poisonTime += Time.deltaTime;
            if (poisonTime > SpellSettings.poisonInterval)
            {
                GetDamaged(SpellSettings.poisonDamageFromOutOfCircle);
                poisonTime = 0;
            }
        }
        else
        {
            poisonTime = 0;
        }
    }

    public void SetHero(HeroData data)
    {
        _spriteRenderer.sprite = data.firstFrame;
        _animator.runtimeAnimatorController = data.animatorController;
        inGameData.vitals.health = 100;
    }

    public void GetDamaged(float damageAmount)
    {
        inGameData.vitals.health -= damageAmount;
        healthBar.SetHealth((int)inGameData.vitals.health);
        if (inGameData.vitals.health <= 0)
        {
            LeanPool.Despawn(gameObject);
            //TODO : dead
        }
    }
        
}