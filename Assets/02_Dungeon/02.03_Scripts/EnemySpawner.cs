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
            Instantiate(enemy, transform.parent);
        }
    }
}