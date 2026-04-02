using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Corners : MonoBehaviour
{
    protected CornersSO data { get; set; }


    protected Transform EndController { get; set; }
    protected Transform FrontController { get; set; }
    protected Vector3 EndAnimationScale;
    protected Vector3 FrontAnimationScale;
    protected CornerManager Owner;

    public virtual void InitializeCorner(CornersSO data, CornerManager currentOwner)
    {
        Debug.Log("Main");
        this.data = data;
        Owner = currentOwner;
        EndController = transform.Find("Front/End");
        FrontController = transform.Find("Front");

        EndController.GetComponentInChildren<Renderer>().material.color = data.Color;
    }
    public void OnDestroy()
    {
        Owner.DeleteCornerOnList(this);
    }

}
