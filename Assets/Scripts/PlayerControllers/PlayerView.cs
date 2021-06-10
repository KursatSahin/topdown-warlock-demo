using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WarlockBrawls.Utils;

public class PlayerView : MonoBehaviour
{
    public Transform playerTransform;
    public Transform projectileSpawnPosition;
    public SpellIndicatorView spellIndicator;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void SetHero(HeroData data)
    {
        _spriteRenderer.sprite = data.firstFrame;
        _animator.runtimeAnimatorController = data.animatorController;
    }
        
}