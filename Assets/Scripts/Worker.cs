using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Worker : MonoBehaviour
{
    [HideInInspector]
    public State _state = new State();
    [HideInInspector]
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

    public static System.Action PlayerCatched;

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

    public void OnPlayerCatched()
    {
        //set catch state
        _state = State.CATCH;
        Debug.Log("Catch State!");
    }

    private void OnDisable()
    {
        PlayerCatched -= OnPlayerCatched;
    }

    public enum State
    {
        PATROL,
        CATCH
    }

}


