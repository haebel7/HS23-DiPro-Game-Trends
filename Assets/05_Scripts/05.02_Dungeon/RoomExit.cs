using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoomExit : MonoBehaviour
{
    public GameObject nextRoomEntry;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Disable player input
            PlayerControls playerControls = other.transform.Find("Player").GetComponent<Movement>().playerControls;
            InputActionAsset asset = playerControls.asset;
            InputActionMap actionMap = asset.FindActionMap("Gameplay");
            actionMap.Disable();

            // Fade out
            // To do

            // Activate next room


            // Move player to next room
            other.transform.position = nextRoomEntry.transform.position;

            // Deactivate current room


            // Fade in
            // To do
        }
    }
}
