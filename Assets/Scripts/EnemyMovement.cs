using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float enemyMoveSpeed = 1;
    
    Rigidbody2D _enemyRigidbody2D;
    BoxCollider2D _enemyBoxCollider2D; 
    
    void Start()
    {
        _enemyRigidbody2D = GetComponent<Rigidbody2D>();
        _enemyBoxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        _enemyRigidbody2D.velocity = new Vector2(enemyMoveSpeed, 0f);
    }
    
    
    void OnTriggerExit2D(Collider2D other)
    {
        enemyMoveSpeed = -enemyMoveSpeed;
            FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(_enemyRigidbody2D.velocity.x)), 1f);
    }
}
