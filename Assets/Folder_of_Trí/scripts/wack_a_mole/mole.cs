using UnityEngine;

public class Mole : MonoBehaviour
{
    public bool hasLetter = false;
    public char letter;

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
