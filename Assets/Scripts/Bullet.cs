using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 10f;
    PlayerMovementScript player;
    float xSpeed;

    Rigidbody2D _rigidbody2D;
    

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovementScript>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
        _rigidbody2D.velocity = new Vector2(xSpeed, 0);
        transform.localScale = _rigidbody2D.position.normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemies")
        {
            Destroy(other.gameObject);
        }
    }
    

    void FlipBulletFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(_rigidbody2D.velocity.x) * bulletSpeed), 1f);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
