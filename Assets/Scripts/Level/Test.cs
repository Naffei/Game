using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    // Public variables
    public int roomWidth = 9;
    public int roomHeight = 11;
    public TileBase wallPrefab;
    public TileBase floorPrefab;
    public TileBase doorPrefab;

    // Private variables
    private List<Vector2Int> visitedCells = new List<Vector2Int>();

    void Start()
    {
        GenerateRoom();
    }

    void GenerateRoom()
    {
        Vector2Int startPosition = new Vector2Int(roomWidth / 2, roomHeight / 2); // Start in the center
        visitedCells.Add(startPosition);

        Vector2Int currentPosition = startPosition;

        // Perform drunken walk until all cells are visited
        while (visitedCells.Count < roomWidth * roomHeight)
        {
            // Get a random direction
            Vector2Int direction = GetRandomDirection();
            Vector2Int newPosition = currentPosition + direction;

            // Check if the new position is within bounds
            if (IsWithinBounds(newPosition))
            {
                // Mark the cell as visited
                visitedCells.Add(newPosition);

                // Move to the new position
                currentPosition = newPosition;
            }
        }

        // Generate walls and floors based on the visited cells
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                Vector2Int cellPosition = new Vector2Int(x, y);

                // Check if the cell was visited
                if (visitedCells.Contains(cellPosition))
                {
                    // Create floor
                    Instantiate(floorPrefab, new Vector3(x, y, 0), Quaternion.identity);

                    // Check for neighboring cells
                    foreach (Vector2Int neighbor in GetNeighbors(cellPosition))
                    {
                        // Create door if the neighboring cell was visited
                        if (visitedCells.Contains(neighbor))
                        {
                            // Create door
                            Instantiate(doorPrefab, new Vector3(x, y, 0), Quaternion.identity);
                        }
                    }
                }
                else
                {
                    // Create wall
                    Instantiate(wallPrefab, new Vector3(x, y, 0), Quaternion.identity);
                }
            }
        }
    }

    Vector2Int GetRandomDirection()
    {
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

    bool IsWithinBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < roomWidth && position.y >= 0 && position.y < roomHeight;
    }

    List<Vector2Int> GetNeighbors(Vector2Int position)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        neighbors.Add(position + Vector2Int.up);
        neighbors.Add(position + Vector2Int.down);
        neighbors.Add(position + Vector2Int.left);
        neighbors.Add(position + Vector2Int.right);
        return neighbors;
    }
}