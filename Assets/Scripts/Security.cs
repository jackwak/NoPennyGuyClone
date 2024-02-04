using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Security : Worker
{
    private State _state = new State();
    [SerializeField]
    private Transform[] _patrolPoints;
    [SerializeField]
    private Transform _lookPoint;
    private NavMeshAgent _agent;
    private bool _isRotating = false;
    private Animator _animator;

    private int _currentPatrolIndex = 0;

    public float pathEndThreshold = 0.1f;
    private bool hasPath = false;

    //Actions
    public static System.Action PlayerCatched;

    private void OnEnable()
    {
        PlayerCatched += OnPlayerCatched;
    }

    private void Start()
    {
        _state = State.PATROL;
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        MoveToNextPatrolPoint();
    }


    public void MoveToNextPatrolPoint()
    {
        _currentPatrolIndex++;

        _agent.SetDestination(_patrolPoints[_currentPatrolIndex % _patrolPoints.Length].position);
        _animator.SetBool("isWalking", true);
        _isRotating = false;
    }

    private void Update()
    {
        switch (_state)
        {
            case State.PATROL:
                if (!_isRotating && AtEndOfPath())
                {
                    _isRotating = true;
                    int a = Random.Range(1, 4);
                    _animator.SetBool("isWalking", false);
                    transform.DOLookAt(_lookPoint.position, a).SetEase(Ease.Linear).OnComplete(() => StartCoroutine(Wait()));
                }
                break;
            case State.CATCH:

                break;
        }
        
    }

    bool AtEndOfPath()
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

    IEnumerator Wait()
    {
        int a = Random.Range(3,5);

        yield return new WaitForSeconds(a);

        MoveToNextPatrolPoint();
    }

    public void OnPlayerCatched()
    {
        //set catch state
        _state = State.CATCH;
    }

    private void OnDisable()
    {
        PlayerCatched -= OnPlayerCatched;
    }
}

enum State
{
    PATROL,
    CATCH
}




