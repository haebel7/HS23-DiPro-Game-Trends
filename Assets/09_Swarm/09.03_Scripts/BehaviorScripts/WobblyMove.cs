using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Wobbly Move")]

public class WobblyMove : FlockBehavior
{
    [Range(0.1f, 3.0f)]
    public float wobbleFactor = 0.2f;

    //align with your neighbors or hold your course
    public override Vector3 CalculateMove(Boid boid, List<Transform> context, Flock flock)
    {
        Vector3 wobblyMove = new(Random.Range(-wobbleFactor, wobbleFactor), Random.Range(-wobbleFactor, wobbleFactor), Random.Range(-wobbleFactor, wobbleFactor));
        return wobblyMove;
    }
}
