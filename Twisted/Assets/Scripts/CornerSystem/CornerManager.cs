using System;
using System.Collections.Generic;
using UnityEngine;

public class CornerManager : MonoBehaviour
{
    [SerializeField] private int totalObject;
    [SerializeField] private float rotateMultiplier;
    [SerializeField] private float radius;
    [SerializeField] private CircleTurn movementDirection;
    [SerializeField] private CircleData[] cornerIndex;
    [SerializeField] private int[] deletedCornerIndexArray;
    int lastTotalObject;
    float rotateAmount = 0;
    private List<Corners> createdCorners = new List<Corners>();
    void Awake()
    {
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
        transform.Rotate(0, rotateAmount, 0);
    }
    private void CreateCircle()
    {
        int totalCornerCount = 0;
        foreach (var c in cornerIndex)
            totalCornerCount += c.TotalCount;

        if (totalObject > totalCornerCount)
        {
            Debug.LogWarning($"totalObject ({totalObject}), cornerIndex toplamını ({totalCornerCount}) aşıyor!");
            return;
        }

        if (cornerIndex.Length == 0 || totalObject <= 0)
            return;
        int y = 0;
        int previousIndex = 0;
        for (int i = 0; i < totalObject; i++)
        {
            float angle = (2 * Mathf.PI) * i / totalObject;
            GameObject placeholderObject = Instantiate(cornerIndex[y].CornerData.Prefab);
            Corners currentCorner = placeholderObject.GetComponent<Corners>();
            createdCorners.Add(currentCorner);
            currentCorner.InitializeCorner(cornerIndex[y].CornerData, this);
            placeholderObject.transform.position = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            Vector3 dirToCenter = (transform.position - placeholderObject.transform.position).normalized;
            placeholderObject.transform.rotation = Quaternion.LookRotation(dirToCenter, Vector3.up);
            //placeholderObject.transform.Rotate(0, 90f, 0, Space.Self); // prefabına göre ayarla


            placeholderObject.transform.parent = transform;
            if (i >= cornerIndex[y].TotalCount - 1 + previousIndex)
            {
                previousIndex += cornerIndex[y].TotalCount;
                y++;
            }
        }
        if (deletedCornerIndexArray.Length > 0)
        {
            List<int> sorted = new List<int>(deletedCornerIndexArray);
            sorted.Sort((a, b) => b.CompareTo(a));

            foreach (var a in sorted)
            {
                Destroy(createdCorners[a].gameObject);
                createdCorners.RemoveAt(a);
            }
        }
    }
    public void DeleteCornerOnList(Corners c)
    {
        foreach (var a in createdCorners)
        {
            if (c == a)
            {
                createdCorners.Remove(a);
                break;
            }
        }
    }
    private void DebugCircle()
    {
        if (lastTotalObject != totalObject)
        {
            foreach (var a in createdCorners)
            {
                if (a.gameObject != null)
                    Destroy(a.gameObject);
            }
            lastTotalObject = totalObject;
            createdCorners.Clear();
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