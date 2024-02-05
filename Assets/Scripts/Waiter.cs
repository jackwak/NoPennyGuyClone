using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Waiter : Worker
{
    private bool _isWaiting = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        MoveToNextPatrolPoint();
    }


    private void Update()
    {
        switch (_state)
        {
            case State.PATROL:
                if (AtEndOfPath() && !_isWaiting)
                {
                    _animator.SetBool("isWalking", false);

                    // if waiter between FoodSpawner
                    if (_currentPatrolIndex % _patrolPoints.Length == 0)
                    {
                        StartCoroutine(Wait(2f));
                    }
                    else
                    {
                        MoveToNextPatrolPoint();
                    }
                    Debug.Log("a");
                }
                break;
            case State.CATCH:

                break;
        }
    }

    public void MoveToNextPatrolPoint()
    {
        _currentPatrolIndex++;

        _agent.SetDestination(_patrolPoints[_currentPatrolIndex % _patrolPoints.Length].position);
        _animator.SetBool("isWalking", true);

    }

    private void OnDisable()
    {
        PlayerCatched -= OnPlayerCatched;
    }

    IEnumerator Wait(float time)
    {
        _isWaiting = true;

        yield return new WaitForSeconds(time);

        MoveToNextPatrolPoint();

        _isWaiting = false;
    }
}

