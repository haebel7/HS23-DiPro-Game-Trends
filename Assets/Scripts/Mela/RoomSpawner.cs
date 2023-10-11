using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public string openingDirection;
    private RoomTemplates templates;
    private int rand;
    private bool spawned;

    private void Start()
    {
        if(openingDirection == "0")
        {
            spawned = true;
        }
        else
        {
            spawned = false;
        }

        GameObject roomTemplates = GameObject.FindGameObjectWithTag("Rooms");
        templates = roomTemplates.GetComponent<RoomTemplates>();
        Invoke("Spawn", 1f); // Delays Spawn()
    }

    private void Spawn()
    {
        if(spawned == false && templates.roomsCount > 1)
        {
            templates.spawnPoints.Add(gameObject);
            if (openingDirection == "N")
            {
                // Need to spawn a room with a south door.
                rand = UnityEngine.Random.Range(0, templates.southRooms.Length);
                Instantiate(templates.southRooms[rand], transform.position, templates.southRooms[rand].transform.rotation);
            }
            else if (openingDirection == "S")
            {
                // Need to spawn a room with a north door.
                rand = UnityEngine.Random.Range(0, templates.northRooms.Length);
                Instantiate(templates.northRooms[rand], transform.position, templates.northRooms[rand].transform.rotation);
            }
            else if (openingDirection == "E")
            {
                // Need to spawn a room with a west door.
                rand = UnityEngine.Random.Range(0, templates.westRooms.Length);
                Instantiate(templates.westRooms[rand], transform.position, templates.westRooms[rand].transform.rotation);
            }
            else if (openingDirection == "W")
            {
                // Need to spawn a room with a east door.
                rand = UnityEngine.Random.Range(0, templates.eastRooms.Length);
                Instantiate(templates.eastRooms[rand], transform.position, templates.eastRooms[rand].transform.rotation);
            }
            else
            {
                Debug.Log("No opening direction!");
            }

            spawned = true;
            templates.roomsCount--;
        }

        //Debug.Log("name: " + name + ", index: " + templates.rooms.FindIndex(room => room == this));
        if (spawned == false && templates.rooms.Count == templates.dungeonSize - 1)
        {
            Debug.Log("rooms count: " + templates.rooms.Count);
            Debug.Log("this: " + name + ", second last room: "+ templates.rooms[templates.rooms.Count - 1].name);
            templates.spawnPoints.Add(gameObject);

            GameObject bossRoom;

            switch (name)
            {
                case "SpawnPointS":
                    bossRoom = GetBossRoom("N");
                    Debug.Log("Last spawn point? " + name);
                    Instantiate(bossRoom, transform.position, bossRoom.transform.rotation);
                    break;
                case "SpawnPointN":
                    bossRoom = GetBossRoom("S");
                    Debug.Log("Last spawn point? " + name);
                    Instantiate(bossRoom, transform.position, bossRoom.transform.rotation);
                    break;
                case "SpawnPointW":
                    bossRoom = GetBossRoom("E");
                    Debug.Log("Last spawn point? " + name);
                    Instantiate(bossRoom, transform.position, bossRoom.transform.rotation);
                    break;
                case "SpawnPointE":
                    bossRoom = GetBossRoom("W");
                    Debug.Log("Last spawn point? " + name);
                    Instantiate(bossRoom, transform.position, bossRoom.transform.rotation);
                    break;
                default:
                    Debug.Log("Opening direction not found!");
                    break;
            }

            spawned = true;
            templates.roomsCount--;
        }
    }

    //private string GetLastOpening()
    //{
    //    //GameObject parent = this.transform.parent.gameObject;
    //    GameObject secondLastRoom = templates.rooms[templates.rooms.Count - 1];
    //    Debug.Log("Second last room: "+secondLastRoom.name);

    //    for (int i = 0; i < secondLastRoom.transform.childCount; i++)
    //    {
    //        Debug.Log("this.name: " + name + "; otherChild.name: " + secondLastRoom.transform.GetChild(i).name);
    //        if (secondLastRoom.transform.GetChild(i).name == name)
    //        {
    //            string lastOpening = secondLastRoom.transform.GetChild(i).name;
    //            lastOpening = lastOpening.Substring(lastOpening.Length - 1);
    //            return lastOpening;
    //        }
    //    }

    //    return "Other child object not found!";
    //}

    private GameObject GetBossRoom(string roomName)
    {
        for (int i = 0; i < templates.bossRooms.Length; i++)
        {
            if (roomName == templates.bossRooms[i].name)
            {
                return templates.bossRooms[i];
            }
        }

        Debug.Log("Room not found!");
        return null;
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("entred OnTriggerEnter");
        if (other.CompareTag("SpawnPoint"))
        {
            //Debug.Log("entred if SpawnPoint");
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                //Debug.Log("entred if closedRoom");
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            spawned = true;
        }
    }
}
