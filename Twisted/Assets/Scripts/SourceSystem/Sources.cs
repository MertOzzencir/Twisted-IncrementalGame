using System.Collections;
using UnityEngine;

public abstract class Sources : MonoBehaviour
{
    public SourcesSO SourceType;

    public void Collect()
    {
        InventoryManager.Instance.AddSource(SourceType);
    }
}
public enum SourceType
{
    Coin,
    Ketchup
}
