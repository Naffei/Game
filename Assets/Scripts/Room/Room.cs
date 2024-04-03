using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject TopDoor;
    public GameObject BottomDoor;
    public GameObject LeftDoor;
    public GameObject RightDoor;

    public Vector2Int RoomIndex { get; set; }

    public Vector3 GetDoorPosition(Vector2 direction)
    {
        if (direction == Vector2.up)
        {
            return TopDoor.transform.position;
        }
        if (direction == Vector2.down)
        {
            return BottomDoor.transform.position;
        }
        if (direction == Vector2.left)
        {
            return LeftDoor.transform.position;
        }
        if (direction == Vector2.right)
        {
            return RightDoor.transform.position;
        }
        return Vector3.zero;
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
}
