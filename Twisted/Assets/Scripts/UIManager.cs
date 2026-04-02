using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas worldCanvas;
    [SerializeField] private Sprite[] UISprites;
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
        cameraDirection.y = 0;
        Vector3 upVector = Vector3.up;
        Quaternion lookDirection = Quaternion.LookRotation(cameraDirection, upVector);
        worldCanvas.transform.rotation = lookDirection;
    }
}
