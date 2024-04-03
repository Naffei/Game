using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject roomPrefab;
    private int maxRooms = 7;
    private int minRooms = 5;

    int cellWidth = 15;
    int cellHeight = 13;
    int gridSizeX = 7;
    int gridSizeY = 7;

    private List<GameObject> roomList = new List<GameObject>();
    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();

    private int[,] gridList;

    private int roomCount;

    private bool generationComplete = false;

    private void Start()
    {
        gridList = new int[gridSizeX, gridSizeY];
        roomQueue = new Queue<Vector2Int>();

        Vector2Int initialRoomindex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGeneration(initialRoomindex);

    }

    private void Update()
    {
        if(roomQueue.Count > 0 && roomCount < maxRooms && !generationComplete)
        {
            Vector2Int roomIndex = roomQueue.Dequeue();
            int gridX = roomIndex.x;
            int gridY = roomIndex.y;

            TryGenerateRoom(new Vector2Int(gridX - 1, gridY));
            TryGenerateRoom(new Vector2Int(gridX + 1, gridY));
            TryGenerateRoom(new Vector2Int(gridX, gridY - 1));
            TryGenerateRoom(new Vector2Int(gridX, gridY + 1));
        } else if(!generationComplete)
        {
            Debug.Log($"Generation Done, {roomCount} rooms made");
            generationComplete = true;
        }
    }

    private void StartRoomGeneration(Vector2Int roomIndex)
    {
        roomQueue.Enqueue(roomIndex);
        int x = roomIndex.x;
        int y = roomIndex.y;
        gridList[x, y] = 1;
        roomCount++;
        var initialRoom = Instantiate(roomPrefab, GetPositionFromGrid(roomIndex), Quaternion.identity);
        initialRoom.name = $"Room-{roomCount}";
        initialRoom.GetComponent<Room>().RoomIndex = roomIndex;
        roomList.Add(initialRoom);

    }

    private bool TryGenerateRoom(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y =roomIndex.y;

        if (roomCount >= maxRooms)
        {
            return false;
        }
        if (Random.value < 0.5f && roomIndex != Vector2Int.zero)
        {
            return false;
        }

        if(AdjacentRoomsCheck(roomIndex) > 1)
        {
            return false;
        }



        roomQueue.Enqueue(roomIndex);
        gridList[x,y] = 1;
        roomCount++;

        var newRoom = Instantiate(roomPrefab, GetPositionFromGrid(roomIndex), Quaternion.identity);
        newRoom.GetComponent<Room>().RoomIndex = roomIndex; 
        newRoom.name = $"Room-{roomCount}";
        roomList.Add(newRoom);

        OpenDoors(newRoom, x, y);

        return true;
    }

    void OpenDoors(GameObject room, int x, int y)
    {
        Room newRoomScript = room.GetComponent<Room>();

        Room leftRoomScript = GetRoomScriptAt(new Vector2Int(x - 1, y));
        Room rightRoomScript = GetRoomScriptAt(new Vector2Int(x + 1, y));
        Room topRoomScript = GetRoomScriptAt(new Vector2Int(x, y + 1));
        Room downRoomScript = GetRoomScriptAt(new Vector2Int(x, y - 1));

        if(x > 0 && gridList[x - 1, y] != 0)
        {
            newRoomScript.OpenDoor(Vector2Int.left);
            leftRoomScript.OpenDoor(Vector2Int.right);
        }

        if (x < gridSizeX && gridList[x + 1, y] != 0)
        {
            newRoomScript.OpenDoor(Vector2.right);
            rightRoomScript.OpenDoor(Vector2.left);
        }

        if (y > 0 && gridList[x, y - 1] != -0)
        {
            newRoomScript.OpenDoor(Vector2.down);
            topRoomScript.OpenDoor(Vector2.up);
        }

        if (y < gridSizeY && gridList[x, y + 1] != -0)
        {
            newRoomScript.OpenDoor(Vector2.up);
            downRoomScript.OpenDoor(Vector2.down);
        }

    }
    Room GetRoomScriptAt(Vector2Int index)
    {
        GameObject roomObject = roomList.Find(r => r.GetComponent<Room>().RoomIndex == index);
        if (roomObject != null)
            return roomObject.GetComponent<Room>();
        return null;
    }

    private int AdjacentRoomsCheck(Vector2Int roomIndex)
    {
        int x = roomIndex.x; 
        int y = roomIndex.y;
        int count = 0;

        if (x > 0 && gridList[x - 1, y] != 0) count++;
        if (x < 0 && gridList[x + 1, y] != 0) count++;
        if (y > 0 && gridList[x, y - 1] != 0) count++;
        if (y < 0 && gridList[x, y + 1] != 0) count++;

        return count;
    }

    private Vector3 GetPositionFromGrid(Vector2Int gridIndex)
    {
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;
        return new Vector3(cellWidth * (gridX - gridSizeX / 2),
            cellHeight * (gridY - gridSizeY / 2));
    }
}
