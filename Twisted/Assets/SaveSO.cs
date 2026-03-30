using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Save Data", menuName = "Create Save Data/New Data")]
public class SaveSO : ScriptableObject
{
    public List<TransformData> ObjectSaveDatas = new List<TransformData>();

    public void SaveData(string GlobalID, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        bool isFound = false;
        foreach (var a in ObjectSaveDatas)
        {
            if (a.GlobalID == GlobalID)
            {
                a.Position = position;
                a.Rotation = rotation;
                a.Scale = scale;
                isFound = true;
            }
        }
        if (!isFound)
        {
            ObjectSaveDatas.Add(new TransformData(GlobalID, position, rotation, scale));
        }
    }
    public TransformData GetData(string GlobalID)
    {
        foreach (var a in ObjectSaveDatas)
        {
            if (a.GlobalID == GlobalID)
            {
                return a;
            }
        }
        return null;
    }
}

[Serializable]
public class TransformData
{
    public string GlobalID;
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;
    public TransformData(string GlobalID, Vector3 pos, Vector3 rot, Vector3 scale)
    {
        this.GlobalID = GlobalID;
        Position = pos;
        Rotation = rot;
        Scale = scale;
    }
}
