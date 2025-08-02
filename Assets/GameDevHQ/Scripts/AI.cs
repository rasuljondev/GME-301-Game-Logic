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

<<<<<<< HEAD
    void Start()
=======
    void OnEnable()
>>>>>>> 0c4bdc9 (WIP: local changes before pulling remote main)
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        if (_navMeshAgent != null && startPoint != null && endPoint != null)
        {
<<<<<<< HEAD
            // Move the duck to the start position first
            transform.position = startPoint.position;

            // Then set its destination to the end point
=======
            // Set position and start moving to the endpoint
            transform.position = startPoint.position;
            _navMeshAgent.Warp(startPoint.position); // Ensures NavMeshAgent is synced
>>>>>>> 0c4bdc9 (WIP: local changes before pulling remote main)
            _navMeshAgent.SetDestination(endPoint.position);
        }
        else
        {
<<<<<<< HEAD
            Debug.LogWarning("Missing NavMeshAgent or points not assigned.");
=======
            Debug.LogWarning("AI is missing required components or points.");
>>>>>>> 0c4bdc9 (WIP: local changes before pulling remote main)
        }
    }

    void Update()
    {
<<<<<<< HEAD
        // Check if duck reached end point
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            Debug.Log("Duck reached endpoint!");
            Destroy(gameObject); // Or trigger escape logic
=======
        // Check if duck reached the endpoint
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            Debug.Log("Duck reached endpoint!");
            // Instead of Destroy, return to pool
            SpawnManager.Instance.ReturnToPool(gameObject);
>>>>>>> 0c4bdc9 (WIP: local changes before pulling remote main)
        }
    }
}
