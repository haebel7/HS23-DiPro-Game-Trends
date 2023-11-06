using System.Collections.Generic;
using UnityEngine;

public abstract class FlockBehavior : ScriptableObject
{
    public float weight;

    public abstract Vector3 CalculateMove(Boid boid, List<Transform> context, Flock flock); //context means other boids or obstacles
}
