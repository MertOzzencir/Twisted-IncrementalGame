using System;
using UnityEngine;

public class ChildColliderTriggerEvent : MonoBehaviour
{
    public event Action<GameObject> OnTriggerEnter;
    void OnCollisionEnter(Collision collision)
    {
        OnTriggerEnter?.Invoke(collision.gameObject);
    }
}
