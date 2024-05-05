using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
    }

    // Spawn player on spawn point
    public void SpawnPlayer()
    {
        // Check if the player prefab and spawn point are assigned
        if (playerPrefab != null && spawnPoint != null)
        {
            // Instantiate the player prefab at the spawn point's position
            GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            player.name = "Player";
            player.transform.parent = transform;
        }
        else
        {
            Debug.LogError("Player prefab or spawn point not assigned in the PlayerSpawner.");
        }
    }
}
