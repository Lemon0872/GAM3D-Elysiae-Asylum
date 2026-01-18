using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton để dễ gọi từ Mole.cs
    public static GameManager Instance;

    [Header("Target Word Settings")]
    public string targetWord = "UNITY";   // từ khóa cần ghép
    private List<char> collectedLetters = new List<char>();

    [Header("Score Settings")]
    public int score = 0;

    void Awake()
    {
        // Thiết lập Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Gọi khi mole bị đập và có chữ cái
    public void CollectLetter(char letter)
    {
        collectedLetters.Add(letter);
        Debug.Log("Collected letter: " + letter);

        CheckWordComplete();
    }

    // Gọi khi mole bị đập nhưng không có chữ cái
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }

    // Trả về chữ cái cần thiết tiếp theo (chưa thu thập)
    public char GetNextNeededLetter()
    {
        foreach (char c in targetWord)
        {
            if (!collectedLetters.Contains(c))
            {
                return c; // trả về chữ cái chưa có
            }
        }
        // Nếu đã đủ hết thì trả về chữ bất kỳ (fallback)
        return targetWord[Random.Range(0, targetWord.Length)];
    }

    // Kiểm tra đã đủ chữ cái để ghép từ chưa
    private void CheckWordComplete()
    {
        bool complete = true;
        foreach (char c in targetWord)
        {
            if (!collectedLetters.Contains(c))
            {
                complete = false;
                break;
            }
        }

        if (complete)
        {
            Debug.Log("Word complete! Level passed!");
            LevelComplete();
        }
    }

    private void LevelComplete()
    {
        // Xử lý qua màn (ví dụ load scene mới, hiện UI thắng)
        Debug.Log("Level Complete! You collected the word: " + targetWord);
    }
}
