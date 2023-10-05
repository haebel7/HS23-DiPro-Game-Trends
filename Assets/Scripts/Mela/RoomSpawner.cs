using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public string openingDirection;
    // N > need door south
    // S > need door north
    // E > need door west
    // W > need door east

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
        Invoke("Spawn", 0.1f); // Delays Spawn()
    }

    private void Spawn()
    {
        if(spawned == false)
        {
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
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("entred OnTriggerEnter");
        if (other.CompareTag("SpawnPoint"))
        {
            Debug.Log("entred if SpawnPoint");
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                Debug.Log("entred if closedRoom");
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            spawned = true;
        }
    }
}
