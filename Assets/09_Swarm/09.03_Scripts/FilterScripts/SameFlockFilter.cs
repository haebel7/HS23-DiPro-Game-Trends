using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/Same Flock")]
public class SameFlockFilter : ContextFilter
{
    public override List<Transform> Filter(Boid boid, List<Transform> original)
    {
        List<Transform> filtered = new();
        foreach (Transform item in original)
        {
            Boid itemBoid = item.GetComponent<Boid>();
            if (itemBoid != null && itemBoid.BoidFlock == boid.BoidFlock)
            {
                filtered.Add(item);
            }
        }
        return filtered;
    }
}