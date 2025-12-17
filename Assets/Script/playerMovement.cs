using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))] // Added this so we can flip the sprite
public class playerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Grab the sprite component

        // 1. Turn off gravity
        rb.gravityScale = 0f; 
        
        // 2. Lock Physics Rotation (Prevents spinning on collision)
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // 3. Force rotation to zero on start (Ensures she stands straight up)
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        // Get Input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Normalize vector
        moveInput = new Vector2(moveX, moveY).normalized;

        // --- REPLACED ROTATION WITH FLIPPING ---
        // This keeps the character upright (Static) but changes facing direction
        
        if (moveX < 0)
        {
            spriteRenderer.flipX = true; // Face Left
        }
        else if (moveX > 0)
        {
            spriteRenderer.flipX = false; // Face Right
        }
    }

    void FixedUpdate()
    {
        // Apply Movement
        rb.velocity = moveInput * moveSpeed;
    }
}