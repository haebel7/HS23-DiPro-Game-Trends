using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManger : MonoBehaviour
{
    [SerializeField]
    private GameObject enemySpawnPoints;

    void Start()
    {
        if (CompareTag("EntryRoom"))
        {
            InitSetting();
        }
    }

    public void InitSetting()
    {
        Debug.Log("init settings.");
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
