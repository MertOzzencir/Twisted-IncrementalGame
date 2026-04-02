using UnityEngine;

public class WaiterAnimationController : MonoBehaviour
{
    private Animator anim;
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }
    [ContextMenu("Anim")]
    public void TriggerHit()
    {
        anim.Play("waiterRobot-hitanimation", 0, 0f);
    }
}
