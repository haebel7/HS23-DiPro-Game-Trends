using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    private RoomTemplates templates;
    private bool spawned;
    private string currentExitDirection;
    GameObject newRoom;
    GameObject currentRoom;
    GameObject currentExitDoor;
    GameObject currentExit;

    private bool entrySet = false;

    private void Start()
    {
        if (name == "SpawnPoint")
        {
            spawned = true;
        }
        else
        {
            spawned = false;
        }

        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f); // Delays Spawn()
    }

    private void FixedUpdate()
    {
        if (!entrySet && newRoom && currentRoom && currentExitDoor && currentExit)
        {
            SetEntryNextRoom();
            entrySet = true;
        }
    }

    private void Spawn()
    {
        GameObject room = null;

        if (spawned == false && templates.roomsCount > 1)
        {
            room = GetRoom();
        }
        else if (spawned == false && templates.rooms.Count == templates.dungeonSize - 1)
        {
            room = GetBossRoom();
        }

        if (room != null)
        {
            newRoom = Instantiate(room, transform.position, transform.rotation);
            currentRoom = gameObject.transform.parent.gameObject;
            currentExitDoor = null;
            currentExit = null;

            // set entry and exit of new room.
            currentExitDirection = name[^1..];
            

            for (int i = 0; i < currentRoom.transform.childCount; i++)
            {
                if (currentRoom.transform.GetChild(i).name == "Door" + currentExitDirection)
                {
                    currentExitDoor = currentRoom.transform.GetChild(i).gameObject;
                    
                    for (int j = 0; j < currentExitDoor.transform.childCount; j++)
                    {
                        if (currentExitDoor.transform.GetChild(j).CompareTag("Exit"))
                        {
                            currentExit = currentExitDoor.transform.GetChild(j).gameObject;
                            currentExit.GetComponent<BoxCollider>().enabled = false;
                            transform.parent.GetComponent<RoomManger>().exit = currentExit;
                        }
                    }
                }
            }
        }
    }

    private void SetEntryNextRoom()
    {
        if (currentExitDirection == "N")
        {
            newRoom.GetComponent<AddRoom>().SetEntry("S", currentExit);
        }
        else if (currentExitDirection == "E")
        {
            newRoom.GetComponent<AddRoom>().SetEntry("W", currentExit);
        }
        else if (currentExitDirection == "S")
        {
            newRoom.GetComponent<AddRoom>().SetEntry("N", currentExit);
        }
        else if (currentExitDirection == "W")
        {
            newRoom.GetComponent<AddRoom>().SetEntry("E", currentExit);
        }
        else
        {
            Debug.LogWarning("Entry next room unknown!");
        }

        spawned = true;
        templates.roomsCount--;
    }

    // Works only, if each template name is unique!!!! => Do do: Should work, even when there are multiple templates with the same name!
    // Excludes cicular dungeons.
    private int CircularCheck(List<GameObject> selectableRooms)
    {
        if (templates.roomsCount < templates.dungeonSize - 3 && templates.rooms[^1].name == templates.rooms[^2].name)
        {
            // Set circular room at the end of array.
            return ExcludeRoom(selectableRooms);
        }
        else
        {
            return selectableRooms.Count - 1;
        }
    }

    private int ExcludeRoom(List<GameObject> selectableRooms)
    {
        GameObject roomToMove = selectableRooms.Find(room => room.name == (templates.rooms[^1].name));
        selectableRooms.Remove(roomToMove);
        selectableRooms.Add(roomToMove);
        return selectableRooms.Count - 2;
    }

    private GameObject GetRoom()
    {
        GameObject room = null;

        if (name == "SpawnPointN")
        {
            room = templates.southRooms[UnityEngine.Random.Range(0, CircularCheck(templates.southRooms))];
        }
        else if (name == "SpawnPointS")
        {
            room = templates.northRooms[UnityEngine.Random.Range(0, CircularCheck(templates.northRooms))];
        }
        else if (name == "SpawnPointE")
        {
            room = templates.westRooms[UnityEngine.Random.Range(0, CircularCheck(templates.westRooms))];
        }
        else if (name == "SpawnPointW")
        {
            room = templates.eastRooms[UnityEngine.Random.Range(0, CircularCheck(templates.eastRooms))];
        }
        else
        {
            Debug.LogWarning("Name of spawn point unknown!");
        }

        return room;
    }

    private GameObject GetBossRoom()
    {
        string bossRoomTag = null;

        if (name == "SpawnPointN")
        {
            bossRoomTag = "S";
        }
        else if (name == "SpawnPointS")
        {
            bossRoomTag = "N";
        }
        else if (name == "SpawnPointE")
        {
            bossRoomTag = "W";
        }
        else if (name == "SpawnPointW")
        {
            bossRoomTag = "E";
        }
        else
        {
            Debug.LogWarning("Unknown name of spawn point!");
        }

        for (int i = 0; i < templates.bossRooms.Length; i++)
        {
            if (templates.bossRooms[i].CompareTag(bossRoomTag))
            {
                return templates.bossRooms[i];
            }
        }

        Debug.LogWarning("Room not found!");
        return null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            spawned = true;
        }
    }
}
