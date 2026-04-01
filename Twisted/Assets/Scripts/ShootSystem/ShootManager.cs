using System.Collections.Generic;
using UnityEngine;
public class ShootManager : MonoBehaviour
{
    [SerializeField] private LayerMask ballLayermask;
    [SerializeField] private int totalBulletsPerReload;
    [SerializeField] private float reloadTimer;
    [SerializeField] private LayerMask groundMask;
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
        logic.InitializeLogic(groundMask);
    }

    float timer;
    float currentReloadTimer;
    private bool shootFlag;
    void Update()
    {

        if (shootFlag)
        {
            timer += Time.unscaledDeltaTime;
            Shader.SetGlobalFloat("_UnscaledTime", timer * 2f);

            for (int i = 0; i < totalBalls.Count; i++)
            {
                logic.CalculateDirection(totalBalls[i].transform.position);
                float tempLength = 5f;
                if (Physics.Raycast(totalBalls[i].transform.position, logic.CurrentDirection(), out RaycastHit hit, Mathf.Infinity, ballLayermask) && logic.CurrentDirection() != Vector3.zero)
                {
                    tempLength = hit.distance;

                    indicator.DrawLine(i, new Vector3(totalBalls[i].transform.position.x, totalBalls[i].transform.position.y, totalBalls[i].transform.position.z), new Vector3(
                        totalBalls[i].transform.position.x + logic.CurrentDirection().x * (tempLength),
                        totalBalls[i].transform.position.y,
                        totalBalls[i].transform.position.z + logic.CurrentDirection().z * (tempLength)),
                        tempLength * 2);
                }
            }
        }
    }
    private void CalculateShootDirection(bool clickState)
    {
        if (clickState)
        {
            Time.timeScale = 0.03f;
            shootFlag = true;
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
            shootFlag = false;
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