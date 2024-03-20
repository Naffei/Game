using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;

public class DungeonGen : MonoBehaviour
{
    public string roomFolder = "Environment/Rooms";
    public int gridX = 100;
    public int gridY = 100;

    void Start()
    {
        Debug.Log("Starting Generation");
        GenerateLayout();
    }

    void GenerateLayout()
    {
        // Get all room prefabs
        GameObject[] roomPrefabs = Resources.LoadAll<GameObject>(roomFolder);

        int minRooms = 4;
        int maxRooms = 7;

        int numOfRoomsToGet = Random.Range(minRooms, maxRooms + 1);


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
    }
}
