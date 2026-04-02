using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
public class ShootManager : MonoBehaviour
{
    [SerializeField] private LayerMask ballLayermask;
    [SerializeField] private int totalBulletsPerReload;
    [SerializeField] private float reloadTimer;
    [SerializeField] private LayerMask groundMask;
    public static ShootManager Instance;
    private List<WaiterManagment> totalBalls = new List<WaiterManagment>();
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
    private bool shootFlag;
    void Update()
    {

        if (shootFlag)
        {
            timer += Time.unscaledDeltaTime;
            Shader.SetGlobalFloat("_UnscaledTime", timer * 2f);

            for (int i = 0; i < totalBalls.Count; i++)
            {
                Transform currentWaiter = totalBalls[i].Waiter.transform;
                totalBalls[i].CurrentDirection = logic.CalculateDirection(currentWaiter.position);
                float tempLength = 5f;
                if (Physics.Raycast(currentWaiter.position, logic.CurrentDirection(), out RaycastHit hit, Mathf.Infinity, ballLayermask) && logic.CurrentDirection() != Vector3.zero)
                {
                    Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.MousePosition());
                    if (Physics.Raycast(ray, out RaycastHit ground))
                    {
                        float groundFloat = Vector3.Distance(currentWaiter.position, ground.point);
                        float wallFloat = Vector3.Distance(currentWaiter.position, hit.point);
                        if (groundFloat < wallFloat)
                            tempLength = groundFloat;
                        else
                            tempLength = wallFloat;
                    }

                    indicator.DrawLine(i, new Vector3(currentWaiter.position.x, currentWaiter.position.y, currentWaiter.position.z), new Vector3(
                        currentWaiter.position.x + logic.CurrentDirection().normalized.x * (tempLength),
                        currentWaiter.position.y,
                        currentWaiter.position.z + logic.CurrentDirection().normalized.z * (tempLength)));
                }

            }
        }
    }
    private void CalculateShootDirection(bool clickState)
    {
        if (clickState)
        {
            shootFlag = true;
        }
        else
        {
            int i = 0;
            foreach (var a in totalBalls)
            {
                a.Waiter.SetDirectionVector(a.CurrentDirection);
                i++;
            }
            logic.ResetFirstClickPosition();
            indicator.IndicatorActiveState(false);
            shootFlag = false;
        }
    }
    public void SubscribeToShootManager(WaiterManager ball)
    {
        WaiterManagment newWaiter = new WaiterManagment(ball, Vector3.zero);
        totalBalls.Add(newWaiter);
        indicator.CreateIndicator();
    }
    public void UnSubscribeToShootManager(WaiterManager ball)
    {
        foreach (var a in totalBalls)
        {
            if (a.Waiter == ball)
            {
                totalBalls.Remove(a);
                break;
            }
        }
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

public class WaiterManagment
{
    public WaiterManager Waiter;
    public Vector3 CurrentDirection;
    public WaiterManagment(WaiterManager newWaiter, Vector3 currentDirection)
    {
        Waiter = newWaiter;
        CurrentDirection = currentDirection;
    }
}