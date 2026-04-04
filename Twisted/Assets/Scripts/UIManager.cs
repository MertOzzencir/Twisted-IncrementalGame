using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas worldCanvas;
    [SerializeField] private Sprite[] UISprites;
    [Range(0f,1f)]
    private Vector3 cameraDirection;
    void Start()
    {
        LookAtCamera();
    }
    void Update()
    {
        LookAtCamera();
    }
    public void SetUI(int currentIndex)
    {
        worldCanvas.GetComponentInChildren<Image>().sprite = UISprites[currentIndex];
    }
    public void LookAtCamera()
    {
        cameraDirection = Camera.main.transform.position - worldCanvas.transform.position;
        Quaternion lookDirection = Quaternion.LookRotation(cameraDirection, Vector3.up);
        worldCanvas.transform.rotation = lookDirection;
    }
}
