using UnityEngine;
using TMPro;

public class LeafManager : MonoBehaviour
{
    public static LeafManager Instance;
    public int leafCount = 0;
    public TextMeshProUGUI leafCounterText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Đây chính là hàm AddLeaf
    public void AddLeaf(int amount)
    {
        leafCount += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (leafCounterText != null)
            leafCounterText.text = "Leaves: " + leafCount;
    }
}
