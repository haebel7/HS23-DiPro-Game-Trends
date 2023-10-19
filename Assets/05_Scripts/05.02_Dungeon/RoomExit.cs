using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoomExit : MonoBehaviour
{
    public GameObject nextRoomEntry;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("entred OnTriggerEnter");

        if (other.CompareTag("Player"))
        {
            // Disable player input
            PlayerControls playerControls = other.transform.GetComponent<Movement>().playerControls;
            InputActionAsset asset = playerControls.asset;
            InputActionMap actionMap = asset.FindActionMap("Gameplay");

            //Debug.Log("action map: " + actionMap);
            actionMap.Disable();
            //Debug.Log("actionMap enabled? " + actionMap.enabled);
            

            // Fade out
            // To do

            // Activate next room


            // Move player to next room
            //Debug.Log("nextRoomEntry: " + nextRoomEntry);
            //Debug.Log("other.transform.position: " + other.transform.position);
            //Debug.Log("nextRoomEntry.transform.position: " + nextRoomEntry.transform.position);

            other.transform.position = nextRoomEntry.transform.position;
            //Debug.Log("other.transform.position: " + other.transform.position);
            //actionMap.Enable();

            // Deactivate current room


            // Fade in
            // To do
        }
    }
}
