using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public float smoothSpeed = 0.125f;

    void FixedUpdate()
    {
        if (playerTransform != null)
        {
            // Get the player's position
            Vector3 desiredPosition = playerTransform.position;
            desiredPosition.z = transform.position.z;

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}