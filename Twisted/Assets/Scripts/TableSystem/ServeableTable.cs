using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ServeableTable : ServeMain
{
    [SerializeField] private List<HitAction> hitActions;
    [SerializeField] private TableChairs[] chairs;
    private UIManager UIManager;
    private int currentHitTimer;
    ServeableTableSO ownData;
    private List<CustomerBase> allCustomer = new List<CustomerBase>();
    public override void InitializeTable(ServeMainSO data, ServeSystemManager currentOwner)
    {

        ownData = data as ServeableTableSO;
        Debug.Log("Destructable local Initialize");
        base.InitializeTable(ownData, currentOwner);
        currentHitTimer = 0;

        UIManager = GetComponent<UIManager>();
        UIManager.SetUI(0);

    }
    public void SitOnTable(CustomerBase currentCustomer)
    {
        allCustomer.Add(currentCustomer);
        InitializeCustomer(currentCustomer);
    }

    private void InitializeCustomer(CustomerBase handleTheCustomer)
    {
        foreach (var a in chairs)
        {
            if (!a.IsOccoupied)
            {
                handleTheCustomer.transform.position = a.ChairObject.position;
                handleTheCustomer.transform.parent = a.ChairObject.transform;
                a.Customer = handleTheCustomer.gameObject;
                a.IsOccoupied = true;
                handleTheCustomer.SuccessfullyManagedSit();
                break;
            }
        }
    }

    public void Hit(Vector3 hitDirection)
    {
        if (!IsThereCustomerOnTable())
        {
            Debug.Log("There is no customer on table");
            return;
        }

        currentHitTimer++;
        HandleReciept(currentHitTimer % 4);
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
        currentHitTimer = 0;
        ServeToAllCustomerOnChairs();
    }
    public void ServeToAllCustomerOnChairs()
    {
        foreach (var a in chairs)
        {
            if (a.IsOccoupied)
            {
                Destroy(a.Customer);
                a.IsOccoupied = false;
            }
        }

    }
    public bool IsThereCustomerOnTable()
    {
        foreach (var a in chairs)
        {
            if (a.IsOccoupied)
                return true;
        }
        return false;
    }
}
[System.Serializable]
public class HitAction
{
    public int hitCount;
    public UnityEvent action;
}
[System.Serializable]
public class TableChairs
{
    public Transform ChairObject;
    public bool IsOccoupied;
    public GameObject Customer;
}
