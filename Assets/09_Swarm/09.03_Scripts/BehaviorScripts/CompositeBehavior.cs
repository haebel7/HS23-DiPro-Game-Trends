using System.Collections.Generic;
using UnityEngine;

public class CompositeBehavior : MonoBehaviour
{
    public List<FlockBehavior> behaviors;

    public Vector3 CalculateMove(Boid boid, List<Transform> context, Flock flock)
    {
        //set up move
        Vector3 move = Vector3.zero;

        //iterate through behaviors
        for (int i = 0; i < behaviors.Count; i++)
        {
            Vector3 partialMove = behaviors[i].CalculateMove(boid, context, flock) * behaviors[i].weight;

            if(partialMove != Vector3.zero)
            {
                if(partialMove.sqrMagnitude > behaviors[i].weight * behaviors[i].weight)
                {
                    partialMove.Normalize();
                    partialMove *= behaviors[i].weight;
                }

                move += partialMove;
            }
        }

        return move;
    }
}
