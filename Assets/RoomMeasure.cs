using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMeasure : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject room;

    void Start()
    {
        CalculateRoomDimensions();
    }

    void CalculateRoomDimensions()
    {
        if (room == null)
        {
            Debug.LogWarning("Room GameObject reference is not set.");
            return;
        }

        // Get all colliders attached to the room GameObject
        Collider2D[] colliders = room.GetComponentsInChildren<Collider2D>();

        if (colliders.Length == 0)
        {
            Debug.LogWarning("No colliders found on the room GameObject.");
            return;
        }

        // Initialize min and max positions
        Vector3 minPosition = Vector3.positiveInfinity;
        Vector3 maxPosition = Vector3.negativeInfinity;

        // Iterate through all colliders to find the min and max positions
        foreach (Collider2D collider in colliders)
        {
            Vector3 colliderMin = collider.bounds.min;
            Vector3 colliderMax = collider.bounds.max;

            // Update min and max positions
            minPosition = Vector3.Min(minPosition, colliderMin);
            maxPosition = Vector3.Max(maxPosition, colliderMax);
        }

        // Calculate length and height of the room
        float length = maxPosition.x - minPosition.x;
        float height = maxPosition.y - minPosition.y;

        Debug.Log("Room length: " + length);
        Debug.Log("Room height: " + height);
    }
}