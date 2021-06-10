using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Joystick And Dependencies")]
    [SerializeField] private UltimateJoystick _movementJoystick;
    [SerializeField] private float _moveSpeed = 5f;
    
    [SerializeField] public PlayerView playerView;

    private Transform _playerTransform;

    public Camera cam;
    
    private Rigidbody2D _rigidbody;
    private Vector2 _movementVector;

    void Awake()
    {
        _rigidbody = playerView.playerTransform.GetComponent<Rigidbody2D>();
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
        _rigidbody.MovePosition(_rigidbody.position + _movementVector * _moveSpeed * Time.fixedDeltaTime);

        if (_movementJoystick.GetJoystickState())
        {
            // Move forward Rotation
            Vector2 lookingDirection = _movementVector;
            float angle = Mathf.Atan2(lookingDirection.y, lookingDirection.x) * Mathf.Rad2Deg - 90f;

            _rigidbody.rotation = angle;
        }
        #endregion End of Movement Scope
    }

    
    /*
    void OnCollisionEnter( Collision collision )
    {
        if( collision.gameObject.CompareTag( "Player" ) )
            _rigidbody.AddForce( collision.contacts[0].normal * 10f, ForceMode.Impulse );
    }
    */
    
}
