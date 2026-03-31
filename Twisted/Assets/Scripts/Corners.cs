using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Corners : MonoBehaviour
{
    protected CornersSO data { get; set; }


    protected Transform EndController { get; set; }
    protected Transform FrontController { get; set; }
    protected Vector3 EndAnimationScale;
    protected Vector3 FrontAnimationScale;
    protected Vector3 EndOriginalScale;
    protected Vector3 FrontOriginalScale;
    public virtual void InitializeCorner(CornersSO data)
    {
        Debug.Log("Main");
        this.data = data;
        EndController = transform.Find("Front/End");
        FrontController = transform.Find("Front");
        EndOriginalScale = EndController.localScale;
        FrontOriginalScale = FrontController.localScale;

        EndAnimationScale = new Vector3(
            EndOriginalScale.x * data.AnimationScaleVector.x,
            EndOriginalScale.y * data.AnimationScaleVector.y,
            EndOriginalScale.z * data.AnimationScaleVector.z);

        FrontAnimationScale = new Vector3(
            FrontOriginalScale.x * data.AnimationScaleVector.x,
            FrontOriginalScale.y * data.AnimationScaleVector.y,
            FrontOriginalScale.z * data.AnimationScaleVector.z);
        EndController.GetComponentInChildren<Renderer>().material.color = data.Color;
    }

}
