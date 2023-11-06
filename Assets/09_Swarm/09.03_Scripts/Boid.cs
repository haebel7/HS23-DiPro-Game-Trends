using UnityEngine;

[RequireComponent(typeof(Collider))]

public class Boid : MonoBehaviour
{
    Flock boidFlock;
    public Flock BoidFlock { get { return boidFlock; } }

    Collider boidCollider;
    public Collider BoidCollider { get { return boidCollider; } }

    void Start()
    {
        boidCollider = GetComponent<Collider>();
    }

    public void Initialize(Flock flock)
    {
        boidFlock = flock;
    }

    public void Move(Vector3 velocity)
    {
        transform.up = velocity; // Boid move in direction head forward
        transform.position += velocity * Time.deltaTime;
    }
}
