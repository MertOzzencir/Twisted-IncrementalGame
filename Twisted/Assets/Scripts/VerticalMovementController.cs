using UnityEngine;

public class VerticalMovementController : MonoBehaviour
{
    public float rideHeight;
    public float rideSpringStrength;
    public float rideSpringDamper;
    private float VerticalOffSetForce = 1f;
    private Vector3 VerticalDirection;

    Ray ray;

    Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void VerticalMovement(Rigidbody rb, float rideHeightFeed)
    {
        ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 vel = rb.linearVelocity;
            Vector3 rayDir = transform.forward;

            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = hit.rigidbody;

            if (hitBody != null)
                otherVel = hitBody.linearVelocity;

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;

            float x = hit.distance - rideHeightFeed;
            VerticalOffSetForce = x * rideSpringStrength - relVel * rideSpringDamper;
            VerticalDirection = rayDir;

        }
        else
            VerticalDirection = -Physics.gravity / VerticalOffSetForce;

        ApplyLogic();
    }
    public void ApplyLogic()
    {
        rb.AddForce(VerticalDirection * VerticalOffSetForce);
    }
    public void FixedUpdate()
    {
        VerticalMovement(rb, rideHeight);
    }

}