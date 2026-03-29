using System.Collections;
using UnityEngine;

public class Corners : MonoBehaviour
{
    private CornersSO data;


    private Transform endDirectionController;
    private Transform frontDirectionController;
    private Vector3 endScaleAnimationScale;
    private Vector3 frontAnimationScale;
    private Vector3 endScale;
    private Vector3 frontScale;
    Coroutine animationCorner;
    public void InitializeCorner(CornersSO data)
    {
        endDirectionController = transform.Find("Front/End");
        frontDirectionController = transform.Find("Front");
        this.data = data;

        endScale = endDirectionController.localScale;
        frontScale = frontDirectionController.localScale;

        endScaleAnimationScale = new Vector3(
            endScale.x * data.AnimationScaleVector.x,
            endScale.y * data.AnimationScaleVector.y,
            endScale.z * data.AnimationScaleVector.z);

        frontAnimationScale = new Vector3(
            frontScale.x * data.AnimationScaleVector.x,
            frontScale.y * data.AnimationScaleVector.y,
            frontScale.z * data.AnimationScaleVector.z);
    }
    public void Hit(Vector3 hitDirection)
    {
        Transform currentController = null;
        Vector3 animationScale = Vector3.zero;
        Vector3 originalScale = Vector3.zero;
        float hitDot = Vector3.Dot(transform.forward, hitDirection);
        if (hitDot >= 0)
        {
            currentController = endDirectionController;
            animationScale = endScaleAnimationScale;
            originalScale = endScale;
        }
        else
        {
            currentController = frontDirectionController;
            animationScale = frontAnimationScale;
            originalScale = frontScale;
        }

        if (animationCorner != null)
        {
            StopCoroutine(animationCorner);
            endDirectionController.localScale = endScale;
            frontDirectionController.localScale = frontScale;
        }
        animationCorner = StartCoroutine(HitAnimation(currentController, animationScale, originalScale));

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
