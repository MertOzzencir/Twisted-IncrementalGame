using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas worldCanvas;
    [SerializeField] private Sprite[] UISprites;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject fillArea;
    [SerializeField] private Image spriteImage;
    [Range(0f, 1f)]
    private Vector3 cameraDirection;
    void Start()
    {
        fillArea.SetActive(false);
        LookAtCamera();
    }
    void Update()
    {
        LookAtCamera();
    }
    public void SetSprite(int currentIndex)
    {
        spriteImage.sprite = UISprites[currentIndex];
    }
    public void SetSlider(float sValue)
    {
        if (sValue == 0)
        {
            fillArea.SetActive(false);
        }
        else if (!fillArea.activeInHierarchy)
            fillArea.SetActive(true);

        slider.value = sValue;
    }
    public void LookAtCamera()
    {
        cameraDirection = Camera.main.transform.position - worldCanvas.transform.position;
        Quaternion lookDirection = Quaternion.LookRotation(cameraDirection, Vector3.up);
        worldCanvas.transform.rotation = lookDirection;
    }
    public void CanvasActivationOnScene(bool state)
    {
        worldCanvas.gameObject.SetActive(state);
    }
}
