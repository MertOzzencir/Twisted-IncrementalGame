using System;
using System.Collections.Generic;
using UnityEngine;

public class ServeSystemManager : MonoBehaviour
{
    [SerializeField] private int totalObject;
    [SerializeField] private float rotateMultiplier;
    [SerializeField] private float radius;
    [SerializeField] private TableTurnDirection movementDirection;
    [SerializeField] private TableData[] tableIndex;
    [SerializeField] private int[] tablesRemoveIndexs;
    int lastTotalObject;
    float rotateAmount = 0;
    private List<ServeMain> createdTables = new List<ServeMain>();
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
        foreach (var c in tableIndex)
            totalCornerCount += c.TotalCount;

        if (totalObject > totalCornerCount)
        {
            Debug.LogWarning($"totalObject ({totalObject}), cornerIndex toplamını ({totalCornerCount}) aşıyor!");
            return;
        }

        if (tableIndex.Length == 0 || totalObject <= 0)
            return;
        int y = 0;
        int previousIndex = 0;
        for (int i = 0; i < totalObject; i++)
        {
            float angle = (2 * Mathf.PI) * i / totalObject;
            GameObject placeholderObject = Instantiate(tableIndex[y].ServeTableData.Prefab);
            ServeMain currentCorner = placeholderObject.GetComponent<ServeMain>();
            createdTables.Add(currentCorner);
            currentCorner.InitializeTable(tableIndex[y].ServeTableData, this);
            placeholderObject.transform.position = new Vector3(Mathf.Cos(angle) * radius, transform.position.y, Mathf.Sin(angle) * radius);
            Vector3 dirToCenter = (transform.position - placeholderObject.transform.position).normalized;
            placeholderObject.transform.rotation = Quaternion.LookRotation(dirToCenter, Vector3.up);
            //placeholderObject.transform.Rotate(0, 90f, 0, Space.Self); // prefabına göre ayarla


            placeholderObject.transform.parent = transform;
            if (i >= tableIndex[y].TotalCount - 1 + previousIndex)
            {
                previousIndex += tableIndex[y].TotalCount;
                y++;
            }
        }
        if (tablesRemoveIndexs.Length > 0)
        {
            List<int> sorted = new List<int>(tablesRemoveIndexs);
            sorted.Sort((a, b) => b.CompareTo(a));

            foreach (var a in sorted)
            {
                Destroy(createdTables[a].gameObject);
                createdTables.RemoveAt(a);
            }
        }
    }
    public void DeleteCornerOnList(ServeMain c)
    {
        foreach (var a in createdTables)
        {
            if (c == a)
            {
                createdTables.Remove(a);
                break;
            }
        }
    }
    private void DebugCircle()
    {
        if (lastTotalObject != totalObject)
        {
            foreach (var a in createdTables)
            {
                if (a.gameObject != null)
                    Destroy(a.gameObject);
            }
            lastTotalObject = totalObject;
            createdTables.Clear();
            CreateCircle();
        }
    }
}
public enum TableTurnDirection
{
    Idle = 0,
    Reversed = 1,
    Straight = -1
}
[Serializable]
public struct TableData
{
    public int TotalCount;
    public ServeMainSO ServeTableData;
}