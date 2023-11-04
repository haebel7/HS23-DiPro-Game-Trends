using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Alignment")]

public class AlignmentBehavior : FilteredFlockBehavior
{
    //align with your neighbors or hold your course
    public override Vector3 CalculateMove(Boid boid, List<Transform> context, Flock flock)
    {
        //if no neighbors,maintain current alignment
        if (context.Count == 0)
            return boid.transform.up;

        //add all points together and average
        Vector3 alignmentMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(boid, context);
        foreach (Transform item in filteredContext)
        {
            alignmentMove += item.transform.up;
        }
        alignmentMove /= context.Count;

        return alignmentMove;
    }
}
