using UnityEngine;

public class SaveTransform : MonoBehaviour
{
    public void Load(TransformData ownData)
    {
        transform.position = ownData.Position;
        transform.eulerAngles = ownData.Rotation;
        transform.localScale = ownData.Scale;
    }
}
