using UnityEngine;
using TMPro;

public class Mole : MonoBehaviour
{
    public bool hasLetter = false;
    public char letter;
    [Header("UI Reference")] 
    public TextMeshProUGUI letterText; // gắn từ prefab

    void Start()
    {
        if (hasLetter)
        {
            letterText.text = letter.ToString();
            letterText.gameObject.SetActive(true);
        }
        else
        {
            letterText.gameObject.SetActive(false);
        }
    }
    // Khi mole bị đập
    public void OnHit()
    {
        if (hasLetter)
        {
            GameManager.Instance.CollectLetter(letter);
        }
        else
        {
            GameManager.Instance.AddScore(1);
        }

        // Ẩn hoặc phá hủy mole sau khi bị đập
        Destroy(gameObject);
    }
}
