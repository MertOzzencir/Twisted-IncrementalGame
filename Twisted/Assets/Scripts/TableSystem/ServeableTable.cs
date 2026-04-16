using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class ServeableTable : ServeMain
{
    [SerializeField] private List<HitAction> hitActions;
    [SerializeField] private TableChairs[] chairs;
    private UIManager UIManager;
    private int currentHitTimer;
    ServeableTableSO ownData;
    private List<CustomerBase> allCustomer = new List<CustomerBase>();
    private int totalHitCount;
    void Awake()
    {
        int lastBiggest = 0;
        foreach (var a in hitActions)
        {
            if (a.hitCount > lastBiggest)
            {
                lastBiggest = a.hitCount;
                totalHitCount = a.hitCount;
            }
        }
    }
    public override void InitializeTable(ServeMainSO data, ServeSystemManager currentOwner)
    {

        ownData = data as ServeableTableSO;
        Debug.Log("Destructable local Initialize");
        base.InitializeTable(ownData, currentOwner);
        currentHitTimer = 0;

        UIManager = GetComponent<UIManager>();
        UIManager.SetSprite(0);

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
                handleTheCustomer.SuccessfullyManagedSit(a);

                a.Customer = handleTheCustomer.gameObject;
                a.IsOccoupied = true;
                break;
            }
        }
    }

    public void Hit(Vector3 hitDirection)
    {
        if (!IsThereCustomerOnTable())
        {
            return;
        }

        currentHitTimer++;
        HandleReciept(currentHitTimer % (totalHitCount + 1));
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
        UIManager.SetSlider((float)currentHitTimer / (float)totalHitCount);
    }
    public void TakeOrder()
    {

        Debug.Log("Order Taken");
        UIManager.SetSprite(1);
    }
    public void CookReceipt()
    {
        UIManager.SetSprite(2);
        Debug.Log("Dish cooked");
    }
    public void ServeFood()
    {
        UIManager.SetSprite(0);
        Debug.Log("Dish Served");
        currentHitTimer = 0;
        ServeToAllCustomerOnChairs();
        UIManager.SetSlider(0);
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
