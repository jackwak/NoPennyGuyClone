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
    [SerializeField] float _stoppingDistance;


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
                    Quaternion firstRotation = transform.rotation;
                    _tween = transform.DOLookAt(_lookPoint.position, a).SetEase(Ease.Linear).OnComplete(() => _coroutine = StartCoroutine(Wait(firstRotation)));
                }
                break;
            case State.CATCH:
                if (!_isRotating && AtEndOfPath())
                {
                    _isRotating = true;

                    LevelManager.Instance.ClearVariables();

                    LevelManager.Instance.OpenCatchPanel();
                }

                break;
        }
        
    }

    IEnumerator Wait(Quaternion firstRotation)
    {
        float a = Random.Range(5,7);

        int random = Random.Range(0, 2);
        if (random == 0)
        {
            // look around
            Quaternion diff = Quaternion.Inverse(transform.rotation) * firstRotation;

            Quaternion randomQuaternion = Quaternion.Euler(transform.rotation.x, Random.Range(0, diff.y), transform.rotation.z);


            transform.DORotateQuaternion(randomQuaternion, 1f).OnComplete(()=>
            {
                transform.DORotateQuaternion(Quaternion.Euler(transform.rotation.x, Random.Range(0, diff.y), transform.rotation.z), 1f);
            });
        }

        yield return new WaitForSeconds(a);
        if(isActiveAndEnabled)
        MoveToNextPatrolPoint();
    }

    public override void OnPlayerCatched(Transform playerTransform)
    {
        if(_state == State.CATCH) return;
        //agent component sets
        _agent.stoppingDistance = _stoppingDistance;

        _isRotating = true;

        //set catch state
        _state = State.CATCH;

        //stop agent
        _agent.isStopped = true;

        //stop walk anim
        _animator.SetBool("isWalking", false);

        transform.DOLookAt(playerTransform.position, 0.5f, AxisConstraint.Y).OnComplete(() =>
        {
            _agent.isStopped = false;
            _animator.SetBool("isWalking", true);
            _agent.SetDestination(playerTransform.position);
            _isRotating = false;
        });

        LevelManager.Instance.Player.GetComponent<PlayerController>().enabled = false;

        // stop coroutine
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        // kill tween for oncomplete is problem
        _tween.Kill();

        
    }

}






