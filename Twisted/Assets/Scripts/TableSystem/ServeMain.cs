
using UnityEngine;

public class ServeMain : MonoBehaviour
{
    protected ServeMainSO data { get; set; }



    protected ServeSystemManager Owner;

    public virtual void InitializeTable(ServeMainSO data, ServeSystemManager currentOwner)
    {
        Debug.Log("Main");
        this.data = data;
        Owner = currentOwner;
    }
    public void OnDestroy()
    {
        Owner.DeleteCornerOnList(this);
    }

}
