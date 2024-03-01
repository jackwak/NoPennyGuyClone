using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class Worker : MonoBehaviour
{
    [HideInInspector]
    public State _state = new State();
    public NavMeshAgent _agent;
    [HideInInspector]
    public Animator _animator;
    [HideInInspector]
    public float pathEndThreshold = 0.1f;
    [HideInInspector]
    public bool hasPath = false;
    [HideInInspector]
    public Transform[] _patrolPoints;
    public Transform _patrolPointsHolder;
    [HideInInspector]
    public int _currentPatrolIndex = 0;
    public Coroutine _coroutine;

    public static System.Action<Transform> PlayerCatched;

    private void OnEnable()
    {
        PlayerCatched += OnPlayerCatched;
    }

    private void Start()
    {
        _state = State.PATROL;

        for (int i = 0; i < _patrolPointsHolder.childCount; i++)
        {
            _patrolPoints[i] = _patrolPointsHolder.GetChild(i);
        }
    }

    public bool AtEndOfPath()
    {
        hasPath |= _agent.hasPath;
        if (hasPath && _agent.remainingDistance <= _agent.stoppingDistance + pathEndThreshold)
        {
            // Arrived
            hasPath = false;
            return true;
        }

        return false;
    }


    public abstract void OnPlayerCatched(Transform playerTransform);

    

    public enum State
    {
        PATROL,
        CATCH
    }

    private void OnDisable()
    {
        PlayerCatched -= OnPlayerCatched;
    }
}


