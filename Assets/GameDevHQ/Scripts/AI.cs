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
    [SerializeField] private float hideTime = 3f;
    [SerializeField] private int maxHideStops = 3;
    [SerializeField] private float barrierDetectRange = 5f;
    [SerializeField] private float barrierSearchRadius = 10f;
    [SerializeField] private LayerMask barrierLayer;

    [Header("Points Settings")]
    [SerializeField] private int pointsOnDeath = 50;
    public UnityEvent<int> OnDeathPointsAwarded;

    [Header("Audio")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip completedTrackSound; // ✅ NEW: Sound when AI finishes path

    private AudioSource _audioSource;

    private enum AIState { Running, Hiding, Dead }
    [SerializeField] private AIState _currentState = AIState.Running;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Transform _currentBarrier;
    private bool isHiding = false;
    private int hideCount = 0;
    private GameManager _gameManager;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = FindObjectOfType<GameManager>();

        if (_navMeshAgent == null || startPoint == null || endPoint == null)
        {
            Debug.LogWarning("Missing NavMeshAgent or start/end points.");
            return;
        }

        transform.position = startPoint.position;
        _navMeshAgent.SetDestination(endPoint.position);
        _currentState = AIState.Running;
        hideCount = 0;
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
                break;
        }
    }

    void RunBehavior()
    {
        _animator?.SetBool("isRunning", true);
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(endPoint.position);

        if (hideCount < maxHideStops)
        {
            Transform bestBarrier = FindBestBarrier();
            if (bestBarrier != null)
            {
                _currentBarrier = bestBarrier;
                _navMeshAgent.SetDestination(_currentBarrier.position);
                _currentState = AIState.Hiding;
                hideCount++;
                Debug.Log($"Hiding at: {_currentBarrier.name} | Hide count: {hideCount}/{maxHideStops}");
            }
        }

        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && _currentState == AIState.Running)
        {
            Debug.Log("Duck reached endpoint!");
            ReturnToPoolAndSpawnNext();
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
            StopAllCoroutines();
            Die();
        }
    }

    private void Die()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("die");
        }

        _navMeshAgent.isStopped = true;

        if (deathSound != null)
        {
            Debug.Log("Playing death sound!");
            GameObject tempAudio = new GameObject("TempDeathSound");
            AudioSource source = tempAudio.AddComponent<AudioSource>();
            source.clip = deathSound;
            source.spatialBlend = 0f;
            source.volume = 1f;
            source.Play();
            Destroy(tempAudio, deathSound.length + 0.1f);
        }

        UIManager.Instance.AddScore(pointsOnDeath);
        UIManager.Instance.IncrementAIKilled();
        _gameManager.OnDuckKilled();

        StartCoroutine(ReturnToPoolAfterDelay(deathSound != null ? deathSound.length + 0.2f : 2f));
    }

    IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPoolAndSpawnNext();
    }

    void ReturnToPoolAndSpawnNext()
    {
        // ✅ Play sound when AI reaches destination
        if (completedTrackSound != null)
        {
            GameObject tempAudio = new GameObject("TempCompletedSound");
            AudioSource source = tempAudio.AddComponent<AudioSource>();
            source.clip = completedTrackSound;
            source.spatialBlend = 0f;
            source.volume = 1f;
            source.Play();
            Destroy(tempAudio, completedTrackSound.length + 0.1f);
        }

        _gameManager.OnDuckEscaped();
        SpawnManager.Instance.ReturnToPool(gameObject);
        SpawnManager.Instance.SpawnDuckAfterDelay(1f);
    }
}
