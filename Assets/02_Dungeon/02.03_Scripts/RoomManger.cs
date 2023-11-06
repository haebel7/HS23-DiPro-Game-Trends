using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManger : MonoBehaviour
{
    public EnemyList EnemyList;
    public GameObject exit;

    [SerializeField]
    private GameObject spawnPoints;

    //[HideInInspector]
    public bool isBossRoom = false;
    private bool bossRoomActive = false;
    private bool victorySceneloaded = false;


    void Start()
    {
        if (CompareTag("EntryRoom"))
        {
            InitSetting();
        }
    }

    private void FixedUpdate()
    {
        if (EnemyList.enemies.Count == 0)
        {
            if (!isBossRoom)
            {
                exit.GetComponent<BoxCollider>().enabled = true;
            }
            else if (bossRoomActive && !victorySceneloaded)
            {
                SceneManager.LoadSceneAsync("Assets/00_Main/07.02.004_VictoryScene.unity", LoadSceneMode.Additive);
                victorySceneloaded = true;
            }
        }
    }

    public void InitSetting()
    {
        if (isBossRoom)
        {
            GameObject.Find("Canvas").SetActive(false);
            bossRoomActive = InitSpawns();
        }
        else
        {
            InitSpawns();
        }
    }

    private bool InitSpawns()
    {
        Spawner[] spawners = spawnPoints.GetComponentsInChildren<Spawner>();

        foreach (Spawner spawner in spawners)
        {
            spawner.SpawnItems();
        }

        return true;
    }
}
