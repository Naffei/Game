using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnRoom : MonoBehaviour
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
        if (wall == null) return false; // Ensure the wall GameObject is assigned

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
}
