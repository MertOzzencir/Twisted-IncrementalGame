using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallObject : MonoBehaviour
{
    [SerializeField] private Transform[] bounceDirections;
    [SerializeField] private int rayCount;
    [SerializeField] private float rayLength;
    [SerializeField] private float forcePower;
    [SerializeField] private float forceTimerThreshold;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private float inAnimationTime;
    [SerializeField] private float outAnimationTime;
    private Rigidbody rb;
    private List<Vector3> normalizedRays;
    Vector3 forceDirection;
    Coroutine ballAnimation;

    float testTimer;
    void Awake()
    {
        ballAnimation = null;
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

                    if (hit.transform.TryGetComponent(out DestructableCorner hitCorner))
                    {
                        Debug.Log("Trying to Hit The wall");
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
            foreach (var b in bounceDirections)
            {
                float bounceThreshhold = Vector3.Dot(b.forward, forceDirection);
                if (bounceThreshhold >= 0.90f)
                {
                    if (ballAnimation != null)
                    {
                        StopAllCoroutines();
                        foreach (var c in bounceDirections)
                            c.localScale = Vector3.one;
                    }
                    ballAnimation = StartCoroutine(BallAnimation(b));
                    break;
                }
            }
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

    private IEnumerator BallAnimation(Transform currentBounceSide)
    {
        Vector3 originalScale = currentBounceSide.localScale;
        Vector3 animationScale = new Vector3(originalScale.x, originalScale.y, originalScale.z * 2f);
        while (Vector3.Distance(currentBounceSide.localScale, animationScale) > 0.001f)
        {
            currentBounceSide.transform.localScale = Vector3.MoveTowards(currentBounceSide.transform.localScale, animationScale, inAnimationTime * Time.deltaTime);
            yield return null;
        }
        currentBounceSide.transform.localScale = animationScale;
        while (Vector3.Distance(currentBounceSide.localScale, originalScale) > 0.001f)
        {
            currentBounceSide.transform.localScale = Vector3.MoveTowards(currentBounceSide.transform.localScale, originalScale, outAnimationTime * Time.deltaTime);
            yield return null;
        }
        currentBounceSide.transform.localScale = originalScale;
        ballAnimation = null;
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

