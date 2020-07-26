using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendedBoid : Boid
{
    private HashSet<Attractor> nearbyAttractors = new HashSet<Attractor>();

    protected override Vector3 GetTotalDirectionTarget()
    {
        Vector3 baseTotal = base.GetTotalDirectionTarget();
        return baseTotal + GetAttractorDirectionTarget();
    }

    protected Vector3 GetAttractorDirectionTarget()
    {
        Vector3 attractorTotal = Vector3.zero;
        if (nearbyAttractors.Count > 0)
        {
            foreach (Attractor attractor in nearbyAttractors)
            {
                Vector3 currentAttractorContrib = (attractor.transform.position - transform.position) * attractor.Force;
                attractorTotal += currentAttractorContrib;
            }
            attractorTotal = attractorTotal / nearbyAttractors.Count;
        }
        return attractorTotal;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        Attractor attractor = other.GetComponent<Attractor>();
        if (attractor != null) nearbyAttractors.Add(attractor);
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        Attractor attractor = other.GetComponent<Attractor>();
        if (attractor != null && nearbyAttractors.Contains(attractor)) nearbyAttractors.Remove(attractor);
    }
}
