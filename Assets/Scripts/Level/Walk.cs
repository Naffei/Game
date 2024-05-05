using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
    public GameObject roomPrefab;
    public GameObject spawnRoomPrefab;
    public GameObject slimePrefab;
    public GameObject Player;
    public GameObject bossRoomPrefab;

    public int gridSizeX = 7;
    public int gridSizeY = 7;
    public int minRooms = 4;
    public int maxRooms = 7;
    public float cellWidth = 11f;
    public float cellHeight = 9f;

    private int numberOfSlimes = 3;

    private List<Vector2Int> visitedCells = new List<Vector2Int>();
    public List<GameObject> roomObjects = new List<GameObject>();

    void Start()
    {
        GenerateLayout();
    }

    // Create Level
    void GenerateLayout()
    {
        Vector2Int startingPosition = new Vector2Int(gridSizeX / 2, gridSizeY / 2);

        // Instantiate spawn room
        Vector2 spawnPos = new Vector2(startingPosition.x * cellWidth, startingPosition.y * cellHeight);
        visitedCells.Add(startingPosition);
        GameObject spawnRoom = Instantiate(spawnRoomPrefab, spawnPos, Quaternion.identity);
        spawnRoom.name = "SpawnRoom";
        roomObjects.Add(spawnRoom);

        // Move Player to spawn position
        Transform playerSpawnPoint = spawnRoom.transform.Find("PSpawnPoint");
        if (playerSpawnPoint != null)
        {
            Player.transform.position = playerSpawnPoint.position;
        }
        else
        {
            Debug.LogError("PSpawnPoint not found in SpawnRoom. Player cannot be teleported.");
        }

        Vector2Int currentPosition = startingPosition;
        Vector2Int lastRoomPosition = currentPosition;

        // Generate additional rooms
        int numOfRoomsToGet = UnityEngine.Random.Range(minRooms, maxRooms + 1);
        int roomsPlaced = 0;

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
                newRoom.name = "Room" + roomsPlaced;
                roomObjects.Add(newRoom);

                // Show doors to connecting rooms and spawn enemies in rooms
                OpenDoors(newRoom, nextPosition.x, nextPosition.y);
                SpawnEnemies(newRoom);

                // Move to the next position
                lastRoomPosition = nextPosition;
                currentPosition = nextPosition;
                roomsPlaced++;

            }
            else
            {
                // If the next position is not valid, go a new direction
               nextPosition = currentPosition + GetRandomDirection();
            }
        }

        // Spawn boss room as last room
        Vector2Int bossRoomPosition = (lastRoomPosition);
        Vector2 bossRoomPos = new Vector2(bossRoomPosition.x * cellWidth, bossRoomPosition.y * cellHeight);
        GameObject bossRoom = Instantiate(bossRoomPrefab, bossRoomPos, Quaternion.identity);
        bossRoom.name = "bossRoom";
        roomObjects.Add(bossRoom);
        OpenDoors(bossRoom, bossRoomPosition.x, bossRoomPosition.y);
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


    // Check if the cell is within bounds and not visited
    bool IsCellValid(Vector2Int cell, List<Vector2Int> visited)
    {

        return cell.x >= 0 && cell.x < gridSizeX && cell.y >= 0 && cell.y < gridSizeY && !visited.Contains(cell);
    }

    // Open doors based on adjacent rooms
    void OpenDoors(GameObject room, int x, int y)
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
    }

    // Find the adjacent room at the given index
    public GameObject GetRoomObjectAt(Vector2Int index)
    {
        foreach (var room in roomObjects)
        {
            Vector2Int roomIndex = new Vector2Int(Mathf.RoundToInt(room.transform.position.x / cellWidth),
                                                  Mathf.RoundToInt(room.transform.position.y / cellHeight));
            if (roomIndex == index)
            {
                return room;
            }
        }
        return null;
    }


    // Spawn enemies
    void SpawnEnemies(GameObject room)
    {
/*        // Get the room number from its name
        string roomName = room.name;

        // Have different types of enemies spawn in rooms
        int roomNumber = int.Parse(roomName.Substring(4));

        if (roomNumber >= 0 && roomNumber <= 3)
        {
            SpawnSlimes(room);
        }*/

        //Spawn slimes
        SpawnSlimes(room);

    }

    void SpawnSlimes(GameObject room)
    {
        // Spawn slimes in the room
        for (int i = 0; i < numberOfSlimes; i++)
        {
            // Get spawn area for slimes
            Transform spawnArea = room.transform.Find("SpawnArea");
            if (spawnArea != null)
            {
                // randomly place slime somewhere on spawn area
                Vector3 spawnPosition = GetRandomSpawnPositionInRoom(spawnArea);
                GameObject slimeObject = Instantiate(slimePrefab, spawnPosition, Quaternion.identity, room.transform);
                slimeObject.name = "Slime";

                Slime slimeScript = slimeObject.GetComponent<Slime>();
                if (slimeScript != null)
                {
                    slimeScript.roomBoundary = room;
                }
                else
                {
                    Debug.LogError("Slime script not found on Slime object. Slime will not work properly.");
                }
            }
            else
            {
                Debug.LogError("SpawnArea not found in the room: " + room.name);
            }
        }
    }


    // Calculate a random position within the spawn area's bounds
    Vector3 GetRandomSpawnPositionInRoom(Transform spawnArea)
    {
        if (spawnArea != null)
        {
            Collider2D spawnAreaCollider = spawnArea.GetComponent<Collider2D>();
            if (spawnAreaCollider != null)
            {
                Bounds bounds = spawnAreaCollider.bounds;
                float randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
                float randomY = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
                Debug.Log("SpawnArea size: " + bounds.size);
                return new Vector3(randomX, randomY, 0);
            }
        }
        return Vector3.zero;
    }


  }