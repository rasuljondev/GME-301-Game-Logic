using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float timeSpent = 0f;

    // Add these fields for win/lose conditions
    private int totalDucksToSpawn = 10; // Matches SpawnManager's poolSize
    private int ducksKilled = 0;
    private int ducksEscaped = 0;
    private float maxEscapePercentage = 0.5f; // 50% (5 out of 10 ducks)
    private bool gameEnded = false;

    // Add this method to initialize game state
    public void InitializeGame(int totalDucks)
    {
        totalDucksToSpawn = totalDucks;
        ducksKilled = 0;
        ducksEscaped = 0;
        gameEnded = false;
    }

    // Add this method to handle duck killed
    public void OnDuckKilled()
    {
        if (gameEnded) return;
        ducksKilled++;
        CheckWinCondition();
    }

    // Add this method to handle duck escaped
    public void OnDuckEscaped()
    {
        if (gameEnded) return;
        ducksEscaped++;
        CheckLoseCondition();
    }

    // Add this method to check win condition
    private void CheckWinCondition()
    {
        if (ducksKilled >= totalDucksToSpawn)
        {
            gameEnded = true;
            UIManager.Instance.UpdateTimer(0f); // Stop timer display
            UIManager.Instance.ShowWinScreen(); // Display win message
            Debug.Log("Win! All ducks killed!");
        }
    }

    // Add this method to check lose condition
    private void CheckLoseCondition()
    {
        float escapePercentage = (float)ducksEscaped / totalDucksToSpawn;
        if (escapePercentage > maxEscapePercentage)
        {
            gameEnded = true;
            UIManager.Instance.UpdateTimer(0f); // Stop timer display
            UIManager.Instance.ShowLoseScreen(); // Display lose message
            Debug.Log("Lose! More than 50% of ducks escaped!");
        }
    }

    void Update()
    {
        if (!gameEnded) // Only update timer if game hasn't ended
        {
            timeSpent += Time.deltaTime;
            UIManager.Instance.UpdateTimer(timeSpent);
        }
    }
}