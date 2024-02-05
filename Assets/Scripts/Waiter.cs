using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Waiter : Worker
{
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
                if (AtEndOfPath())
                {
                    _animator.SetBool("isWalking", false);
                    MoveToNextPatrolPoint();
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

        // waiter bir yere ulaþtýktan sonra 1 saniye beklesin sonra yürüsün
    }

    private void OnDisable()
    {
        PlayerCatched -= OnPlayerCatched;
    }
}
