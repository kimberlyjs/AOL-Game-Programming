using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class NPCPatrol : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 2f;
    public float waitTime = 1f; // Optional: Wait a bit at each point
    
    [Header("Path")]
    public Transform[] waypoints; // Drag your empty game objects here

    private int currentPointIndex = 0;
    private float waitCounter = 0f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Safety Check: If no points are assigned, do nothing
        if (waypoints.Length == 0) return;

        // 1. Identify the Target
        Transform target = waypoints[currentPointIndex];

        // 2. Check Distance: Are we there yet?
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            // We arrived! Now wait a moment.
            waitCounter += Time.deltaTime;
            
            if (waitCounter >= waitTime)
            {
                // Finished waiting, pick next point
                waitCounter = 0f;
                currentPointIndex++;
                
                // If we reached the end of the list, loop back to the start (0)
                if (currentPointIndex >= waypoints.Length)
                {
                    currentPointIndex = 0;
                }
            }
        }
        else
        {
            // 3. Move towards the target
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // 4. Handle Facing (Flip Left/Right)
            Vector2 direction = target.position - transform.position;
            
            if (direction.x < 0) 
            {
                spriteRenderer.flipX = true; // Face Left
            }
            else if (direction.x > 0) 
            {
                spriteRenderer.flipX = false; // Face Right
            }
        }
    }
}
