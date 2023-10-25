using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class RoomExit : MonoBehaviour
{
    public GameObject nextRoomEntry;
    private FadeEffect fade;
    private InputActionMap actionMap;

    public void Start()
    {
        fade = GameObject.Find("FadeInOut").GetComponent<FadeEffect>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Fade in
            fade.enabled = true;
            fade.StartEffect();
            fade.firstToLast = false;
            fade.enabled = false;

            // Disable player control
            actionMap = other.transform.GetComponent<ActionManager>().playerControls.asset.FindActionMap("Gameplay");
            actionMap.Disable();

            // Move player to next room
            Transform newTransform = nextRoomEntry.transform;
            newTransform.position += Vector3.up * (other.GetComponent<CharacterController>().height / 2);
            other.transform.position = newTransform.position;

            // Move camera to next room
            GameObject playerLimitedRoot = GameObject.FindGameObjectWithTag("NavMeshAgent");
            newTransform.position += Vector3.up * playerLimitedRoot.GetComponent<PlayerLimitedRoot>().radius;
            NavMeshAgent rootAgent = playerLimitedRoot.GetComponent<NavMeshAgent>();
            rootAgent.Warp(newTransform.position);

            Invoke("EnableActionMap", 2.5f); // Delay
        }
    }

    private void EnableActionMap()
    {
        // Fade out
        fade.enabled = true;
        fade.StartEffect();
        fade.firstToLast = true;
        fade.enabled = false;

        // Ensable player control
        actionMap.Enable();
    }
}
