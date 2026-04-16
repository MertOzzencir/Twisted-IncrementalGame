using System.Collections.Generic;
using UnityEngine;

public class LineIndicator : MonoBehaviour
{
    [SerializeField] private GameObject indicator;

    LineRenderer indicators;

    public void DrawLine(Vector3 pos1, Vector3 pos2)
    {

        if (!indicators.gameObject.activeSelf)
            indicators.gameObject.SetActive(true);

        indicators.SetPosition(0, pos1);
        indicators.SetPosition(1, pos2);
    }
    public void CreateIndicator()
    {
        GameObject objects = Instantiate(indicator);
        objects.transform.position = new Vector3(objects.transform.position.x, 0, objects.transform.position.z);
        LineRenderer newIndicator = objects.GetComponent<LineRenderer>();
        newIndicator.gameObject.SetActive(false);
        objects.transform.parent = transform;
        indicators = newIndicator;
    }
    public void IndicatorActiveState(bool state)
    {
        indicators.gameObject.SetActive(state);
    }
}
