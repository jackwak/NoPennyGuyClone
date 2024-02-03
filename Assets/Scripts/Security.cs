using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Security : Worker
{
    [SerializeField] 
    private Transform[] _patrolPoints;
    [SerializeField]
    private Transform _lookPoint;
    private NavMeshAgent _agent;
    private bool _isRotating = false;

    private int _currentPatrolIndex = 0;


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        MoveToNextPatrolPoint();
    }


    public void MoveToNextPatrolPoint()
    {
        _currentPatrolIndex++;

        _agent.SetDestination(_patrolPoints[_currentPatrolIndex].position);
        _isRotating = false;
    }

    private void Update()
    {
        if (!_isRotating &&HasReachedDestination())
        {
            _isRotating = true;
            transform.DOLookAt(_lookPoint.position, 1f).SetEase(Ease.InBounce).OnComplete(MoveToNextPatrolPoint);// D�N��TE SIKINTI VAR!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }
    }

    bool HasReachedDestination()
    {
        // Agent hareket halindeyse ve hedefe kalan mesafe, agent'�n durma mesafesinden az veya e�itse
        // ve agent'�n yolu tamamlanm��sa (pathPending false ise)
        return !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance && _agent.hasPath;
    }
}

