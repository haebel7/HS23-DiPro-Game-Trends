using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public int dungeonSize;
    [HideInInspector]
    public int roomsCount;
    public List<GameObject> northRooms;
    public List<GameObject> eastRooms;
    public List<GameObject> southRooms;
    public List<GameObject> westRooms;
    public GameObject[] bossRooms;
    public List<GameObject> rooms;
    public GameObject boss;

    private void Start()
    {
        roomsCount = dungeonSize - 1;
    }
}
