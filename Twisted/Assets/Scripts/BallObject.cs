using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallObject : MonoBehaviour
{
    [SerializeField] private int rayCount;
    [SerializeField] private float rayLength;
    [SerializeField] private float forcePower;
    [SerializeField] private float forceTimerThreshold;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private float inAnimationTime;
    [SerializeField] private float outAnimationTime;
    [SerializeField] private GameObject visual;
    private Rigidbody rb;
    private List<Vector3> normalizedRays;
    Vector3 forceDirection;

    float testTimer;
    void Awake()
    {
        normalizedRays = new List<Vector3>();
        rb = GetComponent<Rigidbody>();
        RayCastToAllSides();
    }

    private void Update()
    {
        testTimer += Time.deltaTime;
        if (testTimer > forceTimerThreshold)
        {
            foreach (var a in normalizedRays)
            {
                if (Physics.Raycast(transform.position, a, out RaycastHit hit, rayLength, hitMask))
                {
                    //Vector3 dir = (hit.point - transform.position).normalized;

                    if (hit.transform.TryGetComponent(out DestructableCorner hitCorner))
                    {
                        SetDirectionVector(Vector3.Reflect(rb.linearVelocity, hit.normal).normalized);
                        Debug.Log("Trying to Hit The wall");
                        hitCorner.Hit(rb.linearVelocity);
                    }
                    testTimer = 0;
                    break;
                }
            }
        }

        if (rb.linearVelocity != Vector3.zero)
        {
            Vector3 lookDirVelo = -rb.linearVelocity;
            lookDirVelo.y = 0;
            Quaternion lookDirection = Quaternion.LookRotation(lookDirVelo, Vector3.up);
            visual.transform.rotation = Quaternion.Lerp(visual.transform.rotation, lookDirection, 15 * Time.deltaTime);
        }
    }
    private void FixedUpdate()
    {
        if (forceDirection != Vector3.zero)
        {
            rb.linearVelocity = forceDirection * forcePower;

            SetDirectionVector(Vector3.zero);
        }

    }
    public void SetDirectionVector(Vector3 dir)
    {
        forceDirection = dir;
    }
    private List<Vector3> RayCastToAllSides()
    {
        for (int i = 0; i < rayCount; i++)
        {
            float angle = (2 * Mathf.PI) * i / rayCount;
            Vector3 dir = new Vector3(Mathf.Cos(angle), transform.position.z, Mathf.Sin(angle));

            normalizedRays.Add(dir);
        }
        return normalizedRays;
    }


    private void OnEnable()
    {
        ShootManager.Instance.SubscribeToShootManager(this);
    }
    private void OnDisable()
    {
        ShootManager.Instance.UnSubscribeToShootManager(this);
    }
}

