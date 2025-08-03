using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreText; // UI element for total score
    [SerializeField] private Text pointsPopupPrefab; // Prefab for "+50" pop-up
    [SerializeField] private Canvas canvas; // Reference to the Canvas
    [SerializeField] private float popupDuration = 1f; // Duration of pop-up animation
    [SerializeField] private float popupMoveDistance = 50f; // How far the pop-up moves up

    private int totalScore = 0;

    void Start()
    {
        if (scoreText == null || canvas == null)
        {
            Debug.LogWarning("ScoreManager: Missing scoreText or canvas reference.");
            return;
        }

        UpdateScoreText();
    }

    public void AddScore(int points)
    {
        totalScore += points; // Add 50 points for each AI killed
        UpdateScoreText();

        // Show pop-up for points
        if (pointsPopupPrefab != null)
        {
            StartCoroutine(ShowPointsPopup(points));
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = $"Score: {totalScore}";
    }

    IEnumerator ShowPointsPopup(int points)
    {
        // Instantiate pop-up text as a child of the canvas
        Text popup = Instantiate(pointsPopupPrefab, canvas.transform);
        popup.text = $"+{points}";

        // Position the pop-up at center of canvas
        RectTransform rectTransform = popup.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 0); // Center of canvas

        // Animate: move up and fade out
        float elapsedTime = 0f;
        Vector2 startPos = rectTransform.anchoredPosition;
        Color startColor = popup.color;

        while (elapsedTime < popupDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / popupDuration;

            // Move up
            rectTransform.anchoredPosition = startPos + new Vector2(0, popupMoveDistance * t);

            // Fade out
            popup.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(startColor.a, 0f, t));

            yield return null;
        }

        // Destroy pop-up
        Destroy(popup.gameObject);
    }
}