using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [Header("Duck Flight Path")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    private NavMeshAgent _navMeshAgent;

    void OnEnable()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        if (_navMeshAgent != null && startPoint != null && endPoint != null)
        {
            // Set position and start moving to the endpoint
            transform.position = startPoint.position;
            _navMeshAgent.Warp(startPoint.position); // Ensures NavMeshAgent is synced
            _navMeshAgent.SetDestination(endPoint.position);
        }
        else
        {
            Debug.LogWarning("AI is missing required components or points.");
        }
    }

    void Update()
    {
        // Check if duck reached the endpoint
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            Debug.Log("Duck reached endpoint!");
            // Instead of Destroy, return to pool
            SpawnManager.Instance.ReturnToPool(gameObject);
        }
    }
}
