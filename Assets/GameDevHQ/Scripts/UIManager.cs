using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text aiKilledText;
    [SerializeField] private Text timerText;
    [SerializeField] private Text winLoseText;
    [SerializeField] private Text winText;

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

    // Add this method to display win screen
    public void ShowWinScreen()
    {
        if (winText != null)
            winText.text = "You Win! All Ducks Killed!";
    }

    // Add this method to display lose screen
    public void ShowLoseScreen()
    {
        if (winLoseText != null)
            winLoseText.text = "Game Over! Too Many Ducks Escaped!";
    }
}