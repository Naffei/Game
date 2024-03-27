using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkenWalk : MonoBehaviour
{
    public string roomFolder = "Environment/Rooms";
    public int dungeonSize = 7;
    public int minRooms = 7;
    public int maxRooms = 9;
    public float cellWidth = 13f;
    public float cellHeight = 11f;

    private List<Vector2Int> visitedCells = new List<Vector2Int>();

    void Start()
    {
        GenerateLayout();
    }

    void GenerateLayout()
    {
        // Get all room prefabs
        GameObject[] roomPrefabs = Resources.LoadAll<GameObject>(roomFolder);

        int numOfRoomsToGet = Random.Range(minRooms, maxRooms + 1); // Generate a random number of rooms

        Debug.Log("Number of room prefabs found: " + roomPrefabs.Length);

        // Create list to hold selected rooms
        List<GameObject> chosenRooms = new List<GameObject>();

        // Randomly Select Rooms
        for (int i = 0; i < numOfRoomsToGet; i++)
        {
            int randIndex = Random.Range(0, roomPrefabs.Length);
            chosenRooms.Add(roomPrefabs[randIndex]);
        }

        // Log selected rooms
        Debug.Log("Selected rooms:");
        foreach (GameObject room in chosenRooms)
        {
            Debug.Log(room.name);
        }

        // Perform drunken walk to generate dungeon layout
        Vector2Int currentPosition = Vector2Int.zero;
        visitedCells.Add(currentPosition);

        int roomsPlaced = 1; // Start with one room already placed
        while (roomsPlaced < numOfRoomsToGet)
        {
            // Randomly select the next direction
            Vector2Int nextDirection = GetRandomDirection();
            Vector2Int nextPosition = currentPosition + nextDirection;

            // Check if the next position is within bounds and unvisited
            if (IsCellValid(nextPosition, visitedCells))
            {
                // Mark the cell as visited
                visitedCells.Add(nextPosition);

                // Instantiate a room at the current position
                GameObject roomPrefab = chosenRooms[Random.Range(0, chosenRooms.Count)];
                Vector2 position = new Vector2(nextPosition.x * cellWidth, nextPosition.y * cellHeight);
                Instantiate(roomPrefab, position, Quaternion.identity);

                roomsPlaced++;

                // Move to the next position
                currentPosition = nextPosition;
            }
        }
    }

    Vector2Int GetRandomDirection()
    {
        // Generate a random direction (x,y)
        int randomDirection = Random.Range(0, 4);
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
        // Check if the cell is within bounds and unvisited
        return cell.x >= 0 && cell.x < dungeonSize && cell.y >= 0 && cell.y < dungeonSize && !visited.Contains(cell);
    }
}