using System;
using System.Collections.Generic;
using UnityEngine;

public class WaiterManager : MonoBehaviour
{
    public static WaiterManager Instance;
    [SerializeField] private WaiterController WaiterPrefab;
    [SerializeField] private int maxWaiter;
    public List<WaiterController> Waiters = new List<WaiterController>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        CreateWaiter();
    }

    private void PickWaiter(int obj)
    {
        for (int i = 0; i < Waiters.Count; i++)
        {
            bool state = obj == i + 1 ? true : false;
            Waiters[i].OnPickedFromManager(state);
        }
    }

    public void CreateWaiter()
    {
        for (int i = 0; i < maxWaiter; i++)
        {
            WaiterController currentWaiter = Instantiate(WaiterPrefab, transform.position + Vector3.right * i, Quaternion.Euler(90, 0, 0));
            currentWaiter.transform.parent = this.transform;
            Waiters.Add(currentWaiter);
        }

    }
    public WaiterController ActiveWaiter()
    {
        foreach (var a in Waiters)
        {
            if (a.IsActiveWorking)
                return a;
        }
        return null;
    }
    void OnEnable()
    {
        InputManager.OnNumPads += PickWaiter;
    }
}
