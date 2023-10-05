using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    private RoomTemplates templates;

    private void Start()
    {
        GameObject room = GameObject.FindGameObjectWithTag("Rooms");
        templates = room.GetComponent<RoomTemplates>();
        templates.rooms.Add(gameObject);
    }
}
