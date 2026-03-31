using UnityEngine;

public class SaveTransform : MonoBehaviour
{
    public void Load(TransformData ownData)
    {
        if (ownData == null)
            return;

        transform.position = ownData.Position;
        transform.eulerAngles = ownData.Rotation;
        transform.localScale = ownData.Scale;
    }
}
