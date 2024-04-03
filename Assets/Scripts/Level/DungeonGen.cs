using System.Collections;
using System.Collections.Generic;
<<<<<<< Updated upstream:Assets/Scripts/DungeonGen.cs
using System.IO;
using System.Linq;
using TMPro.EditorUtilities;
=======
>>>>>>> Stashed changes:Assets/Scripts/Level/DungeonGen.cs
using UnityEngine;

public class DungeonGen : MonoBehaviour
{
<<<<<<< Updated upstream:Assets/Scripts/DungeonGen.cs
    public string roomFolder = "Environment/Rooms";
    public int gridX = 100;
    public int gridY = 100;

=======

    public string roomFolder = "Environment/Rooms";
    public int gridX = 5;
    public int gridY = 5;
    public float cellSize = 20f;

    public int minRooms = 5;
    public int maxRooms = 8;

    int roomsCreated = 0;

    private List<Vector2Int> occupiedCells = new List<Vector2Int>();

>>>>>>> Stashed changes:Assets/Scripts/Level/DungeonGen.cs
    void Start()
    {
        Debug.Log("Starting Generation");
        GenerateLayout();
    }

    void GenerateLayout()
    {
<<<<<<< Updated upstream:Assets/Scripts/DungeonGen.cs
        // Get all room prefabs
        GameObject[] roomPrefabs = Resources.LoadAll<GameObject>(roomFolder);

        int minRooms = 4;
        int maxRooms = 7;

=======
>>>>>>> Stashed changes:Assets/Scripts/Level/DungeonGen.cs
        int numOfRoomsToGet = Random.Range(minRooms, maxRooms + 1);
        GameObject[] roomPrefabs = Resources.LoadAll<GameObject>(roomFolder);

        Debug.Log("Number of room prefabs found: " + roomPrefabs.Length);

        //Create list to hold selected rooms
        List<GameObject> chosenRooms = new List<GameObject>();

        //Randomly Select Rooms
        for(int i = 0; i < numOfRoomsToGet; i++)
        {
            int randIndex = Random.Range(0, roomPrefabs.Length);

            chosenRooms.Add(roomPrefabs[randIndex]);
        }

        Debug.Log("Selected rooms:");
        foreach (GameObject room in chosenRooms)
        {
            Debug.Log(room.name);
        }

        // Generate rooms randomly
        int roomsCreated = 0;
        for (int i = 0; i < numOfRoomsToGet; i++)
        {
<<<<<<< Updated upstream:Assets/Scripts/DungeonGen.cs
            // Generate random coordinates within the grid bounds
            int randomX = Random.Range(0, gridX);
            int randomY = Random.Range(0, gridY);

            if(roomsCreated >= maxRooms)
            {
                Debug.Log("Maximum number of rooms reached.");
                return;
            }

                // Get the next room 
                int randomRoomIndex = Random.Range(0, chosenRooms.Count);
                GameObject roomPrefab = chosenRooms[randomRoomIndex];

                Debug.Log(roomPrefab);

                // Instantiate the room prefab
                Vector2 position = new Vector2(randomX, randomY);
                Instantiate(roomPrefab, position, Quaternion.identity);

                roomsCreated++;
            
        }
=======
            //Select random coordiante within grid
            int randomX = Random.Range(0, gridX);
            int randomY = Random.Range(0, gridY);

            //Check if cell is empty
            if(!IsCellOccupied(randomX, randomY))
            {
                int randomRoomIndex = Random.Range(0, chosenRooms.Count);
                GameObject roomPrefab = chosenRooms[randomRoomIndex];

                Vector2 position = new Vector2(startX + randomX * cellSize, startY + randomY * cellSize);
                Instantiate(roomPrefab, position, Quaternion.identity);

                cellOccupiedT(randomX, randomY);

                roomsCreated++;
            }
        }

       bool IsCellOccupied(int x, int y)
        {
            return occupiedCells.Contains(new Vector2Int(x, y));
        }

        void cellOccupiedT(int x, int y)
        {
            occupiedCells.Add(new Vector2Int(x, y));
        }

>>>>>>> Stashed changes:Assets/Scripts/Level/DungeonGen.cs
    }
}
