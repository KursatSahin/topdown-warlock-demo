using System;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using Utils;
using WarlockBrawls.Utils;
using static Utils.ContainerFacade;

public class PlayerActionController : MonoBehaviour
{
    [SerializeField] public PlayerView playerView;
    
    [SerializeField] public GameObject spellProjectilePrefab;
    [SerializeField] private UltimateJoystick _attackSpellJoystick;

    [SerializeField, Min(0.05f)] private float spellJoystickTresholdTime;
    
    [HideInInspector] public HeroData selectedHeroData;
    
    private Animator _playerAnimator;

    private bool _isReleased = false;
    private bool _isHolding = false;
    private float _timer;

    private float _attackSpellRange;
    
    private Vector3 _spellDirection;

    private void Awake()
    {
        _playerAnimator = playerView.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _attackSpellJoystick.CustomPointerDown += OnJoystickPressedDown;
        _attackSpellJoystick.CustomPointerUp += OnJoystickRelease;
        EventManager.GetInstance().AddHandler(Events.ThrowSpellStart, OnThrowSpellStart);
        EventManager.GetInstance().AddHandler(Events.ThrowSpellEnd, OnThrowSpellEnd);
    }

    private void OnDisable()
    {
        _attackSpellJoystick.CustomPointerDown -= OnJoystickPressedDown;
        _attackSpellJoystick.CustomPointerUp -= OnJoystickRelease;
        EventManager.GetInstance().RemoveHandler(Events.ThrowSpellStart, OnThrowSpellStart);
        EventManager.GetInstance().RemoveHandler(Events.ThrowSpellEnd, OnThrowSpellEnd);
    }

    // Update is called once per frame
    void Update()
    {
        if (_attackSpellJoystick.GetJoystickState())
        {
            _timer += Time.deltaTime;

            if (_timer > spellJoystickTresholdTime)
            {
                _spellDirection = new Vector3(_attackSpellJoystick.HorizontalAxis, _attackSpellJoystick.VerticalAxis);
                
                float angle = Mathf.Atan2(_spellDirection.y, _spellDirection.x) * Mathf.Rad2Deg - 90f;
                playerView.spellIndicator.transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));

                _attackSpellRange = _attackSpellJoystick.GetDistance() * SpellSettings.attackSpellRangeMultiplier;
                playerView.spellIndicator.SetUI(_attackSpellRange);
                //Debug.Log($"Range : {_attackSpellRange} | Joystick Distance : {_attackSpellJoystick.GetDistance()}" );

                
                if (!playerView.spellIndicator.gameObject.activeSelf)
                {
                    playerView.spellIndicator.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            if (playerView.spellIndicator.gameObject.activeSelf)
            {
                playerView.spellIndicator.ResetUI();
                playerView.spellIndicator.gameObject.SetActive(false);
            }
        }
    }

    private void OnJoystickRelease()
    {
        _isReleased = true;
        _isHolding = false;
        
        CastSpell();
    }

    private void OnJoystickPressedDown()
    {
        _isReleased = false;
        _isHolding = true;

        _timer = 0f;
    }

    private void OnThrowSpellEnd(object obj)
    {
        _attackSpellJoystick.EnableJoystick();
    }

    private void OnThrowSpellStart(object obj)
    {
        _attackSpellJoystick.DisableJoystick();
    }
    
    private void CastSpell()
    {
        #region Attack Spell Scope
        
        var spellProjectile = LeanPool.Spawn(spellProjectilePrefab);
        spellProjectile.transform.position = playerView.projectileSpawnPosition.position;

        var attackSpellData = spellProjectile.GetComponent<AttackSpell>();

        attackSpellData.speed = SpellSettings.attackSpellSpeed;
        attackSpellData.range = _attackSpellRange;
        attackSpellData.damage = SpellSettings.attackSpellDamage;
        attackSpellData.hitVfxPrefab = selectedHeroData.spellImpactVfx;
        
        if (_timer > spellJoystickTresholdTime) // in sec
        {
            //spellProjectile.GetComponent<Rigidbody2D>().velocity = (_spellDirection).normalized * _projectileDistanceFactor;
            attackSpellData.direction =  (_spellDirection.normalized);
            
            // float angle = Mathf.Atan2(_spellDirection.y, _spellDirection.x) * Mathf.Rad2Deg - 90f;
            // spellProjectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            //spellProjectile.GetComponent<Rigidbody2D>().velocity = (_spawnPos.position - _playerTransform.transform.position).normalized * _projectileDistanceFactor;

            var temp = (playerView.projectileSpawnPosition.position - playerView.playerTransform.transform.position);
            attackSpellData.direction =  (temp).normalized;
            
            float angle = Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg - 90f;
            spellProjectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        _playerAnimator.SetBool("Walk", false);
        _playerAnimator.SetBool("Glide", true);
        _playerAnimator.SetBool("Attack", false);
        
        //spellProjectile.SetActive(true);
        attackSpellData.ThrowSpell(playerView.playerTransform);

        #endregion End of Attack Spell Scope
    }
}
