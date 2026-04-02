using System.Collections;
using UnityEngine;

public abstract class Sources : MonoBehaviour
{
    public SourcesSO SourceType;
    VerticalMovementController controller;
    private bool isCollected;
    public Rigidbody rb { get; set; }
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Collect()
    {
        if (!isCollected)
        {
            isCollected = true;
            StartCoroutine(CollectAnimation());
        }
    }
    public void FixedUpdate()
    {
        rb.angularVelocity = Vector3.up * 5f;
    }
    private IEnumerator CollectAnimation()
    {
        controller = GetComponent<VerticalMovementController>();
        controller.rideHeight = controller.rideHeight * 7;
        while (Vector3.Distance(transform.localScale, Vector3.zero) > 0.01f)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, 7f * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }
    public void OnDestroy()
    {
        InventoryManager.Instance.AddSource(SourceType);
    }
}
public enum SourceType
{
    Coin,
    Ketchup
}
