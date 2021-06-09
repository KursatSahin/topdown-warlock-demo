using Lean.Pool;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [SerializeField] private Transform _spawnPos;
    [SerializeField] private GameObject _spellProjectilePrefab;
    [SerializeField] private UltimateJoystick _attackSpellJoystick;
    [SerializeField] private GameObject _spellDirectionIndicator;
    
    [SerializeField, Min(1)] float attackSpellSpeed = 5f;
    [SerializeField, Min(1)] float attackSpellRange = 10f;
    [SerializeField, Min(1)] float attackSpellDamage = 10f;

    [SerializeField, Min(0.05f)] private float spellJoystickTresholdTime;

    private Transform _playerTransform;
    private bool isReleased = false;
    private bool isHolding = false;
    private float timer;

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
        isReleased = true;
        isHolding = false;
        
        CastSpell();
    }

    private void OnJoystickPressedDown()
    {
        isReleased = false;
        isHolding = true;

        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_attackSpellJoystick.GetJoystickState())
        {
            timer += Time.deltaTime;

            if (timer > .5f)
            {
                Debug.Log("READY");
            }
            
            
            float angle = Mathf.Atan2(_spellDirection.y, _spellDirection.x) * Mathf.Rad2Deg;
            //_spellDirectionIndicator.transform.rotation = Quaternion.Euler(_spellDirection);
            _spellDirectionIndicator.SetActive(true);
            //_spellDirectionIndicator.transform.localScale = new Vector3(.1f, _attackSpellJoystick.GetDistance(), .1f);
        }
        else
        {
            _spellDirectionIndicator.SetActive(false);
        }
    }

    private void CastSpell()
    {
        var spellProjectile = LeanPool.Spawn(_spellProjectilePrefab);
        spellProjectile.transform.position = _spawnPos.position;
        _spawnPos.transform.rotation = _playerTransform.rotation;
            
        var spellData = spellProjectile.GetComponent<SpellBase>();

        spellData.speed = attackSpellSpeed;
        spellData.range = attackSpellRange;
        spellData.damage = attackSpellDamage;
        
        if (timer > spellJoystickTresholdTime) // in sec
        {
            _spellDirection = new Vector3(_attackSpellJoystick.HorizontalAxis, _attackSpellJoystick.VerticalAxis);
            //spellProjectile.GetComponent<Rigidbody2D>().velocity = (_spellDirection).normalized * _projectileDistanceFactor;
            spellData.direction =  (_spellDirection.normalized);
        }
        else
        {
            //spellProjectile.GetComponent<Rigidbody2D>().velocity = (_spawnPos.position - _playerTransform.transform.position).normalized * _projectileDistanceFactor;
            spellData.direction =  (_spawnPos.position - _playerTransform.transform.position).normalized;
        }
        
        spellProjectile.SetActive(true);
    }
}
