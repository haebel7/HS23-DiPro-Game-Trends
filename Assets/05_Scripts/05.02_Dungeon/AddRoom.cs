using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    private RoomTemplates templates;
    public string entryDirect = "";
    public string exitDirect = "";
    public GameObject entry;
    public GameObject exit;

    private void Start()
    {
        GameObject room = GameObject.FindGameObjectWithTag("Rooms");
        templates = room.GetComponent<RoomTemplates>();
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
                doorFunction = (GameObject)Instantiate(Resources.Load("Entry"), door.transform.position, Quaternion.identity);
                entry = doorFunction;
            } else if (door.CompareTag("Door") && door.name[^1..] != entryDirect)
            {
                exitDirect = door.name[^1..];
                door.tag = "Exit";
                doorFunction = (GameObject)Instantiate(Resources.Load("Exit"), door.transform.position, Quaternion.identity);
                exit = doorFunction;
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
