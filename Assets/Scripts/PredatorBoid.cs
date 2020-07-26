using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorBoid : Boid
{
    protected override void Start()
    {
        originalColour = Color.red;
        SetColour(originalColour);
    }

    protected override void BoidColourUpdate()
    {
        return;
    }
}
