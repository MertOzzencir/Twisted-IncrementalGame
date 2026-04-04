using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterManager : MonoBehaviour
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
    WaiterAnimationController animController;
    void Awake()
    {
        animController = GetComponent<WaiterAnimationController>();
        normalizedRays = new List<Vector3>();
        rb = GetComponent<Rigidbody>();
        RayCastToAllSides();
    }
    float animTimer = 0;
    private void Update()
    {
        testTimer += Time.deltaTime;
        animTimer += Time.deltaTime;
        if (testTimer > forceTimerThreshold)
        {
            foreach (var a in normalizedRays)
            {
                if (Physics.Raycast(transform.position, a, out RaycastHit hit, rayLength, hitMask))
                {
                    //Vector3 dir = (hit.point - transform.position).normalized;

                    if (hit.transform.TryGetComponent(out ServeableTable hitCorner))
                    {
                        SetDirectionVector(Vector3.Reflect(rb.linearVelocity, hit.normal).normalized);
                        Debug.Log("Trying to Hit The wall");
                        if (animTimer > 0.1f)
                        {
                            animController.TriggerHit();
                            animTimer = 0;
                        }
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
            Vector3 dir = new Vector3(Mathf.Cos(angle), transform.position.y, Mathf.Sin(angle));

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

