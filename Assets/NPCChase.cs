using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class NPCChase : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Vision Settings")]
    public float detectionRange = 4f;
    public float escapeRange = 7f;
    public Color debugColor = Color.red;

    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 4.5f;

    [Header("Patrol Path")]
    public Transform[] waypoints;
    public float waitTime = 1f;

    // Internal Variables
    private int currentPointIndex = 0;
    private float waitCounter = 0f;
    private bool isChasing = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentPointIndex = 0;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (isChasing)
        {
            // --- CHASE MODE ---
            if (distanceToPlayer > escapeRange)
            {
                // Player Escaped!
                isChasing = false;
                
                // 1. Reset timer so we move instantly
                waitCounter = 0f;

                // 2. Just find the closest point and go there. 
                // This prevents "skipping" points which causes confusion.
                currentPointIndex = GetClosestWaypointIndex();
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            // --- PATROL MODE ---
            if (distanceToPlayer < detectionRange)
            {
                isChasing = true; 
            }
            else
            {
                Patrol();
            }
        }
    }

    void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, runSpeed * Time.deltaTime);
        HandleFacing(player.position);
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentPointIndex];
        
        // DEBUG: Draws a line to show where NPC is going
        Debug.DrawLine(transform.position, target.position, Color.green);

        transform.position = Vector2.MoveTowards(transform.position, target.position, walkSpeed * Time.deltaTime);
        HandleFacing(target.position);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            waitCounter += Time.deltaTime;
            if (waitCounter >= waitTime)
            {
                waitCounter = 0f;
                currentPointIndex++;
                if (currentPointIndex >= waypoints.Length)
                {
                    currentPointIndex = 0;
                }
            }
        }
    }

    int GetClosestWaypointIndex()
    {
        if (waypoints.Length == 0) return 0;

        float minDistance = Mathf.Infinity;
        int closestIndex = 0;

        for (int i = 0; i < waypoints.Length; i++)
        {
            float dist = Vector2.Distance(transform.position, waypoints[i].position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    void HandleFacing(Vector3 targetPosition)
    {
        // Simple flip check
        if (targetPosition.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else if (targetPosition.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = debugColor;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, escapeRange);
    }
}