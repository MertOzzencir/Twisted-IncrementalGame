using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    [SerializeField] private float xOffset;
    [SerializeField] private float zOffset;
    [SerializeField] private GameObject[] plankPrefabs;
    [SerializeField] private int plantRowCount;

    private int lastRowCount;
    private List<GameObject> tempPlankListToDestroyForCreateGround = new List<GameObject>();
    void Update()
    {
        if (plantRowCount != lastRowCount)
        {
            foreach (var a in tempPlankListToDestroyForCreateGround)
                Destroy(a.gameObject);
            tempPlankListToDestroyForCreateGround.Clear();
            CreateGround();
            lastRowCount = plantRowCount;
        }
    }
    private void CreateGround()
    {
        int xRow = plantRowCount / (int)xOffset;
        BoxCollider col = GetComponent<BoxCollider>();
        if (col == null) col = gameObject.AddComponent<BoxCollider>();

        col.size = new Vector3(xRow * xOffset, 0.2f, plantRowCount * zOffset);
        col.center = new Vector3(xRow * xOffset / 2f - xOffset / 2f, 0, (plantRowCount * zOffset) / 2f - zOffset / 2f);

        // geri kalanı aynı
        for (int i = 0; i < plantRowCount; i++)
        {
            for (int y = 0; y < xRow; y++)
            {
                Vector3 tP = transform.position;
                int randPlank = Random.Range(0, plankPrefabs.Length);
                GameObject currentPlank = Instantiate(plankPrefabs[randPlank]);
                Vector3 worldPos = transform.position
    + transform.right * (y * xOffset)
    + transform.forward * (i * zOffset);

                currentPlank.transform.position = worldPos;
                currentPlank.transform.parent = transform;
                tempPlankListToDestroyForCreateGround.Add(currentPlank);
            }
        }
    }

}
