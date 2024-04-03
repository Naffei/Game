using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinder : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        // Find the player clone GameObject by tag
        GameObject playerClone = GameObject.FindWithTag("PlayerClone");

        // If the player clone is found, assign it to the Virtual Camera
        if (playerClone != null && virtualCamera != null)
        {
            virtualCamera.Follow = playerClone.transform;
            virtualCamera.LookAt = playerClone.transform;
        }
        else
        {
            Debug.LogWarning("Player clone not found or Virtual Camera not assigned.");
        }
    }
}
