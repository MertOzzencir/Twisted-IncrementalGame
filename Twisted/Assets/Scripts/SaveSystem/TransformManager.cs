using UnityEngine;

public class TransformManager : MonoBehaviour
{
    public SaveSO AllData;
    public void Save()
    {
        SaveTransform[] allTransform = FindObjectsByType<SaveTransform>(FindObjectsSortMode.None);
        foreach (var a in allTransform)
        {
            AllData.SaveData(UnityEditor.GlobalObjectId.GetGlobalObjectIdSlow(a.gameObject).ToString(), a.transform.position, a.transform.eulerAngles, a.transform.localScale);
        }
    }
    public void Load()
    {
        SaveTransform[] allTransform = FindObjectsByType<SaveTransform>(FindObjectsSortMode.None);
        foreach (var a in allTransform)
        {
            a.Load(AllData.GetData(UnityEditor.GlobalObjectId.GetGlobalObjectIdSlow(a.gameObject).ToString()));
        }
    }
    public void EraseAllData()
    {
        AllData.EraseAllData();
    }
    void OnEnable()
    {
        InputManager.OnSave += Save;
        InputManager.OnLoad += Load;
    }
    void OnDisable()
    {
        InputManager.OnSave -= Save;
        InputManager.OnLoad -= Load;
    }
}
