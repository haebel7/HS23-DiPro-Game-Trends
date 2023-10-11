using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    private RoomTemplates templates;
    private bool spawned;

    private void Start()
    {
        if(name == "SpawnPoint")
        {
            spawned = true;
        }
        else
        {
            spawned = false;
        }

        GameObject roomTemplates = GameObject.FindGameObjectWithTag("Rooms");
        templates = roomTemplates.GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f); // Delays Spawn()
    }

    private void Spawn()
    {
        templates.spawnPoints.Add(gameObject);

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
            Instantiate(room, transform.position, room.transform.rotation);
            spawned = true;
            templates.roomsCount--;
        }
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
            Debug.Log("Name of spawn point unknown!");
        }

        return room;
    }

    private GameObject GetBossRoom()
    {
        string bossRoomName = null;

        if(name == "SpawnPointN")
        {
            bossRoomName = "S";
        }
        else if(name == "SpawnPointS")
        {
            bossRoomName = "N";
        }
        else if (name == "SpawnPointE")
        {
            bossRoomName = "W";
        }
        else if (name == "SpawnPointW")
        {
            bossRoomName = "S";
        }
        else
        {
            Debug.Log("Unknown name of spawn point!");
        }

        for (int i = 0; i < templates.bossRooms.Length; i++)
        {
            if (bossRoomName == templates.bossRooms[i].name)
            {
                return templates.bossRooms[i];
            }
        }

        Debug.Log("Room not found!");
        return null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            spawned = true;
        }
    }
}
