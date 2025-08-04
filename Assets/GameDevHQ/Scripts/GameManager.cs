using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float timeSpent = 0f;

    void Update()
    {
        timeSpent += Time.deltaTime;
        UIManager.Instance.UpdateTimer(timeSpent);
    }
}
