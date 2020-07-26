using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [SerializeField]
    private float separationDistance = 4f;
    [SerializeField]
    private float steerSpeed = 1.5f;
    [SerializeField]
    private float speed = 7f;
    [SerializeField]
    private float maxDistFromCenter = 100f;
    [SerializeField]
    private MeshRenderer meshRenderer;

    private HashSet<Boid> nearbyBoids = new HashSet<Boid>();
    protected Color originalColour;

    public Color OriginalColour
    {
        get
        {
            return originalColour;
        }
    }

    public MeshRenderer Renderer
    {
        get
        {
            return meshRenderer;
        }
    }

    private Boid ColliderToBoid(Collider collider)
    {
        GameObject obj = collider.gameObject;
        return obj.GetComponent<Boid>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Boid otherBoid = ColliderToBoid(other);
        if (otherBoid != null) nearbyBoids.Add(otherBoid);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        Boid otherBoid = ColliderToBoid(other);
        if (otherBoid != null) nearbyBoids.Remove(otherBoid);
        if (nearbyBoids.Count == 0) SetColour(originalColour);
    }

    protected float DistanceTo(Boid otherBoid)
    {
        return Vector3.Distance(transform.position, otherBoid.transform.position);
    }

    protected virtual void Start()
    {
        originalColour = Random.ColorHSV();
        SetColour(originalColour);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
        UpdateMovement(GetTotalDirectionTarget());
        BoidColourUpdate();
    }

    protected virtual Vector3 GetTotalDirectionTarget()
    {
        Vector3 target = Vector3.zero;
        if (Vector3.Distance(Vector3.zero, transform.position) > maxDistFromCenter)
        {
            target = Vector3.zero - transform.position;
        }

        return BoidBehaviour(target);
    }

    protected virtual Vector3 BoidBehaviour(Vector3 baseTarget)
    {
        Vector3 target = baseTarget;
        if (nearbyBoids.Count > 0)
        {
            Vector3 totalPosition = Vector3.zero;
            Vector3 totalForwards = Vector3.zero;
            foreach (Boid otherBoid in nearbyBoids)
            {
                if (DistanceTo(otherBoid) < separationDistance)
                {
                    Vector3 oppositeDirection = transform.position + (otherBoid.transform.position - transform.position) * -1f;
                    //Debug.DrawLine(transform.position, oppositeDirection, Color.green);
                    target += oppositeDirection;
                }
                else
                {
                    totalPosition += otherBoid.transform.position;
                    totalForwards += otherBoid.transform.forward;
                }
            }

            Vector3 othersAveragePos = totalPosition / nearbyBoids.Count;
            target += othersAveragePos;
            Vector3 othersAverageForwards = totalForwards / nearbyBoids.Count;
            target += othersAverageForwards;
        }

        Vector3 targetDirection = target - transform.position;
        return targetDirection;
    }

    private void UpdateMovement(Vector3 targetDirection)
    {
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, steerSpeed * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
        //Debug.DrawRay(transform.position, newDirection, Color.red);

        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward.normalized, speed * Time.deltaTime);
    }

    protected virtual void BoidColourUpdate()
    {
        Color totalColour = originalColour * originalColour;
        foreach (Boid otherBoid in nearbyBoids)
        {
            Color otherColour = otherBoid.OriginalColour;
            totalColour += (otherColour * otherColour);
        }
        SetColour(ColourSqrRt(totalColour / (nearbyBoids.Count + 1)));
    }

    protected Color ColourSqrRt(Color color)
    {
        return new Color(Mathf.Sqrt(color.r), Mathf.Sqrt(color.g), Mathf.Sqrt(color.b), Mathf.Sqrt(color.a));
    }

    protected void SetColour(Color colour)
    {
        meshRenderer.material.SetColor("_Color", colour);
    }
}
