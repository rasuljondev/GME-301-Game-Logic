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

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        if (_navMeshAgent != null && startPoint != null && endPoint != null)
        {
            // Move the duck to the start position first
            transform.position = startPoint.position;

            // Then set its destination to the end point
            _navMeshAgent.SetDestination(endPoint.position);
        }
        else
        {
            Debug.LogWarning("Missing NavMeshAgent or points not assigned.");
        }
    }

    void Update()
    {
        // Check if duck reached end point
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            Debug.Log("Duck reached endpoint!");
            Destroy(gameObject); // Or trigger escape logic
        }
    }
}
