using System.Collections.Generic;
using UnityEngine;

public class LineIndicator : MonoBehaviour
{
    [SerializeField] private GameObject indicator;

    List<LineRenderer> indicators = new List<LineRenderer>();

    public void DrawLine(int lineIndex, Vector3 pos1, Vector3 pos2, Vector3 pos3, float lineLength)
    {
        indicators[lineIndex].SetPosition(0, pos1);
        indicators[lineIndex].SetPosition(1, pos2);
        indicators[lineIndex].SetPosition(2, pos3);
    }
    public void CreateIndicator()
    {
        GameObject objects = Instantiate(indicator);
        objects.transform.position = new Vector3(objects.transform.position.x, objects.transform.position.y, 0);
        LineRenderer newIndicator = objects.GetComponent<LineRenderer>();
        newIndicator.gameObject.SetActive(false);
        objects.transform.parent = transform;
        indicators.Add(newIndicator);
    }
    public void IndicatorActiveState(bool state)
    {
        foreach (var a in indicators)
        {
            a.gameObject.SetActive(state);
        }
    }
}
