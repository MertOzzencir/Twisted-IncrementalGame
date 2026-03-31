using UnityEngine;

[CreateAssetMenu(fileName = "Corner Data", menuName = "Create Corner Data/New Data")]
public class CornersSO : ScriptableObject
{
    public Vector3 AnimationScaleVector;
    public float AnimationTimer;
    public Color Color;
    public GameObject Prefab;
}
