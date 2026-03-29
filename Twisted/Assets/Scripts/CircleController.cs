using System.Collections.Generic;
using UnityEngine;

public class CircleController : MonoBehaviour
{

    [SerializeField] private GameObject holderPrefab;
    [SerializeField] private int totalObject;
    [SerializeField] private float rotateMultiplier;
    [SerializeField] private float radius;
    [SerializeField] private CircleTurn movementDirection;
    [SerializeField] private bool isRound;
    [SerializeField] private CornersSO cornerData;
    int lastTotalObject;
    float rotateAmount = 0;
    private List<GameObject> totalCreatedObjects;
    void Awake()
    {
        totalCreatedObjects = new List<GameObject>();
        lastTotalObject = totalObject;
        CreateCircle();

    }

    void Update()
    {
        DebugCircle();
        if (InputManager.Instance.MovementVector().x != 0)
        {
            rotateAmount = (int)movementDirection * InputManager.Instance.MovementVector().x * rotateMultiplier * Time.deltaTime;
        }
        else
        {
            rotateAmount = Mathf.Lerp(rotateAmount, 0, 15 * Time.deltaTime);
        }

        transform.Rotate(0, 0, rotateAmount);

    }

    private void CreateCircle()
    {
        for (int i = 0; i < totalObject; i++)
        {
            if (i == 0 && !isRound)
                continue;
            float angle = (2 * Mathf.PI) * i / totalObject;
            GameObject placeholderObject = Instantiate(holderPrefab);
            Corners currentCorner = placeholderObject.AddComponent<Corners>();
            currentCorner.InitializeCorner(cornerData);
            placeholderObject.transform.position = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            float angleDegrees = angle * Mathf.Rad2Deg;
            placeholderObject.transform.LookAt(transform.position);
            placeholderObject.transform.Rotate(0, 0, 90f, Space.Self);
            placeholderObject.transform.parent = transform;
            totalCreatedObjects.Add(placeholderObject);
        }
    }
    private void DebugCircle()
    {
        if (lastTotalObject != totalObject)
        {
            foreach (var a in totalCreatedObjects)
            {
                Destroy(a);
            }
            lastTotalObject = totalObject;
            totalCreatedObjects.Clear();
            CreateCircle();
        }
    }
}
public enum CircleTurn
{
    Idle = 0,
    Reversed = 1,
    Straight = -1
}