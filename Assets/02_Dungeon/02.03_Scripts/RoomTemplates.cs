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

    //private bool spawnedBoss = false;
    //public GameObject boss;

    private void Start()
    {
        //Debug.LogError("Force the build console open...");
        roomsCount = dungeonSize - 1;
    }

    private void FixedUpdate()
    {
        //if (roomsCount == 0 && spawnedBoss == false)
        if (roomsCount == 0)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == rooms.Count - 1)
                {
                    rooms[i].GetComponent<RoomManger>().isBossRoom = true;
                }
            }
        }
    }
}
