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
    private Animator _playerAnimator;
    public Camera cam;
    
    private Rigidbody2D _rigidbody;
    private Vector2 _movementVector;

    private bool _throwingSpell = false;

    private bool _insideCircle = true;

    void Awake()
    {
        _rigidbody = playerView.playerTransform.GetComponent<Rigidbody2D>();
        _playerAnimator = playerView.GetComponent<Animator>();
        
        _playerAnimator.SetBool("Walk", true);
        _playerAnimator.SetBool("Glide", false);
        _playerAnimator.SetBool("Attack", false);
    }

    private void OnEnable()
    {
        EventManager.GetInstance().AddHandler(Events.ThrowSpellStart, OnThrowSpellStart);
        EventManager.GetInstance().AddHandler(Events.ThrowSpellEnd, OnThrowSpellEnd);
        EventManager.GetInstance().AddHandler(Events.OutOfCircle, OnOutOfCircle);
        EventManager.GetInstance().AddHandler(Events.InsideOfCircle, OnInsideOfCircle);
    }

    private void OnDisable()
    {
        EventManager.GetInstance().RemoveHandler(Events.ThrowSpellStart, OnThrowSpellStart);
        EventManager.GetInstance().RemoveHandler(Events.ThrowSpellEnd, OnThrowSpellEnd);
        EventManager.GetInstance().RemoveHandler(Events.OutOfCircle, OnOutOfCircle);
        EventManager.GetInstance().RemoveHandler(Events.InsideOfCircle, OnInsideOfCircle);
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

        if (_dashButton.interactable)
        {
            if (_movementJoystick.GetJoystickState())
            {
                if (_insideCircle)
                {
                    _playerAnimator.SetBool("Walk", true);
                    _playerAnimator.SetBool("Glide", false);
                    _playerAnimator.SetBool("Attack", false);
                }
                else
                {
                    _playerAnimator.SetBool("Walk", false);
                    _playerAnimator.SetBool("Glide", true);
                    _playerAnimator.SetBool("Attack", false);
                }

                
                _rigidbody.MovePosition(_rigidbody.position + _movementVector * _moveSpeed * Time.fixedDeltaTime);
                
                // Move forward Rotation
                Vector2 lookingDirection = _movementVector;
                float angle = Mathf.Atan2(lookingDirection.y, lookingDirection.x) * Mathf.Rad2Deg - 90f;

                _rigidbody.rotation = angle;
            }
            else
            {
                if (_insideCircle)
                {
                    _playerAnimator.SetBool("Walk", false);
                    _playerAnimator.SetBool("Glide", false);
                    _playerAnimator.SetBool("Attack", false);
                }
                else
                {
                    _playerAnimator.SetBool("Walk", false);
                    _playerAnimator.SetBool("Glide", true);
                    _playerAnimator.SetBool("Attack", false);
                }
            }
        }
        #endregion End of Movement Scope
    }

    private void OnInsideOfCircle(object obj)
    {
        _insideCircle = true;
    }

    private void OnOutOfCircle(object obj)
    {
        _insideCircle = false;
    }
    
    private void OnThrowSpellEnd(object obj)
    {
        _throwingSpell = false;
        _dashButton.interactable = true;
    }

    private void OnThrowSpellStart(object obj)
    {
        _throwingSpell = true;
        _dashButton.interactable = false;
    }
    
    public void DashEffect()
    {
        Vector2 dashPosition = _rigidbody.transform.position + _rigidbody.transform.up * _dashAmount;
        
        _rigidbody.transform.DOMove(dashPosition,.2f).OnStart((() =>
        {
            _playerAnimator.SetBool("Walk", false);
            _playerAnimator.SetBool("Glide", true);
            _playerAnimator.SetBool("Attack", false);
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
