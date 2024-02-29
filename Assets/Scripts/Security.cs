using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Security : Worker
{
    [SerializeField]
    private Transform _lookPoint;
    private bool _isRotating = false;
    public Coroutine _coroutine;
    private Tween _tween;


    private void Start()
    {
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
                    float a = Random.Range(1, 3);
                    _animator.SetBool("isWalking", false);
                    _tween = transform.DOLookAt(_lookPoint.position, a).SetEase(Ease.Linear).OnComplete(() => _coroutine = StartCoroutine(Wait()));
                }
                break;
            case State.CATCH:

                break;
        }
        
    }

    IEnumerator Wait()
    {
        float a = Random.Range(3,5);

        yield return new WaitForSeconds(a);
        if(isActiveAndEnabled)
        MoveToNextPatrolPoint();
    }

    public override void OnPlayerCatched(Transform playerTransform)
    {
        //set catch state
        _state = State.CATCH;

        //stop agent
        _agent.Stop();

        //stop walk anim
        _animator.SetBool("isWalking", false);

        // stop coroutine
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        // kill tween for oncomplete is problem
        _tween.Kill();

        transform.DOLookAt(playerTransform.position, 0.5f, AxisConstraint.Y);
    }

}






