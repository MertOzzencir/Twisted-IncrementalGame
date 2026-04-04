using System;
using UnityEngine;

public class CustomerMovement : MonoBehaviour
{
    [SerializeField] private float walkPower = 5f;
    public Rigidbody RB { get; set; }
    void Awake()
    {
        RB = GetComponent<Rigidbody>();
    }
    public virtual void FixedUpdate()
    {
        RB.linearVelocity = transform.forward * walkPower;
    }

}
