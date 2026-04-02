using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI sourceName;
    [SerializeField] private TextMeshProUGUI sourceAmount;
    public SourcesSO SourceOnSlot { get; set; }

    public void Add(SourcesSO source, string amount, string name, Sprite Icon)
    {
        SourceOnSlot = source;
        sourceAmount.text = amount;
        sourceName.text = name;
        icon.sprite = Icon;
        icon.color = new Color(icon.color.r, icon.color.b, icon.color.g, 1);
    }
}
