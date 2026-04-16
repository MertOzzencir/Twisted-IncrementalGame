using UnityEngine;

public abstract class CustomerBase : MonoBehaviour
{
    [SerializeField] private CustomerSO data;
    protected CustomerManager Owner;
    protected CustomerMovement movementController;
    public void Awake()
    {
        movementController = GetComponent<CustomerMovement>();
    }
    public void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            if (hit.transform.gameObject.TryGetComponent(out ServeableTable table))
            {
                table.SitOnTable(this);
            }
        }
    }
    public void OnDestroy()
    {
        data.DropPrefab.Collect();
    }
    public void OnSpawned(CustomerManager owner)
    {
        Owner = owner;
    }
    public void SuccessfullyManagedSit(TableChairs currentChair)
    {
        movementController.StopVelocity();
        Destroy(movementController);

        transform.position = currentChair.ChairObject.position;
        transform.parent = currentChair.ChairObject;
        Owner.DeleteCustomerOnManager(this);
    }

}
