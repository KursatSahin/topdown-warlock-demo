using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using WarlockBrawls.Utils;
using static Utils.ContainerFacade;

public class PlayerActionController : MonoBehaviour
{
    [SerializeField] public PlayerView playerView;
    
    [SerializeField] public GameObject spellProjectilePrefab;
    [SerializeField] private UltimateJoystick _attackSpellJoystick;

    [SerializeField, Min(0.05f)] private float spellJoystickTresholdTime;
    
    [HideInInspector] public HeroData selectedHeroData;

    private bool _isReleased = false;
    private bool _isHolding = false;
    private float _timer;

    private float _attackSpellRange;
    
    private Vector3 _spellDirection;

    private void OnEnable()
    {
        _attackSpellJoystick.CustomPointerDown += OnJoystickPressedDown;
        _attackSpellJoystick.CustomPointerUp += OnJoystickRelease;
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

    private void CastSpell()
    {
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
            
            float angle = Mathf.Atan2(_spellDirection.y, _spellDirection.x) * Mathf.Rad2Deg - 90f;
            spellProjectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            //spellProjectile.GetComponent<Rigidbody2D>().velocity = (_spawnPos.position - _playerTransform.transform.position).normalized * _projectileDistanceFactor;

            var temp = (playerView.projectileSpawnPosition.position - playerView.playerTransform.transform.position);
            attackSpellData.direction =  (temp).normalized;
            
            float angle = Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg - 90f;
            spellProjectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        spellProjectile.SetActive(true);
        
        #region Attack Spell Scope

        

        #endregion End of Attack Spell Scope
    }
}
