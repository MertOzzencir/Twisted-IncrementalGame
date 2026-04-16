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
    private WaiterManager waiterManager;
    private int currentBullets;
    private LineIndicator indicator;
    private ShootLogic logic;
    void Awake()
    {
        waiterManager = WaiterManager.Instance;
        indicator = GetComponent<LineIndicator>();
        indicator.CreateIndicator();
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
            WaiterController currentWaiter = waiterManager.ActiveWaiter();
            if (currentWaiter == null)
                return;
            Vector3 direction = logic.CalculateDirection(currentWaiter.transform.position);
            if (Vector3.Distance(currentWaiter.transform.position, logic.LastClickedPosition()) < 2f)
                return;
            currentWaiter.CurrentDirection = direction;
            float tempLength = 5f;
            if (Physics.Raycast(currentWaiter.transform.position, logic.CurrentDirection(), out RaycastHit hit, Mathf.Infinity, ballLayermask) && logic.CurrentDirection() != Vector3.zero)
            {
                Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.MousePosition());
                if (Physics.Raycast(ray, out RaycastHit ground))
                {
                    float groundFloat = Vector3.Distance(currentWaiter.transform.position, ground.point);
                    float wallFloat = Vector3.Distance(currentWaiter.transform.position, hit.point);
                    if (groundFloat < wallFloat)
                        tempLength = groundFloat;
                    else
                        tempLength = wallFloat;
                }

                indicator.DrawLine(new Vector3(currentWaiter.transform.position.x, currentWaiter.transform.position.y, currentWaiter.transform.position.z), new Vector3(
                    currentWaiter.transform.position.x + logic.CurrentDirection().normalized.x * (tempLength),
                    currentWaiter.transform.position.y,
                    currentWaiter.transform.position.z + logic.CurrentDirection().normalized.z * (tempLength)));
            }

        }
    }
    private void CalculateShootDirection(bool clickState)
    {
        shootFlag = clickState;
        if (!clickState)
        {
            foreach (var a in waiterManager.Waiters)
            {
                if (!a.IsActiveWorking)
                    continue;

                a.SetDirectionVector(a.CurrentDirection);
            }
            logic.ResetFirstClickPosition();
            indicator.IndicatorActiveState(false);
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

