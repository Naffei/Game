using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public GameObject TopDoor;
    public GameObject BottomDoor;
    public GameObject LeftDoor;
    public GameObject RightDoor;

    public GameObject northWall;
    public GameObject southWall;
    public GameObject eastWall;
    public GameObject westWall;

    public List<GameObject> items;
    private List<GameObject> enemies = new List<GameObject>();

    public Vector2Int RoomIndex { get; set; }


    public bool IsPlayerInRoom(Transform player)
    {
        // Check if player is not within the bounds of any wall
        return !IsPlayerInWall(player, northWall) &&
               !IsPlayerInWall(player, southWall) &&
               !IsPlayerInWall(player, eastWall) &&
               !IsPlayerInWall(player, westWall);
    }

    private bool IsPlayerInWall(Transform player, GameObject wall)
    {
        if (wall == null) return false;

        // Get the collider attached to the wall
        Collider2D collider = wall.GetComponent<Collider2D>();

        // Check if player's position is within the bounds of the collider
        return collider != null && collider.bounds.Contains(player.position);
    }

    public void OpenDoor(Vector2 direction)
    {
        if (direction == Vector2.up)
        {
            TopDoor.SetActive(true);
        }
        if (direction == Vector2.down)
        {
            BottomDoor.SetActive(true);
        }
        if (direction == Vector2.left)
        {
            LeftDoor.SetActive(true);
        }
        if (direction == Vector2.right)
        {
            RightDoor.SetActive(true);
        }
    }

    public Vector3 GetDoorPosition(Vector2 direction)
    {
        Vector3 doorPosition = Vector3.zero;

        if (Mathf.Approximately(direction.x, 0) && direction.y > 0)
        {
            if (TopDoor.activeSelf)
            {
                doorPosition = TopDoor.transform.position;
                doorPosition.y += TopDoor.GetComponent<Collider2D>().bounds.extents.y;
            }
        }
        else if (Mathf.Approximately(direction.x, 0) && direction.y < 0)
        {
            if (BottomDoor.activeSelf)
            {
                doorPosition = BottomDoor.transform.position;
                doorPosition.y -= BottomDoor.GetComponent<Collider2D>().bounds.extents.y;
            }
        }
        else if (direction.x < 0 && Mathf.Approximately(direction.y, 0))
        {
            if (LeftDoor.activeSelf)
            {
                doorPosition = LeftDoor.transform.position;
                doorPosition.x -= LeftDoor.GetComponent<Collider2D>().bounds.extents.x;
            }
        }
        else if (direction.x > 0 && Mathf.Approximately(direction.y, 0))
        {
            if (RightDoor.activeSelf)
            {
                doorPosition = RightDoor.transform.position;
                doorPosition.x += RightDoor.GetComponent<Collider2D>().bounds.extents.x;
            }
        }

        return doorPosition;
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            SpawnItem();
        }
    }


    private void SpawnItem()
    {
        Debug.Log("Spawning Item");

        int itemCount = Random.Range(1, 3);

        for (int i = 0; i < itemCount; i++)
        {
            GameObject spawnItem = items[Random.Range(0, items.Count)];

            Transform spawnArea = transform.Find("SpawnArea");
            if (spawnArea != null)
            {
                // Get the boundaries of the spawn area
                Bounds bounds = spawnArea.GetComponent<Collider2D>().bounds;

                // Get a random point within the spawn area
                float randomX = Random.Range(bounds.min.x, bounds.max.x);
                float randomY = Random.Range(bounds.min.y, bounds.max.y);

                // Return the spawn position
                Vector3 spawnPosition = new Vector3(randomX, randomY, 0);

                Instantiate(spawnItem, spawnPosition, Quaternion.identity);
                Debug.Log("Item spawned: " + spawnItem.name);
            }
            else
            {
                Debug.LogError("SpawnArea not found in the room: " + name);
            }
        }
    }
}


    /*   
                public Vector3 GetDoorPosition(Vector2 direction)
    {
        if (Mathf.Approximately(direction.x, 0) && direction.y > 0)
        {
            return TopDoor.activeSelf ? TopDoor.transform.position : Vector3.zero;
        }
        else if (Mathf.Approximately(direction.x, 0) && direction.y < 0)
        {
            return BottomDoor.activeSelf ? BottomDoor.transform.position : Vector3.zero;
        }
        else if (direction.x < 0 && Mathf.Approximately(direction.y, 0))
        {
            return LeftDoor.activeSelf ? LeftDoor.transform.position : Vector3.zero;
        }
        else if (direction.x > 0 && Mathf.Approximately(direction.y, 0))
        {
            return RightDoor.activeSelf ? RightDoor.transform.position : Vector3.zero;
        }
        else
        {
            return Vector3.zero;
        }
    }
        }*/