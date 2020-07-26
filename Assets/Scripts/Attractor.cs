using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    [SerializeField]
    private float force = 1f;

    public float Force
    {
        get
        {
            return force;
        }
    }
}
