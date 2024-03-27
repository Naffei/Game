using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    public int numDoors;
    public Vector2Int[] doorDirections;

    public RoomInfo(int numDoors, Vector2Int[] doorDirections)
    {
        this.numDoors = numDoors;
        this.doorDirections = doorDirections;
    }
}


