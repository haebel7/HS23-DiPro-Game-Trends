using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> enemies;

    public void SpawnEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            GameObject activeEnemy = Instantiate(enemy,transform.position, Quaternion.identity);
            activeEnemy.transform.parent = transform;
        }
    }
}
