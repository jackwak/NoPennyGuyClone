using DG.Tweening;
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

        PlayerCatched += OnPlayerCatched;
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
                        _coroutine = StartCoroutine(Wait(2f));
                    }
                    else
                    {
                        MoveToNextPatrolPoint();
                    }
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


    IEnumerator Wait(float time)
    {
        _isWaiting = true;

        yield return new WaitForSeconds(time);

        MoveToNextPatrolPoint();

        _isWaiting = false;
    }

    public override void OnPlayerCatched(Transform playerTransform)
    {
        //set catch state
        _state = State.CATCH;

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _agent.Stop();


        //set catched anim
        _animator.SetBool("isWalking", false);
        _animator.Play("Idle"); // not this

        transform.DOLookAt(playerTransform.position, 0.5f, AxisConstraint.Y);

        
    }
}

