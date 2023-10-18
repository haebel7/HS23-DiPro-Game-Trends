using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public int dungeonSize;
    [HideInInspector]
    public int roomsCount;

    public List<GameObject> northRooms;
    public List<GameObject> southRooms;
    public List<GameObject> westRooms;
    public List<GameObject> eastRooms;
    public GameObject closedRoom;
    public GameObject[] bossRooms;
    public List<GameObject> rooms;

    private bool spawnedBoss = false;
    public GameObject boss;

    private void Start()
    {
        roomsCount = dungeonSize - 1;
    }

    private void Update()
    {
        if (roomsCount == 0 && spawnedBoss == false)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == rooms.Count - 1)
                {
                    Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
                    spawnedBoss = true;
                }
            }
        }
    }
}
