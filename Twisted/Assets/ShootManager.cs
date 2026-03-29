using System.Collections.Generic;
using UnityEngine;
public class ShootManager : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private LayerMask ballLayermask;
    public static ShootManager Instance;
    Vector2 pressedPosition;
    private Vector2 direction;
    private List<BallPhysic> totalBalls = new List<BallPhysic>();
    private List<LineRenderer> indicators = new List<LineRenderer>();
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private Vector3 ScreenToWorld(Vector2 screenPos)
    {
        Vector3 pos = new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane);
        return Camera.main.ScreenToWorldPoint(pos);
    }
    float timer;
    void Update()
    {
        if (pressedPosition != Vector2.zero)
        {
            Vector3 currentWorld = ScreenToWorld(InputManager.Instance.MousePosition());
            Vector3 pressedWorld = ScreenToWorld(pressedPosition);

            Vector3 aimDirection = (currentWorld - pressedWorld);
            aimDirection.z = 0;
            aimDirection = aimDirection.normalized;
            timer += Time.unscaledDeltaTime;
            direction = aimDirection;
            Shader.SetGlobalFloat("_UnscaledTime", timer * 2f);
            for (int i = 0; i < totalBalls.Count; i++)
            {
                float tempLength = 5f;
                if (Physics.Raycast(totalBalls[i].transform.position, aimDirection, out RaycastHit hit, Mathf.Infinity, ballLayermask))
                {
                    tempLength = hit.distance;
                    indicators[i].SetPosition(0, new Vector3(totalBalls[i].transform.position.x, totalBalls[i].transform.position.y, 0));
                    indicators[i].SetPosition(1, new Vector3(
                        totalBalls[i].transform.position.x + aimDirection.x * (tempLength),
                        totalBalls[i].transform.position.y + aimDirection.y * (tempLength),
                        0));
                    Vector3 hitnormalOffZ = hit.normal;
                    hitnormalOffZ.z = 0;
                    Vector3 reflected = Vector3.Reflect(aimDirection, hitnormalOffZ).normalized;
                    reflected.z = 0;
                    Vector3 hitPoint = hit.point;
                    hitPoint.z = 0;
                    indicators[i].SetPosition(2, hitPoint + reflected);
                    Debug.Log(indicators[i].gameObject.activeSelf);
                }
                else
                {
                    Debug.Log("sa?");
                    indicators[i].SetPosition(0, new Vector3(totalBalls[i].transform.position.x, totalBalls[i].transform.position.y, 0));
                    indicators[i].SetPosition(1, Vector3.zero);
                }

            }
        }
    }
    private void CalculateShootDirection(bool clickState)
    {
        if (clickState)
        {
            pressedPosition = InputManager.Instance.MousePosition();
            Time.timeScale = 0.03f;
            foreach (var a in indicators)
                a.gameObject.SetActive(true);
        }
        else
        {

            Time.timeScale = 1f;
            int i = 0;
            foreach (var a in totalBalls)
            {
                a.SetDirectionVector(direction);
                i++;
            }
            pressedPosition = Vector2.zero;
            foreach (var a in indicators)
                a.gameObject.SetActive(false);
        }
    }
    public void SubscribeToShootManager(BallPhysic ball)
    {
        totalBalls.Add(ball);
        GameObject objects = Instantiate(indicator);
        objects.transform.position = new Vector3(objects.transform.position.x, objects.transform.position.y, 0);
        LineRenderer newIndicator = objects.GetComponent<LineRenderer>();
        newIndicator.gameObject.SetActive(false);
        objects.transform.parent = transform;
        indicators.Add(newIndicator);
    }
    public void UnSubscribeToShootManager(BallPhysic ball)
    {
        totalBalls.Remove(ball);
    }
    void OnEnable()
    {
        InputManager.OnMouseLeftClick += CalculateShootDirection;
    }
    private void OnDisable()
    {
        InputManager.OnMouseLeftClick -= CalculateShootDirection;
    }
}