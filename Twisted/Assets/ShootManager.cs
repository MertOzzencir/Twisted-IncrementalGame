using System.Collections.Generic;
using UnityEngine;
public class ShootManager : MonoBehaviour
{
    [SerializeField] private LayerMask ballLayermask;
    [SerializeField] private int totalBulletsPerReload;
    [SerializeField] private float reloadTimer;
    public static ShootManager Instance;
    private List<BallObject> totalBalls = new List<BallObject>();
    private int currentBullets;
    private LineIndicator indicator;
    private ShootLogic logic;
    void Awake()
    {
        indicator = GetComponent<LineIndicator>();
        logic = GetComponent<ShootLogic>();
        currentBullets = totalBulletsPerReload;
        if (Instance == null)
            Instance = this;
    }

    float timer;
    float currentReloadTimer;
    void Update()
    {

        if (logic.ReturnFirstClickPosition() != Vector2.zero)
        {
            logic.CalculateDirection();
            timer += Time.unscaledDeltaTime;
            Shader.SetGlobalFloat("_UnscaledTime", timer * 2f);
            if (logic.CurrentDirection() == Vector2.zero)
                return;

            for (int i = 0; i < totalBalls.Count; i++)
            {
                float tempLength = 5f;
                if (Physics.Raycast(totalBalls[i].transform.position, logic.CurrentDirection(), out RaycastHit hit, Mathf.Infinity, ballLayermask) && logic.CurrentDirection() != Vector2.zero)
                {
                    tempLength = hit.distance;
                    Vector3 reflected = logic.CalculateReflectOnDirection(hit.normal);
                    Vector3 hitPoint = hit.point;
                    hitPoint.z = 0;
                    reflected.z = 0;
                    indicator.DrawLine(i, new Vector3(totalBalls[i].transform.position.x, totalBalls[i].transform.position.y, 0), new Vector3(
                        totalBalls[i].transform.position.x + logic.CurrentDirection().x * (tempLength),
                        totalBalls[i].transform.position.y + logic.CurrentDirection().y * (tempLength),
                        0), hitPoint + reflected, tempLength);
                }
            }
        }
    }
    private void CalculateShootDirection(bool clickState)
    {
        if (clickState)
        {
            Time.timeScale = 0.03f;
            logic.FirstClickPosition();
        }
        else
        {
            Time.timeScale = 1f;
            int i = 0;
            foreach (var a in totalBalls)
            {
                a.SetDirectionVector(logic.CurrentDirection());
                i++;
            }
            logic.ResetFirstClickPosition();
            indicator.IndicatorActiveState(false);
            Debug.Log("Indicators Set to false in Mouse click");
        }
    }
    public void SubscribeToShootManager(BallObject ball)
    {
        totalBalls.Add(ball);
        indicator.CreateIndicator();
    }
    public void UnSubscribeToShootManager(BallObject ball)
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