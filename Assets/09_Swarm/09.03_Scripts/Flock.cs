using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public Boid boidPrefab;
    private readonly List<Boid> boids = new();
    public GameObject behaviorObj;
    private CompositeBehavior behavior;

    [Range(10, 500)]
    public int startingCount = 250;
    const float BoidDensity = 0.09f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neightborRadius = 1.5f;
    [Range(0f, 1f)]
    public float separationRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareSeparationRadius;
    public float SquareSeparationRadius { get { return squareSeparationRadius; } }

    // Start is called before the first frame update
    void Start()
    {
        behavior = behaviorObj.GetComponent<CompositeBehavior>();
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neightborRadius * neightborRadius;
        squareSeparationRadius = squareNeighborRadius * separationRadiusMultiplier * separationRadiusMultiplier;

        for (int i = 0; i < startingCount; i++)
        {
            Boid newBoid = Instantiate(
                boidPrefab,
                transform.position + BoidDensity / 2 * startingCount * Random.insideUnitSphere, // BoidDensity * startingCount sets the radius
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
                );
            newBoid.name = "Boid " + i;
            newBoid.Initialize(this);
            boids.Add(newBoid);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Boid boid in boids)
        {
            List<Transform> context = GetNearbyObjects(boid);
            Vector3 move = behavior.CalculateMove(boid, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            boid.Move(move);
        }
    }

    List<Transform> GetNearbyObjects(Boid boid)
    {
        List<Transform> context = new();
        Collider[] contextColliders = Physics.OverlapSphere(boid.transform.position, neightborRadius);
        foreach (Collider c in contextColliders)
        {
            if (c != boid.BoidCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
}
