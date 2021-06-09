using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    // Serialized Variables
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Color materialColor;
    [SerializeField] private string horizontalAxis = "Horizontal";
    [SerializeField] private string verticalAxis = "Vertical";
    [SerializeField] private UltimateJoystick _movementJoystick;
    public Camera cam;
    
    private Rigidbody2D _rigidbody;
    private float inputHorizontal;
    private float inputVertical;

    private Vector2 _movementVector;
    private Vector2 _mousePos;

    private bool isMovementInputReceived = false;

    
    void Awake()
    {
        GetComponent<Renderer>().material.color = materialColor;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        #region Movement Scope

        _movementVector.x = _movementJoystick.HorizontalAxis;
        _movementVector.y = _movementJoystick.VerticalAxis;

        _mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        
        isMovementInputReceived = (_movementVector != Vector2.zero) ? true : false;
        // inputHorizontal = SimpleInput.GetAxis( horizontalAxis );
        // inputVertical = SimpleInput.GetAxis( verticalAxis );

        #endregion
    }

    void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _movementVector * moveSpeed * Time.fixedDeltaTime);
        
        if (_movementJoystick.GetJoystickState())
        {
            // Move forward Rotation
            Vector2 lookingDirection = _movementVector;
            float angle = Mathf.Atan2(lookingDirection.y, lookingDirection.x) * Mathf.Rad2Deg - 90f;
            
            // Mouse Rotation
            // Vector2 lookDir = _mousePos - _rigidbody.position;
            // float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            
            _rigidbody.rotation = angle;
        }
    }

    
    /*
    void OnCollisionEnter( Collision collision )
    {
        if( collision.gameObject.CompareTag( "Player" ) )
            _rigidbody.AddForce( collision.contacts[0].normal * 10f, ForceMode.Impulse );
    }
    */
    
}
