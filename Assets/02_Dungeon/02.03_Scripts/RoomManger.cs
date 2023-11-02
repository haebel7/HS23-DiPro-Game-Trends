using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManger : MonoBehaviour
{
    public EnemyList EnemyList;
    public GameObject exit;

    [SerializeField]
    private GameObject enemySpawnPoints;

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
        Debug.Log("is boss room? " + isBossRoom);

        if (EnemyList.enemies.Count == 0)
        {
            if (!isBossRoom)
            {
                exit.GetComponent<BoxCollider>().enabled = true;
            }
            else if (bossRoomActive && !victorySceneloaded)
            {
                SceneManager.LoadSceneAsync("Assets/02_Dungeon/02.02_Scenes/VictoryScene.unity", LoadSceneMode.Additive);
                victorySceneloaded = true;
            }
        }
    }

    public void InitSetting()
    {
        if (isBossRoom)
        {
            GameObject.Find("Canvas").SetActive(false);
            bossRoomActive = SpawnEnemies();
        }
        else
        {
            SpawnEnemies();
        }
    }

    private bool SpawnEnemies()
    {
        EnemySpawner[] enemySpawners = enemySpawnPoints.GetComponentsInChildren<EnemySpawner>();

        foreach (EnemySpawner spawner in enemySpawners)
        {
            spawner.SpawnEnemies();
        }

        return true;
    }
}
