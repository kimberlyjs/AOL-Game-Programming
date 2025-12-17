using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform player;       // Drag your Fairy/Player object here

    [Header("Settings")]
    public float smoothSpeed = 5f; // Higher = tighter lock, Lower = more lazy/smooth
    public Vector3 offset = new Vector3(0, 0, -10); // Keeps camera backed away (Z axis)

    [Header("Zoom")]
    [Range(1, 20)] 
    public float cameraSize = 2f;  // 5 is default. Smaller = Zoom IN, Larger = Zoom OUT
    
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        // Apply the initial size
        cam.orthographicSize = cameraSize;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // 1. Handle Movement (Smoothly follow the player)
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // 2. Handle Zoom (Updates constantly so you can tweak it while playing)
        cam.orthographicSize = cameraSize;
    }
}
