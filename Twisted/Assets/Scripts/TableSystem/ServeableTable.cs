using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ServeableTable : ServeMain
{
    [SerializeField] private List<HitAction> hitActions;

    private UIManager UIManager;
    private int currentHitTimer;
    private Coroutine animationCorner;
    protected Vector3 EndOriginalScale;
    protected Vector3 FrontOriginalScale;
    public float AnimationTimer;
    ServeableTableSO ownData;
    public override void InitializeTable(ServeMainSO data, ServeSystemManager currentOwner)
    {

        ownData = data as ServeableTableSO;
        Debug.Log("Destructable local Initialize");
        base.InitializeTable(ownData, currentOwner);
        currentHitTimer = 0;

        UIManager = GetComponent<UIManager>();
        UIManager.SetUI(0);

    }
    public void Hit(Vector3 hitDirection)
    {
        HandleReciept(currentHitTimer % 3);
        currentHitTimer++;
    }
    private void HandleReciept(int currentHit)
    {
        foreach (var a in hitActions)
        {
            if (a.hitCount == currentHit)
            {
                a.action?.Invoke();
            }
        }
    }
    public void TakeOrder()
    {
        Debug.Log("Order Taken");
        UIManager.SetUI(1);
    }
    public void CookReceipt()
    {
        UIManager.SetUI(2);
        Debug.Log("Dish cooked");
    }
    public void ServeFood()
    {
        UIManager.SetUI(0);
        Debug.Log("Dish Served");
        ownData.DropPrefab.GetComponent<Sources>().Collect();
        Destroy(gameObject);
        currentHitTimer = 0;
    }

}
[System.Serializable]
public class HitAction
{
    public int hitCount;
    public UnityEvent action;
}
