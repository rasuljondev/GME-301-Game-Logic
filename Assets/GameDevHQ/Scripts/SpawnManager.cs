using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [Header("Duck Setup")]
    [SerializeField] private GameObject duckPrefab;
    [SerializeField] private Transform startPoint;
    [SerializeField] private int poolSize = 10;

    private Queue<GameObject> duckPool = new Queue<GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject duck = Instantiate(duckPrefab);
            duck.SetActive(false);
            duckPool.Enqueue(duck);
        }
    }

    void Start()
    {
        SpawnDuck();
    }

    public void SpawnDuck()
    {
        if (duckPool.Count > 0)
        {
            GameObject duck = duckPool.Dequeue();
            duck.transform.position = startPoint.position;
            duck.transform.rotation = Quaternion.identity;
            duck.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No ducks left in pool!");
        }
    }

    public void SpawnDuckAfterDelay(float delay)
    {
        StartCoroutine(SpawnAfterDelayRoutine(delay));
    }

    private IEnumerator SpawnAfterDelayRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnDuck();
    }

    public void ReturnToPool(GameObject duck)
    {
        duck.SetActive(false);
        duckPool.Enqueue(duck);
    }
}
