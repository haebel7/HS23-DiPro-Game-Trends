using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoomExit : MonoBehaviour
{
    public GameObject nextRoomEntry;
    private GDTFadeEffect fade;
    private InputActionMap actionMap;

    public void Start()
    {
        fade = GameObject.Find("GDT-Canvas-Fade-Object").GetComponent<GDTFadeEffect>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            actionMap = other.transform.GetComponent<ActionManager>().playerControls.asset.FindActionMap("Gameplay");
            actionMap.Disable();

            // Fade out
            fade.enabled = true;

            // Move player to next room
            other.transform.position = nextRoomEntry.transform.position;
            Invoke("EnableActionMap", 2.2f); // Delay
        }
    }

    private void EnableActionMap()
    {
        actionMap.Enable();
    }
}
