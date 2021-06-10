using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Joystick And Dependencies")]
    [SerializeField] private UltimateJoystick _movementJoystick;
    [SerializeField] private float _moveSpeed = 5f;
    
    [SerializeField] private Button _dashButton;
    [SerializeField] private float _dashAmount;

    [SerializeField] public PlayerView playerView;

    private Transform _playerTransform;

    public Camera cam;
    
    private Rigidbody2D _rigidbody;
    private Vector2 _movementVector;

    private bool throwingSpell = false;

    void Awake()
    {
        _rigidbody = playerView.playerTransform.GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        EventManager.GetInstance().AddHandler(Events.ThrowSpellStart, OnThrowSpellStart);
        EventManager.GetInstance().AddHandler(Events.ThrowSpellEnd, OnThrowSpellEnd);
    }

    private void OnDisable()
    {
        EventManager.GetInstance().RemoveHandler(Events.ThrowSpellStart, OnThrowSpellStart);
        EventManager.GetInstance().RemoveHandler(Events.ThrowSpellEnd, OnThrowSpellEnd);
    }

    void Update()
    {
        #region Movement Scope
        _movementVector.x = _movementJoystick.HorizontalAxis;
        _movementVector.y = _movementJoystick.VerticalAxis;
        #endregion End of Movement Scope
    }

    void FixedUpdate()
    {
        #region Movement Scope
        if (_movementJoystick.GetJoystickState() && _dashButton.interactable)
        {
            _rigidbody.MovePosition(_rigidbody.position + _movementVector * _moveSpeed * Time.fixedDeltaTime);
            
            // Move forward Rotation
            Vector2 lookingDirection = _movementVector;
            float angle = Mathf.Atan2(lookingDirection.y, lookingDirection.x) * Mathf.Rad2Deg - 90f;

            _rigidbody.rotation = angle;
        }
        #endregion End of Movement Scope
    }

    private void OnThrowSpellEnd(object obj)
    {
        throwingSpell = false;
        _dashButton.interactable = true;
    }

    private void OnThrowSpellStart(object obj)
    {
        throwingSpell = true;
        _dashButton.interactable = false;
    }
    
    public void DashEffect()
    {
        Vector2 dashPosition = _rigidbody.transform.position + _rigidbody.transform.up * _dashAmount;
        
        _rigidbody.transform.DOMove(dashPosition,.2f).OnStart((() =>
        {
            _dashButton.interactable = false;
        })).OnComplete((() =>
        {
            _dashButton.interactable = true;
        }));
        
        Debug.Log( $"{nameof(DashEffect)} is called. Player Position is {_rigidbody.transform.position}  and dashPosition is {dashPosition}");
    }
    
    /*
    void OnCollisionEnter( Collision collision )
    {
        if( collision.gameObject.CompareTag( "Player" ) )
            _rigidbody.AddForce( collision.contacts[0].normal * 10f, ForceMode.Impulse );
    }
    */
    
}
