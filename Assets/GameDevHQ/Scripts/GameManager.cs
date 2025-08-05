using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float timeSpent = 0f;

    // Add these fields for win/lose conditions
    private int totalDucksToSpawn = 10; // Matches SpawnManager's poolSize
    private int ducksKilled = 0;
    private int ducksEscaped = 0;
    private float maxEscapePercentage = 0.2f; // 20% (2 out of 10 ducks)
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
            UIManager.Instance.ShowWinScreen(); // Display win screen
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
            UIManager.Instance.ShowLoseScreen(); // Display lose screen
            Debug.Log("Lose! Too many ducks escaped!");
        }
    }

    void Update()
    {
        timeSpent += Time.deltaTime;
        UIManager.Instance.UpdateTimer(timeSpent);
    }
}