using UnityEngine;
using UnityEngine.UI;

public class OrderUIManager : MonoBehaviour
{
    [SerializeField] private Canvas worldCanvas;
    [SerializeField] private Sprite[] UISprites;
    private Vector3 cameraDirection;
    void Start()
    {
        cameraDirection = Camera.main.transform.position - worldCanvas.transform.position;
        Vector3 upVector = transform.position - worldCanvas.transform.position;
        Quaternion lookDirection = Quaternion.LookRotation(cameraDirection, -upVector);
        worldCanvas.transform.rotation = lookDirection;
    }

    public void SetUI(int currentIndex)
    {
        worldCanvas.GetComponentInChildren<Image>().sprite = UISprites[currentIndex];
    }
}
