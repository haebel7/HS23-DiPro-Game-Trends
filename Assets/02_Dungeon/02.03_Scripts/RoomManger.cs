using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManger : MonoBehaviour
{
    public EnemyList EnemyList;
    public GameObject exit;

    [SerializeField]
    private GameObject enemySpawnPoints;
    

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
            exit.GetComponent<BoxCollider>().enabled = true;
        }
    }

    public void InitSetting()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        EnemySpawner[] enemySpawners = enemySpawnPoints.GetComponentsInChildren<EnemySpawner>();

        foreach (EnemySpawner spawner in enemySpawners)
        {
            spawner.SpawnEnemies();
        }
    }
}
