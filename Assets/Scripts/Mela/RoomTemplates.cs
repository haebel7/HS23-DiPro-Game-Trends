using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public int dungeonSize;
    [HideInInspector]
    public int roomsCount;

    public GameObject[] northRooms;
    public GameObject[] southRooms;
    public GameObject[] westRooms;
    public GameObject[] eastRooms;
    public GameObject closedRoom;
    public GameObject[] bossRooms;

    public List<GameObject> spawnPoints;
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
