using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    // Reference to the paired door
    private DoorController pairedDoor;

    // Method to pair this door with another door
    public void PairWith(DoorController otherDoor)
    {
        pairedDoor = otherDoor;
    }

    // Method to handle room transition when the player interacts with the door
    public void Interact()
    {
        // Check if the paired door exists
        if (pairedDoor != null)
        {
            // Perform room transition or any other action
            // For example, you might deactivate the current room and activate the next room
            // This depends on how your game is structured
        }
    }
}