using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D enemyRigidbody2D;
    BoxCollider2D boxCollider2D;
    [SerializeField] float moveSpeed = 1f;

    void Start()
    {
        enemyRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        enemyRigidbody2D.velocity = new Vector2(moveSpeed, 0f);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // change direction of movement
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRigidbody2D.velocity.x)), 1f);
    }
}
