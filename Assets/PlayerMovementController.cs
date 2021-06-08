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

    private Rigidbody2D _rigidbody;
    private float inputHorizontal;
    private float inputVertical;

    private Vector2 _movement;
    private Vector2 _rotation;

    private bool inputIsValid = false;
    
    void Awake()
    {
        GetComponent<Renderer>().material.color = materialColor;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        #region Movement Scope
        _movement.x = SimpleInput.GetAxis( horizontalAxis );
        _movement.y = SimpleInput.GetAxis( verticalAxis );

        inputIsValid = (_movement != Vector2.zero) ? true : false;
        // inputHorizontal = SimpleInput.GetAxis( horizontalAxis );
        // inputVertical = SimpleInput.GetAxis( verticalAxis );

        if (inputIsValid)
        {
            Vector2 lookingDirection = _movement;
            float angle = Mathf.Atan2(lookingDirection.y, lookingDirection.x) * Mathf.Rad2Deg - 90f;
            
            _rigidbody.rotation = (angle);    
        }

        #endregion
    }

    void FixedUpdate()
    {
        //_rigidbody.AddRelativeForce( new Vector3( 0f, 0f, inputVertical ) * 10f );
        _rigidbody.MovePosition(_rigidbody.position + _movement * moveSpeed * Time.fixedDeltaTime);


        //rigidbodyRotation.eulerAngles = angle;
    }

    
    /*
    void OnCollisionEnter( Collision collision )
    {
        if( collision.gameObject.CompareTag( "Player" ) )
            _rigidbody.AddForce( collision.contacts[0].normal * 10f, ForceMode.Impulse );
    }
    */
    
}
