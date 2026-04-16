using UnityEngine;
using UnityEngine.PlayerLoop;

public class ShootLogic : MonoBehaviour
{
    Vector2 pressedPosition;
    private Vector3 direction;
    LayerMask groundMask;
    private Vector3 lastPosition;

    public Vector3 CalculateDirection(Vector3 ballPosition)
    {
        Vector3 currentWorld = FindPositionOnGround(groundMask);
        Vector3 ballPos = ballPosition;

        Vector3 aimDirection = currentWorld - ballPos;
        aimDirection.y = 0;
        direction = aimDirection.normalized;
        return direction;
    }
    public Vector3 CurrentDirection()
    {
        return direction;
    }
    public Vector3 LastClickedPosition()
    {
        return lastPosition;
    }
    private Vector3 FindPositionOnGround(LayerMask groundMask)
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.MousePosition());
        this.groundMask = groundMask;
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
        {
            lastPosition = hit.point;
            return hit.point;
        }
        Debug.Log("Shoot");
        return lastPosition;
    }
    public void InitializeLogic(LayerMask groundmask)
    {
        groundMask = groundmask;

    }
    public void ResetFirstClickPosition()
    {
        pressedPosition = Vector2.zero;

    }
    public Vector3 ReturnFirstClickPosition()
    {
        return pressedPosition;
    }
    public Vector3 CalculateReflectOnDirection(Vector3 normal)
    {
        normal.y = 0;
        return Vector3.Reflect(CurrentDirection(), normal).normalized;
    }

}
