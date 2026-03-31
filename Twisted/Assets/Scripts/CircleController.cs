using System;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;

public class CircleController : MonoBehaviour
{

    [SerializeField] private int totalObject;
    [SerializeField] private float rotateMultiplier;
    [SerializeField] private float radius;
    [SerializeField] private CircleTurn movementDirection;
    [SerializeField] private CircleData[] cornerIndex;
    [SerializeField] private int[] deletedCornerIndexArray;
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
        int y = 0;
        int previousIndex = 0;
        for (int i = 0; i < totalObject; i++)
        {

            float angle = (2 * Mathf.PI) * i / totalObject;
            GameObject placeholderObject = Instantiate(cornerIndex[y].CornerData.Prefab);
            Corners currentCorner = placeholderObject.GetComponent<Corners>();
            currentCorner.InitializeCorner(cornerIndex[y].CornerData);
            placeholderObject.transform.position = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            Vector3 dirToCenter = (transform.position - placeholderObject.transform.position).normalized;
            float angleDegrees = Mathf.Atan2(dirToCenter.y, dirToCenter.x) * Mathf.Rad2Deg;
            placeholderObject.transform.rotation = Quaternion.Euler(0, 0, angleDegrees - 90f);
            placeholderObject.transform.parent = transform;
            totalCreatedObjects.Add(placeholderObject);
            if (i >= cornerIndex[y].TotalCount - 1 + previousIndex)
            {
                previousIndex += cornerIndex[y].TotalCount;
                y++;
            }

        }
        if (deletedCornerIndexArray.Length > 0)
        {
            foreach (var a in deletedCornerIndexArray)
            {
                Destroy(totalCreatedObjects[a]);
            }
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
[Serializable]
public struct CircleData
{
    public int TotalCount;
    public CornersSO CornerData;
}