using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    private RoomTemplates templates;
    private string entryDirect = "";
    private GameObject entry;

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        templates.rooms.Add(gameObject);
    }

    public void SetEntry(string entryDirection, GameObject lastRoomExit)
    {
        entryDirect = entryDirection;
        GameObject door;
        GameObject doorFunction = null;

        for (int i = 0; i < transform.childCount; i++)
        {
            door = transform.GetChild(i).gameObject;

            if (door.CompareTag("Door") && door.name[^1..] == entryDirect)
            {
                entryDirect = door.name[^1..];
                door.tag = "Entry";

                // For debugging
                door.transform.Find("ShowDoor").GetComponent<MeshRenderer>().material.mainTexture = Resources.Load<Texture>("prototype_512x512_green3");

                
                doorFunction = (GameObject)Instantiate(Resources.Load("02.01.02.01_Entry"), door.transform.position, door.transform.rotation);
                entry = doorFunction;
            }
            else if (door.CompareTag("Door") && door.name[^1..] != entryDirect)
            {
                door.tag = "Exit";
                doorFunction = (GameObject)Instantiate(Resources.Load("02.01.02.02_Exit"), door.transform.position, door.transform.rotation);
            }

            if (doorFunction != null)
            {
                doorFunction.transform.parent = door.transform;
                doorFunction = null;
            }
        }

        lastRoomExit.GetComponent<RoomExit>().nextRoomEntry = entry;
    }
}
