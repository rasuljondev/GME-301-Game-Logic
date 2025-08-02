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
        // Singleton enforcement
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // Create duck pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject duck = Instantiate(duckPrefab);
            duck.SetActive(false);
            duckPool.Enqueue(duck);
        }
    }

    void Start()
    {
        // Spawn one AI duck at start
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

    public void ReturnToPool(GameObject duck)
    {
        duck.SetActive(false);
        duckPool.Enqueue(duck);
    }
}
