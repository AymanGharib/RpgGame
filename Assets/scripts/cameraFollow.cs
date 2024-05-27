using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerafollow : MonoBehaviour
{
    private Transform player;
    private float minX = -20;
    private float maxX = 30;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void LateUpdate()
    {
        if (player == null) { return; }

        // Get the current camera position
        Vector3 cameraPosition = transform.position;

        // Calculate the target position of the camera based on the player's position
        float targetX = Mathf.Clamp(player.position.x, minX, maxX);
        Vector3 targetPosition = new Vector3(targetX, cameraPosition.y, cameraPosition.z);

        // Move the camera to the target position
        transform.position = targetPosition;
    }
}
