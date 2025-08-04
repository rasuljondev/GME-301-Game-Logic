using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text aiKilledText;
    [SerializeField] private Text timerText;

    private int score = 0;
    private int aiKilledCount = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void IncrementAIKilled()
    {
        aiKilledCount++;
        if (aiKilledText != null)
            aiKilledText.text = "AI Killed: " + aiKilledCount;
    }

    public void UpdateTimer(float seconds)
    {
        if (timerText != null)
            timerText.text = "Time: " + Mathf.RoundToInt(seconds).ToString();
    }
}
