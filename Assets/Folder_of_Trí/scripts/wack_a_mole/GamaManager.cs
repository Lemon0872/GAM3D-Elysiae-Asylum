using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Target Word Settings")]
    public string targetWord = "UNITY";   // từ khóa cần ghép
    public List<char> collectedLetters = new List<char>();

    [Header("Score Settings")]
    public int score = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CollectLetter(char letter)
    {
        collectedLetters.Add(letter);
        Debug.Log("Collected letter: " + letter);
        CheckWordComplete();
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }

    // Trả về chữ cái ngẫu nhiên trong số chưa thu thập
    public char GetNextNeededLetter()
    {
        List<char> remaining = targetWord.ToList()
            .Where(c => !collectedLetters.Contains(c))
            .ToList();

        if (remaining.Count > 0)
        {
            return remaining[Random.Range(0, remaining.Count)];
        }
        else
        {
            // fallback: nếu đã đủ hết thì random bất kỳ chữ trong từ
            return targetWord[Random.Range(0, targetWord.Length)];
        }
    }

    private void CheckWordComplete()
    {
        bool complete = targetWord.All(c => collectedLetters.Contains(c));

        if (complete)
        {
            Debug.Log("🔔🔔Word complete! Level passed!🔔🔔");
            LevelComplete();
        }
    }

    private void LevelComplete()
    {
        Debug.Log("Level Complete! You collected the word: " + targetWord);
        // TODO: load scene mới hoặc hiện UI thắng
    }
}
