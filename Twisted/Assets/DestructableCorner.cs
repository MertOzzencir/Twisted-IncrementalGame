using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DestructableCorner : Corners
{
    [SerializeField] private List<HitAction> hitActions;

    private OrderUIManager UIManager;
    private int currentHitTimer;
    private GameObject dropOnOrderComplete;
    private Coroutine animationCorner;

    public override void InitializeCorner(CornersSO data)
    {
        DestructableCornerSO dCornerData = data as DestructableCornerSO;
        Debug.Log("Destructable local Initialize");
        base.InitializeCorner(dCornerData);
        currentHitTimer = 0;

        UIManager = GetComponent<OrderUIManager>();
        UIManager.SetUI(0);
        dropOnOrderComplete = dCornerData.DropPrefab;
    }
    public void Hit(Vector3 hitDirection)
    {
        Debug.Log("Wall recived Hit");
        Transform currentController = null;
        Vector3 animationScale = Vector3.zero;
        Vector3 originalScale = Vector3.zero;
        float hitDot = Vector3.Dot(transform.forward, hitDirection);
        if (hitDot >= 0)
        {
            currentController = EndController;
            animationScale = EndAnimationScale;
            originalScale = EndOriginalScale;
        }
        else
        {
            currentController = FrontController;
            animationScale = FrontAnimationScale;
            originalScale = FrontOriginalScale;
        }

        if (animationCorner != null)
        {
            StopCoroutine(animationCorner);
            EndController.localScale = EndOriginalScale;
            FrontController.localScale = FrontOriginalScale;
        }
        animationCorner = StartCoroutine(HitAnimation(currentController, animationScale, originalScale));
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
        Instantiate(dropOnOrderComplete, transform.position, Quaternion.identity);
        Destroy(gameObject);
        currentHitTimer = 0;
    }
    private IEnumerator HitAnimation(Transform animationController, Vector3 animationScale, Vector3 originalScale)
    {
        while (Vector3.Distance(animationController.localScale, animationScale) > 0.001f)
        {
            animationController.localScale = Vector3.MoveTowards(animationController.localScale, animationScale, data.AnimationTimer * Time.deltaTime);
            yield return null;
        }
        animationController.localScale = animationScale;

        while (Vector3.Distance(animationController.localScale, originalScale) > 0.001f)
        {
            animationController.localScale = Vector3.MoveTowards(animationController.localScale, originalScale, data.AnimationTimer * Time.deltaTime);
            yield return null;
        }
        animationController.localScale = originalScale;
        animationCorner = null;
    }
}
[System.Serializable]
public class HitAction
{
    public int hitCount;
    public UnityEvent action;
}
