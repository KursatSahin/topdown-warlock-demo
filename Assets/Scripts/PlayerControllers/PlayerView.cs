using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Lean.Pool;
using UnityEngine;
using WarlockBrawls.Utils;

public class PlayerView : MonoBehaviour
{
    public Transform playerTransform;
    public Transform projectileSpawnPosition;
    public SpellIndicatorView spellIndicator;

    [SerializeField] private Hero inGameData;
    
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        inGameData = new Hero();
        inGameData.vitals.health = 100;
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
        if (inGameData.vitals.health <= 0)
        {
            LeanPool.Despawn(gameObject);
            //TODO : dead
        }
    }
        
}