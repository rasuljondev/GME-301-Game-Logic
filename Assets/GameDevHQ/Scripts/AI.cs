using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [Header("List of waypoints for duck to fly between")]
    [SerializeField] private List<Transform> _Waypoints;

    private NavMeshAgent _NavMeshAgent;

    void Start()
    {
        _NavMeshAgent = GetComponent<NavMeshAgent>();

        if (_NavMeshAgent != null && _Waypoints.Count > 0)
        {
            // Set a random starting destination from the waypoint list
            _NavMeshAgent.destination = _Waypoints[Random.Range(0, _Waypoints.Count)].position;
        }
        else
        {
            Debug.LogWarning("NavMeshAgent or Waypoints not properly assigned.");
        }
    }

    void Update()
    {
        // Optional: Check if agent reached destination and pick a new one
        if (!_NavMeshAgent.pathPending && _NavMeshAgent.remainingDistance <= _NavMeshAgent.stoppingDistance)
        {
            SetNewDestination();
        }
    }

    void SetNewDestination()
    {
        if (_Waypoints.Count > 0)
        {
            Vector3 nextPos = _Waypoints[Random.Range(0, _Waypoints.Count)].position;
            _NavMeshAgent.SetDestination(nextPos);
        }
    }
}
