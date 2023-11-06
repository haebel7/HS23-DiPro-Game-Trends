using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> items;

    public void SpawnItems()
    {
        foreach (GameObject item in items)
        {
            GameObject activeEnemy = Instantiate(item,transform.position, Quaternion.identity);
            activeEnemy.transform.parent = transform;
        }
    }
}
