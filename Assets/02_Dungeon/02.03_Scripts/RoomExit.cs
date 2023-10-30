using System;
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
    Collider player;

    public void Start()
    {
        fade = GameObject.Find("FadeInOut").GetComponent<FadeEffect>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other;
            fade.StartEffect();
            actionMap = player.transform.GetComponent<ActionManager>().playerControls.asset.FindActionMap("Gameplay");
            actionMap.Disable();
            Invoke("MoveToNextRoom", 1.5f); // Delay
        }
    }

    private void MoveToNextRoom()
    {
        // Move player to next room
        Transform newTransform = nextRoomEntry.transform;
        newTransform.position += Vector3.up * (player.GetComponent<CharacterController>().height / 2);
        player.transform.position = newTransform.position;

        // Move camera to next room
        GameObject playerLimitedRoot = GameObject.FindGameObjectWithTag("NavMeshAgent");
        newTransform.position += Vector3.up * playerLimitedRoot.GetComponent<PlayerLimitedRoot>().radius;
        NavMeshAgent rootAgent = playerLimitedRoot.GetComponent<NavMeshAgent>();
        rootAgent.Warp(newTransform.position);

        Invoke("NextRoomReady", 1.0f); // Delay
    }

    private void NextRoomReady()
    {
        fade.StartEffect();
        nextRoomEntry.transform.parent.gameObject.GetComponent<RoomManger>().InitSetting();
        actionMap.Enable();
    }
}
