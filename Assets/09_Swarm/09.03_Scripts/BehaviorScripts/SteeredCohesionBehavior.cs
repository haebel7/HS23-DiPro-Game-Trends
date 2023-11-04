using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Steered Cohesion")]

public class SteeredCohesionBehavior : FilteredFlockBehavior
{
    Vector3 currentVelosity;
    public float boidSmoothTime = 0.5f;

    //find the middle point of all neighbors and move there
    public override Vector3 CalculateMove(Boid boid, List<Transform> context, Flock flock)
    {
        //if no neighbors, return no adjustment
        if (context.Count == 0)
            return Vector3.zero;

        //add all points together and average
        Vector3 cohesionMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(boid, context);
        foreach (Transform item in filteredContext)
        {
            cohesionMove += item.position;
        }
        cohesionMove /= context.Count;

        //create offset from boid position
        cohesionMove -= boid.transform.position;
        cohesionMove = Vector3.SmoothDamp(boid.transform.up, cohesionMove, ref currentVelosity, boidSmoothTime); //ref will change the value of the original variable
        return cohesionMove;
    }
}
