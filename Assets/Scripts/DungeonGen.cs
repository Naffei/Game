using System.Collections;
using System.Collections.Generic;
<<<<<<< Updated upstream
=======
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro.EditorUtilities;
>>>>>>> Stashed changes
using UnityEngine;

public class DungeonGen : MonoBehaviour
{
<<<<<<< Updated upstream
    // Start is called before the first frame update
=======
    public string roomFolder = "Environment/Rooms";
    public int gridX = 4;
    public int gridY = 4;
    public float cellSize = 15f;

    public int minRooms = 7;
    public int maxRooms = 12;

    int roomsCreated = 0;

    private List<Vector2Int> occupiedCells = new List<Vector2Int>();

>>>>>>> Stashed changes
    void Start()
    {
        GenerateLayout();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateLayout()
    {

<<<<<<< Updated upstream
=======
        int numOfRoomsToGet = Random.Range(minRooms, maxRooms + 1);


        Debug.Log("Number of room prefabs found: " + roomPrefabs.Length);

        //Create list to hold selected rooms
        List<GameObject> chosenRooms = new List<GameObject>();

        //Randomly Select Rooms from room folder
        for(int i = 0; i < numOfRoomsToGet; i++)
        {
            int randIndex = Random.Range(0, roomPrefabs.Length);

            chosenRooms.Add(roomPrefabs[randIndex]);
        }

        //Log what rooms have been selected in Console
        Debug.Log("Selected rooms:");
        foreach (GameObject room in chosenRooms)
        {
            Debug.Log(room.name);
        }

        float startX = -(gridX * cellSize) / 2f + cellSize / 2f;
        float startY = -(gridY * cellSize) / 2f + cellSize / 2f;

        // Generate rooms randomly on grid
        for (int i = 0; i < numOfRoomsToGet; i++)
        {
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




/*        for (int i = 0; i < numOfRoomsToGet; i++)
        {
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
            
        }*/
>>>>>>> Stashed changes
    }

}
