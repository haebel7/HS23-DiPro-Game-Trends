using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private float spawnTime;

    private void Start()
    {
        spawnTime = Time.fixedTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        if (Time.fixedTime >= spawnTime + 10)
        {
            Destroy(gameObject);
        }
    }
}
