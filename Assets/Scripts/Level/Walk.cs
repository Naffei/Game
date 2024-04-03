using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.EditorTools;
using UnityEngine;

public class Walk : MonoBehaviour
{
    public GameObject roomPrefab;
    public GameObject spawnRoomPrefab;
    public int gridSizeX = 7;
    public int gridSizeY = 7;
    public int minRooms = 4;
    public int maxRooms = 7;
    public float cellWidth = 11f;
    public float cellHeight = 9f;

    private List<Vector2Int> visitedCells = new List<Vector2Int>();
    private List<GameObject> roomObjects = new List<GameObject>();

    private Transform playerTransform;

    public Dictionary<Vector2, Vector3> doorPositions = new Dictionary<Vector2, Vector3>();

    void Start()
    {
        GenerateLayout();
    }

    void GenerateLayout()
    {
        // Instantiate spawn room
        Vector2 spawnPos = new Vector2(0, 0);
        Vector2Int currentPosition = Vector2Int.zero;
        visitedCells.Add(currentPosition);
        GameObject spawnRoom = Instantiate(spawnRoomPrefab, spawnPos, Quaternion.identity);
        roomObjects.Add(spawnRoom);
        OpenDoors(spawnRoom, currentPosition.x, currentPosition.y);

        // Generate additional rooms
        int numOfRoomsToGet = UnityEngine.Random.Range(minRooms, maxRooms + 1);
        int roomsPlaced = 1; // Start with 1 because spawn already instantiated

        while (roomsPlaced < numOfRoomsToGet)
        {
            // Randomly choose what direction
            Vector2Int nextDirection = GetRandomDirection();
            Vector2Int nextPosition = currentPosition + nextDirection;

            // Check if the next position is within bounds and unvisited
            if (IsCellValid(nextPosition, visitedCells))
            {
                // Mark the cell as visited
                visitedCells.Add(nextPosition);

                // Instantiate a room at the current position
                Vector2 position = new Vector2(nextPosition.x * cellWidth, nextPosition.y * cellHeight);
                GameObject newRoom = Instantiate(roomPrefab, position, Quaternion.identity);
                roomObjects.Add(newRoom);

                // Open doors based on adjacent rooms
                OpenDoors(newRoom, nextPosition.x, nextPosition.y);

                // Move to the next position
                currentPosition = nextPosition;
                roomsPlaced++;
            }
            else
            {
                // If the next position is not valid, go a new direction
                nextDirection = GetRandomDirection();
            }
        }
    }

    Vector2Int GetRandomDirection()
    {
        // Pick a random direction (x,y)
        int randomDirection = UnityEngine.Random.Range(0, 4);
        switch (randomDirection)
        {
            case 0: return Vector2Int.up;
            case 1: return Vector2Int.down;
            case 2: return Vector2Int.left;
            case 3: return Vector2Int.right;
            default: return Vector2Int.zero;
        }
    }

    bool IsCellValid(Vector2Int cell, List<Vector2Int> visited)
    {
        // Check if the cell is within bounds and not visited
        return cell.x >= 0 && cell.x < gridSizeX && cell.y >= 0 && cell.y < gridSizeY && !visited.Contains(cell);
    }

    /*    void OpenDoors(GameObject room, int x, int y)
        {
            // Check for adjacent rooms
            GameObject leftRoom = GetRoomObjectAt(new Vector2Int(x - 1, y));
            GameObject rightRoom = GetRoomObjectAt(new Vector2Int(x + 1, y));
            GameObject topRoom = GetRoomObjectAt(new Vector2Int(x, y + 1));
            GameObject downRoom = GetRoomObjectAt(new Vector2Int(x, y - 1));

            if (leftRoom != null)
            {
                // Open door to the left in the current room and to the right in the left room
                room.GetComponent<Room>().OpenDoor(Vector2.left);
                leftRoom.GetComponent<Room>().OpenDoor(Vector2.right);
            }

            if (rightRoom != null)
            {
                // Open door to the right in the current room and to the left in the right room
                room.GetComponent<Room>().OpenDoor(Vector2.right);
                rightRoom.GetComponent<Room>().OpenDoor(Vector2.left);
            }

            if (topRoom != null)
            {
                // Open door upwards in the current room and downwards in the top room
                room.GetComponent<Room>().OpenDoor(Vector2.up);
                topRoom.GetComponent<Room>().OpenDoor(Vector2.down);
            }

            if (downRoom != null)
            {
                // Open door downwards in the current room and upwards in the bottom room
                room.GetComponent<Room>().OpenDoor(Vector2.down);
                downRoom.GetComponent<Room>().OpenDoor(Vector2.up);
            }
        }*/

    Dictionary<Vector2, Vector3> OpenDoors(GameObject room, int x, int y)
    {
        Dictionary<Vector2, Vector3> doorPositions = new Dictionary<Vector2, Vector3>();

        // Check for adjacent rooms
        GameObject leftRoom = GetRoomObjectAt(new Vector2Int(x - 1, y));
        GameObject rightRoom = GetRoomObjectAt(new Vector2Int(x + 1, y));
        GameObject topRoom = GetRoomObjectAt(new Vector2Int(x, y + 1));
        GameObject downRoom = GetRoomObjectAt(new Vector2Int(x, y - 1));

        if (leftRoom != null)
        {
            // Open door to the left in the current room and to the right in the left room
            room.GetComponent<Room>().OpenDoor(Vector2.left);
            leftRoom.GetComponent<Room>().OpenDoor(Vector2.right);

            // Add left door position to the dictionary
            doorPositions.Add(Vector2.left, leftRoom.GetComponent<Room>().GetDoorPosition(Vector2.right));
        }

        if (rightRoom != null)
        {
            // Open door to the right in the current room and to the left in the right room
            room.GetComponent<Room>().OpenDoor(Vector2.right);
            rightRoom.GetComponent<Room>().OpenDoor(Vector2.left);

            // Add right door position to the dictionary
            doorPositions.Add(Vector2.right, rightRoom.GetComponent<Room>().GetDoorPosition(Vector2.left));
        }

        if (topRoom != null)
        {
            // Open door upwards in the current room and downwards in the top room
            room.GetComponent<Room>().OpenDoor(Vector2.up);
            topRoom.GetComponent<Room>().OpenDoor(Vector2.down);

            // Add top door position to the dictionary
            doorPositions.Add(Vector2.up, topRoom.GetComponent<Room>().GetDoorPosition(Vector2.down));
        }

        if (downRoom != null)
        {
            // Open door downwards in the current room and upwards in the bottom room
            room.GetComponent<Room>().OpenDoor(Vector2.down);
            downRoom.GetComponent<Room>().OpenDoor(Vector2.up);

            // Add bottom door position to the dictionary
            doorPositions.Add(Vector2.down, downRoom.GetComponent<Room>().GetDoorPosition(Vector2.up));
        }

        foreach (var doorPosition in doorPositions)
        {
            Debug.Log("Direction: " + doorPosition.Key + ", Position: " + doorPosition.Value);
        }

        return doorPositions;
    }

    GameObject GetRoomObjectAt(Vector2Int index)
    {
        // Find the room object at the given index
        return roomObjects.Find(r => r.transform.position == new Vector3(index.x * cellWidth, index.y * cellHeight, 0));
    }

    public void TeleportPlayerToDoor(Vector3 doorPosition, Vector2 doorDirection)
    {
        // Calculate the destination position based on the door position and direction
        Vector3 destinationPosition = doorPosition + (Vector3)doorDirection;

        // Teleport the player to the destination position
        playerTransform.position = destinationPosition;
    }
}