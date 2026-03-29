using System.Collections.Generic;
using UnityEngine;

public class BallPhysic : MonoBehaviour
{
    [SerializeField] private int rayCount;
    [SerializeField] private float rayLength;
    [SerializeField] private float forcePower;
    [SerializeField] private float forceTimerThreshold;
    [SerializeField] private LayerMask hitMask;
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
                    Vector3 dir = (hit.point - transform.position).normalized;
                    SetDirectionVector(Vector3.Reflect(dir, hit.normal).normalized);
                    if (hit.transform.TryGetComponent(out Corners hitCorner))
                    {
                        hitCorner.Hit(dir);
                    }
                    testTimer = 0;
                    break;
                }
            }
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
            Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);

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

