using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;

    bool isAlive = true;
    
    Vector2 moveInput;
    Rigidbody2D _rigidbody2D;
    Animator _animator;
    CapsuleCollider2D _bodyCollider2D;
    BoxCollider2D _myFeetCollider2D;
    SpriteRenderer _spriteRenderer;

    [SerializeField] GameObject Bullet;
    [SerializeField] Transform gun;
    
    CoinsPickup _coinsPickup;
    
    enum MovementState { idle  , isShooting , isRunning , isClimbing }
    MovementState state;
    
    
    
    float gravityScaleAtStart;
    
    
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _bodyCollider2D = GetComponent<CapsuleCollider2D>();
        _myFeetCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        gravityScaleAtStart = _rigidbody2D.gravityScale;
    }

    void Update()
    {
        if (!isAlive)
        {
            return;
        }
        
        _animator.SetInteger("state" , (int)state);

        Run();
        FlipSprite();
        ClimbLadder();
        PlayerDeath();
    }
    
    void OnFire(InputValue value)
    {
        if (!isAlive)
        {

            return;
        }
        state = MovementState.isShooting;
        Instantiate(Bullet , gun.position , transform.localRotation);
    }
    
    void OnMove(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }
        
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }
        
        if (!_myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            state = MovementState.idle;
            return; 
        }
        
        if (value.isPressed)
        {
            _rigidbody2D.velocity = new Vector2(0f, jumpSpeed);
        }
    }
    

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = playerVelocity;
        
        if (moveInput.x != 0 && state != MovementState.isClimbing)
        {
            state = MovementState.isRunning;
        }else if (moveInput.x == 0)
        {
            state = MovementState.idle;
        }

    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(_rigidbody2D.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(_rigidbody2D.velocity.x) , 1f);
        }
    }
    
    void ClimbLadder()
    {
        if (!_myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing Layer")))
        {
            _rigidbody2D.gravityScale = gravityScaleAtStart;
            /*state = MovementState.idle;*/
            return;
        }
        
        Vector2 climbVelocity = new Vector2(_rigidbody2D.velocity.x , moveInput.y * climbSpeed);
        _rigidbody2D.velocity = climbVelocity;
        _rigidbody2D.gravityScale = 0;
        
        state = MovementState.isClimbing;
    }

    void PlayerDeath()
    {
        if (_bodyCollider2D.IsTouchingLayers(LayerMask.GetMask( "Enemies","Hazards")))
        {
            isAlive = false;
            _animator.SetTrigger("isDead");
            _spriteRenderer.color = new Color(255, 0, 0, 255);

            StartCoroutine(WaitToShowDeathAnim());;
            
        }
    }
    
    IEnumerator WaitToShowDeathAnim()
    {
        yield return new WaitForSecondsRealtime(0.9f);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
        
    }

}
