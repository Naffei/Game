using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEditor.EditorTools;
using UnityEngine;

public class Walk : MonoBehaviour
{
    public GameObject roomPrefab;
    public GameObject spawnRoomPrefab;
/*    public GameObject slimePrefab;*/
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

    void GenerateLayout()
    {
        Vector2Int startingPosition = new Vector2Int(gridSizeX / 2, gridSizeY / 2);

        // Instantiate spawn room
        Vector2 spawnPos = new Vector2(startingPosition.x * cellWidth, startingPosition.y * cellHeight);
        visitedCells.Add(startingPosition);
        GameObject spawnRoom = Instantiate(spawnRoomPrefab, spawnPos, Quaternion.identity);
        spawnRoom.name = "SpawnRoom";
        roomObjects.Add(spawnRoom);

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
                newRoom.name = "Room" + roomsPlaced; // Assign room name
                roomObjects.Add(newRoom);

                // Call OpenDoors function
                OpenDoors(newRoom, nextPosition.x, nextPosition.y);

                /*                SpawnEnemies(newRoom);*/

                // Move to the next position
                lastRoomPosition = nextPosition;
                currentPosition = nextPosition;
                roomsPlaced++;

            }
            else
            {
                // If the next position is not valid, go a new direction
                nextDirection = GetRandomDirection();
            }
        }

        Vector2Int bossRoomPosition = GetValidFarthestPosition(lastRoomPosition);
        Vector2 bossRoomPos = new Vector2(bossRoomPosition.x * cellWidth, bossRoomPosition.y * cellHeight);
        GameObject bossRoom = Instantiate(bossRoomPrefab, bossRoomPos, Quaternion.identity);
        bossRoom.name = "BossRoom";
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

    Vector2Int GetValidFarthestPosition(Vector2Int spawnRoomPosition)
    {
        Vector2Int farthestPosition = Vector2Int.zero;
        float maxDistance = 0f;

        List<Vector2Int> validPositions = new List<Vector2Int>();

        // Check adjacent positions for validity
        Vector2Int leftPos = spawnRoomPosition + Vector2Int.left;
        if (IsCellValid(leftPos, visitedCells))
            validPositions.Add(leftPos);

        Vector2Int rightPos = spawnRoomPosition + Vector2Int.right;
        if (IsCellValid(rightPos, visitedCells))
            validPositions.Add(rightPos);

        Vector2Int upPos = spawnRoomPosition + Vector2Int.up;
        if (IsCellValid(upPos, visitedCells))
            validPositions.Add(upPos);

        Vector2Int downPos = spawnRoomPosition + Vector2Int.down;
        if (IsCellValid(downPos, visitedCells))
            validPositions.Add(downPos);

        // Find the position with the maximum distance from the spawn room
        foreach (Vector2Int position in validPositions)
        {
            float distance = Vector2Int.Distance(position, spawnRoomPosition);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestPosition = position;
            }
        }

        if (farthestPosition == Vector2Int.zero)
        {
            Debug.LogError("No valid adjacent position found for boss room. Returning original position.");
            return spawnRoomPosition;
        }
        else
        {
            return farthestPosition;
        }
    }

    bool IsCellValid(Vector2Int cell, List<Vector2Int> visited)
    {
        // Check if the cell is within bounds and not visited
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

        List<DoorInfo> doorPairs = new List<DoorInfo>();

        if (leftRoom != null)
        {
            // Open door to the left in the current room and to the right in the left room
            room.GetComponent<Room>().OpenDoor(Vector2.left);
            leftRoom.GetComponent<Room>().OpenDoor(Vector2.right);

            // Store door info information
            DoorInfo leftDoor = new DoorInfo("LeftDoor", Vector2.left, room.GetComponent<Room>().GetDoorPosition(Vector2.left));
            DoorInfo rightDoor = new DoorInfo("RightDoor", Vector2.right, leftRoom.GetComponent<Room>().GetDoorPosition(Vector2.right));
            doorPairs.Add(leftDoor);
            doorPairs.Add(rightDoor);
        }

        if (rightRoom != null)
        {
            // Open door to the right in the current room and to the left in the right room
            room.GetComponent<Room>().OpenDoor(Vector2.right);
            rightRoom.GetComponent<Room>().OpenDoor(Vector2.left);

            // Store door info information
            DoorInfo rightDoor = new DoorInfo("RightDoor", Vector2.right, room.GetComponent<Room>().GetDoorPosition(Vector2.right));
            DoorInfo leftDoor = new DoorInfo("LeftDoor", Vector2.left, rightRoom.GetComponent<Room>().GetDoorPosition(Vector2.left));
            doorPairs.Add(rightDoor);
            doorPairs.Add(leftDoor);
        }

        if (topRoom != null)
        {
            // Open door upwards in the current room and downwards in the top room
            room.GetComponent<Room>().OpenDoor(Vector2.up);
            topRoom.GetComponent<Room>().OpenDoor(Vector2.down);

            // Store door info information
            DoorInfo topDoor = new DoorInfo("TopDoor", Vector2.up, room.GetComponent<Room>().GetDoorPosition(Vector2.up));
            DoorInfo bottomDoor = new DoorInfo("BottomDoor", Vector2.down, topRoom.GetComponent<Room>().GetDoorPosition(Vector2.down));
            doorPairs.Add(topDoor);
            doorPairs.Add(bottomDoor);
        }

        if (downRoom != null)
        {
            // Open door downwards in the current room and upwards in the bottom room
            room.GetComponent<Room>().OpenDoor(Vector2.down);
            downRoom.GetComponent<Room>().OpenDoor(Vector2.up);

            // Store door info information
            DoorInfo bottomDoor = new DoorInfo("BottomDoor", Vector2.down, room.GetComponent<Room>().GetDoorPosition(Vector2.down));
            DoorInfo topDoor = new DoorInfo("TopDoor", Vector2.up, downRoom.GetComponent<Room>().GetDoorPosition(Vector2.up));
            doorPairs.Add(bottomDoor);
            doorPairs.Add(topDoor);
        }

        foreach (var doorPair in doorPairs)
        {
            Debug.Log("Door Name: " + doorPair.name + ", Direction: " + doorPair.direction + ", Position: " + doorPair.position);
        }
    }

    public class DoorInfo
    {
        public string name;
        public Vector2 direction;
        public Vector3 position;

        public DoorInfo(string doorName, Vector2 dir, Vector3 pos)
        {
            name = doorName;
            direction = dir;
            position = pos;
        }
    }

    public GameObject GetRoomObjectAt(Vector2Int index)
    {
        // Find the room object at the given index
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

/*    void SpawnEnemies(GameObject room)
    {
        // Get the room number from its name
        string roomName = room.name;
        int roomNumber = int.Parse(roomName.Substring(4)); // Assuming the room name format is "RoomX"

        // Check room number and spawn enemies accordingly
        if (roomNumber >= 0 && roomNumber <= 2)
        {
            // Spawn slimes for rooms 0 to 2
            SpawnSlimes(room);
        }
    }

    void SpawnSlimes(GameObject room)
    {
        // Spawn slimes in the room
        // Example code to spawn slimes
        for (int i = 0; i < numberOfSlimes; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPositionInRoom(room);
            Instantiate(slimePrefab, spawnPosition, Quaternion.identity, room.transform);
        }
    }

    Vector3 GetRandomSpawnPositionInRoom(GameObject room)
    {
        Renderer roomRenderer = room.GetComponent<Renderer>();
        if (roomRenderer != null)
        {
            // Calculate a random position within the room's bounds
            Bounds bounds = roomRenderer.bounds;
            float randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
            float randomY = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
            return new Vector3(randomX, randomY, 0);
        }
        else
        {
            // Fallback spawn position (e.g., center of the room)
            return room.transform.position;
        }
    }*/

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
    }


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
            // Open left door in current room and right in adjacent
            room.GetComponent<Room>().OpenDoor(Vector2.left);
            leftRoom.GetComponent<Room>().OpenDoor(Vector2.right);

            // Add left door position to the dictionary
            doorPositions.Add(Vector2.left, leftRoom.GetComponent<Room>().GetDoorPosition(Vector2.right));
        }

        if (rightRoom != null)
        {
            // Open right door in current room and left in adjacent
            room.GetComponent<Room>().OpenDoor(Vector2.right);
            rightRoom.GetComponent<Room>().OpenDoor(Vector2.left);

            // Add right door position to the dictionary
            doorPositions.Add(Vector2.right, rightRoom.GetComponent<Room>().GetDoorPosition(Vector2.left));
        }

        if (topRoom != null)
        {
            // Open top door in current room and bottom door in adjancet
            room.GetComponent<Room>().OpenDoor(Vector2.up);
            topRoom.GetComponent<Room>().OpenDoor(Vector2.down);

            // Add top door position to the dictionary
            doorPositions.Add(Vector2.up, topRoom.GetComponent<Room>().GetDoorPosition(Vector2.down));
        }

        if (downRoom != null)
        {
            // Open bottom door in current room and top door in adjancet
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
 
 
 */