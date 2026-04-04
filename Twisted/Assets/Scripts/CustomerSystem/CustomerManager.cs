using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private CustomerBase CustomerPrefab;
    [SerializeField] private float spawnTimer;
    [SerializeField] private float maxSpawnableCustomerAmount;
    private List<CustomerBase> spawnedCustomers = new List<CustomerBase>();
    private float flagTimer;
    private float totalSpawnableCustomerAmount;
    private ChildColliderTriggerEvent childTrigger;
    void Awake()
    {
        childTrigger = GetComponentInChildren<ChildColliderTriggerEvent>();
        childTrigger.OnTriggerEnter += RuinedCustomer;
        totalSpawnableCustomerAmount = maxSpawnableCustomerAmount;
    }


    void Update()
    {
        flagTimer += Time.deltaTime;
        if (flagTimer > spawnTimer)
        {
            if (spawnedCustomers.Count < totalSpawnableCustomerAmount)
            {
                flagTimer = 0;
                SpawnCustomer();
            }
        }
    }
    public void SpawnCustomer()
    {
        CustomerBase currentCustomer = Instantiate(CustomerPrefab, transform.position, Quaternion.LookRotation(transform.forward));
        currentCustomer.OnSpawned(this);
        spawnedCustomers.Add(currentCustomer);
    }
    public void DeleteCustomerOnManager(CustomerBase customer)
    {
        spawnedCustomers.Remove(customer);
    }
    private void RuinedCustomer(GameObject customerGORef)
    {
        if (customerGORef.transform.TryGetComponent(out CustomerBase ruinedCustomer))
        {
            DeleteCustomerOnManager(ruinedCustomer);
            Destroy(ruinedCustomer.gameObject);
        }
    }

}
