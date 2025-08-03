using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AI : MonoBehaviour
{
    [Header("Duck Flight Path")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [Header("AI Settings")]
    [SerializeField] private float hideTime = 3f; // Fixed hide time
    [SerializeField] private int maxHideStops = 3; // Max number of hide stops
    [SerializeField] private float barrierDetectRange = 5f;
    [SerializeField] private float barrierSearchRadius = 10f;
    [SerializeField] private LayerMask barrierLayer; // For detecting barriers

    [Header("Points Settings")]
    [SerializeField] private int pointsOnDeath = 50;
    public UnityEvent<int> OnDeathPointsAwarded; // Event to notify points awarded

    private enum AIState { Running, Hiding, Dead }
    [SerializeField] private AIState _currentState = AIState.Running;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Transform _currentBarrier;
    private bool isHiding = false;
    private int hideCount = 0; // Track number of hide stops

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        if (_navMeshAgent == null || startPoint == null || endPoint == null)
        {
            Debug.LogWarning("Missing NavMeshAgent or start/end points.");
            return;
        }

        transform.position = startPoint.position;
        _navMeshAgent.SetDestination(endPoint.position);
    }

    void Update()
    {
        switch (_currentState)
        {
            case AIState.Running:
                RunBehavior();
                break;

            case AIState.Hiding:
                if (!isHiding)
                    StartCoroutine(HideRoutine());
                break;

            case AIState.Dead:
                // Die() is called once in TriggerDeath to avoid repeated calls
                break;
        }
    }

    void RunBehavior()
    {
        _animator?.SetBool("isRunning", true);
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(endPoint.position);

        // Only look for barriers if hide limit not reached
        if (hideCount < maxHideStops)
        {
            Transform bestBarrier = FindBestBarrier();
            if (bestBarrier != null)
            {
                _currentBarrier = bestBarrier;
                _navMeshAgent.SetDestination(_currentBarrier.position);
                _currentState = AIState.Hiding;
                hideCount++;
                Debug.Log($"Entering Hide State at barrier: {_currentBarrier.name}, Hide count: {hideCount}/{maxHideStops}");
            }
        }

        // Reached end
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && _currentState == AIState.Running)
        {
            Debug.Log("Duck reached endpoint!");
            Destroy(gameObject); // Optional: escape animation
        }
    }

    Transform FindBestBarrier()
    {
        Collider[] barriers = Physics.OverlapSphere(transform.position, barrierSearchRadius, barrierLayer);
        Transform bestBarrier = null;
        float closestDistanceToEnd = float.MaxValue;

        foreach (Collider barrier in barriers)
        {
            if (barrier.CompareTag("Barrier"))
            {
                Vector3 barrierPos = barrier.transform.position;
                float distanceToEnd = Vector3.Distance(barrierPos, endPoint.position);
                float distanceToAI = Vector3.Distance(transform.position, barrierPos);

                // Prioritize barriers closer to endpoint but within reach
                if (distanceToEnd < closestDistanceToEnd && distanceToAI <= barrierDetectRange)
                {
                    closestDistanceToEnd = distanceToEnd;
                    bestBarrier = barrier.transform;
                }
            }
        }

        return bestBarrier;
    }

    IEnumerator HideRoutine()
    {
        isHiding = true;
        _animator?.SetBool("isRunning", false);
        _navMeshAgent.isStopped = true;

        Debug.Log($"Hiding for {hideTime:F2} seconds.");
        yield return new WaitForSeconds(hideTime);

        isHiding = false;
        _currentState = AIState.Running;
        _navMeshAgent.SetDestination(endPoint.position);
        Debug.Log("Back to Running");
    }

    public void TriggerDeath()
    {
        if (_currentState != AIState.Dead)
        {
            _currentState = AIState.Dead;
            Die();
        }
    }

    void Die()
    {
        _animator?.SetTrigger("die");
        _navMeshAgent.isStopped = true;
        
        // Award points via UnityEvent
        OnDeathPointsAwarded?.Invoke(pointsOnDeath);
        Debug.Log($"Duck is Dead, awarded {pointsOnDeath} points");

        Destroy(gameObject, 2f); // Delay to let animation play
    }
}