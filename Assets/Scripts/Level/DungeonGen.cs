using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGen : MonoBehaviour
{

    public string roomFolder = "Environment/Rooms";
    public int gridX = 5;
    public int gridY = 5;
    public float cellSize = 20f;

    public int minRooms = 5;
    public int maxRooms = 8;

    int roomsCreated = 0;

    private List<Vector2Int> occupiedCells = new List<Vector2Int>();

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
        int numOfRoomsToGet = Random.Range(minRooms, maxRooms + 1);
        GameObject[] roomPrefabs = Resources.LoadAll<GameObject>(roomFolder);

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

    }

}
