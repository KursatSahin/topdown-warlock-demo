using DG.Tweening;
using Lean.Pool;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [SerializeField] private Transform _spawnPos;
    [SerializeField] private GameObject _spellProjectilePrefab;
    [SerializeField] private UltimateJoystick _attackSpellJoystick;
    [SerializeField] private SpellIndicatorUIController _spellDirectionIndicator;
    
    [SerializeField, Min(1)] private float _attackSpellSpeed = 5f;
    [SerializeField, Min(1)] private float _attackSpellRangeMultiplier = 10f;
    [SerializeField, Min(1)] private float _attackSpellDamage = 10f;

    [SerializeField, Min(0.05f)] private float spellJoystickTresholdTime;

    private Transform _playerTransform;
    private bool _isReleased = false;
    private bool _isHolding = false;
    private float _timer;

    private float _attackSpellRange;
    
    private Vector3 _spellDirection; 
    
    private void Awake()
    {
        _playerTransform = GetComponent<Transform>();
    }

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
                _spellDirectionIndicator.transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));

                _attackSpellRange = _attackSpellJoystick.GetDistance() * _attackSpellRangeMultiplier;
                _spellDirectionIndicator.SetUI(_attackSpellRange);
                //Debug.Log($"Range : {_attackSpellRange} | Joystick Distance : {_attackSpellJoystick.GetDistance()}" );

                
                if (!_spellDirectionIndicator.gameObject.activeSelf)
                {
                    _spellDirectionIndicator.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            if (_spellDirectionIndicator.gameObject.activeSelf)
            {
                _spellDirectionIndicator.ResetUI();
                _spellDirectionIndicator.gameObject.SetActive(false);
            }
        }
    }

    private void CastSpell()
    {
        var spellProjectile = LeanPool.Spawn(_spellProjectilePrefab);
        spellProjectile.transform.position = _spawnPos.position;

        var spellData = spellProjectile.GetComponent<SpellBase>();

        spellData.speed = _attackSpellSpeed;
        spellData.range = _attackSpellRange;
        spellData.damage = _attackSpellDamage;
        
        if (_timer > spellJoystickTresholdTime) // in sec
        {
            //spellProjectile.GetComponent<Rigidbody2D>().velocity = (_spellDirection).normalized * _projectileDistanceFactor;
            spellData.direction =  (_spellDirection.normalized);
            
            float angle = Mathf.Atan2(_spellDirection.y, _spellDirection.x) * Mathf.Rad2Deg - 90f;
            spellProjectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            //spellProjectile.GetComponent<Rigidbody2D>().velocity = (_spawnPos.position - _playerTransform.transform.position).normalized * _projectileDistanceFactor;

            var temp = (_spawnPos.position - _playerTransform.transform.position);
            spellData.direction =  (temp).normalized;
            
            float angle = Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg - 90f;
            spellProjectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        spellProjectile.SetActive(true);
    }
}
