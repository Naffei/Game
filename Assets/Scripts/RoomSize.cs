using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSize : MonoBehaviour
{
    public int width = 1;
    public int height = 1;

    public Vector2Int GetRoomSize()
    {
        return new Vector2Int(width, height);
    }
}
