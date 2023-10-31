using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManger : MonoBehaviour
{
    [SerializeField]
    private GameObject enemySpawnPoints;

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
