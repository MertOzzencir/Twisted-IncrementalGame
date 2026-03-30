using UnityEngine;

public class ShootLogic : MonoBehaviour
{
    Vector2 pressedPosition;
    private Vector2 direction;
    public void CalculateDirection()
    {
        Vector3 currentWorld = ScreenToWorld(InputManager.Instance.MousePosition());
        Vector3 pressedWorld = ScreenToWorld(pressedPosition);

        Vector3 aimDirection = currentWorld - pressedWorld;
        aimDirection.z = 0;
        direction = aimDirection.normalized;
    }
    public Vector2 CurrentDirection()
    {
        return direction;
    }

    public void FirstClickPosition()
    {
        pressedPosition = InputManager.Instance.MousePosition();
    }
    public void ResetFirstClickPosition()
    {
        pressedPosition = Vector2.zero;
    }
    public Vector2 ReturnFirstClickPosition()
    {
        return pressedPosition;
    }
    public Vector3 CalculateReflectOnDirection(Vector3 normal)
    {
        normal.z = 0;
        return Vector3.Reflect(CurrentDirection(), normal).normalized;
    }
    private Vector3 ScreenToWorld(Vector2 screenPos)
    {
        Vector3 pos = new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane);
        return Camera.main.ScreenToWorldPoint(pos);
    }
}
