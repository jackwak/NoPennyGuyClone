using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class TutorialManager : MonoBehaviour
{
    public RectTransform Hand;

    public static TutorialManager Instance;
    [SerializeField] private Tutorial[] _tutorials;
    private Tutorial _currentTutorial;
    private Tween _currentTween;
    private Sequence _handSequence;
    private int _currentTutorialStepId = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < _tutorials.Length; i++)
        {
            string tutName = "Tutorial" + i;

            if (!PlayerPrefs.HasKey(tutName))
            {
                PlayerPrefs.SetInt(tutName, 0);
            }
        }
    }

    private void Update()
    {
        if (_currentTutorial != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                NextTutorialStep();
            }
        }
        
    }

    public void GetTutorial(int id)
    {
        if (PlayerPrefs.GetInt("Tutorial" + id) != 1)
        {
            PlayerPrefs.SetInt("Tutorial" + id, 1);
            Debug.Log(PlayerPrefs.GetInt("Tutorial0"));
            Time.timeScale = 0;

            _currentTutorial = _tutorials[id];
            _currentTutorial.tutorialGO.SetActive(true);

            GetTutorialStep(0);

            _currentTutorialStepId = 0;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void GetTutorialStep(int id)
    {
        Hand.gameObject.SetActive(true);
        _currentTutorial.step[id].stepGO.SetActive(true);

        //if there are tween or sequence stop
        _currentTween?.Kill();
        _handSequence?.Kill();

        if (_currentTutorial.step[id].HandRectTransforms.Length == 1)
        {
            Hand.position = _currentTutorial.step[id].HandRectTransforms[0].position;
            _currentTween = Hand.DOScale(Hand.localScale, 1f).From(Hand.localScale * 0.75f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true).SetEase(Ease.Linear);

            // maskeleme i�ini yap etraf karanl�k olsun el ayd�nl�k sonra da birden fazla hand positionl�y� yap
        }
        else
        {
            // move hand between positions
            Hand.position = _currentTutorial.step[id].HandRectTransforms[0].position;

            // Sequence olu�tur
            _handSequence = DOTween.Sequence();
            int handPositionCount = _currentTutorial.step[id].HandRectTransforms.Length;
            for (int i = 0; i < handPositionCount; i++)
            {
                // Her pozisyon i�in Hand.DOMove animasyonu Sequence'e eklenir
                _handSequence.Append(Hand.DOMove(_currentTutorial.step[id].HandRectTransforms[i].position, 0.1f).SetEase(Ease.Linear));
            }
            _handSequence.SetLoops(-1, LoopType.Yoyo).SetUpdate(true); // Sonsuz d�ng�
        }
    }

    public void NextTutorialStep()
    {
        _currentTutorialStepId++;
        if (_currentTutorialStepId < _currentTutorial.step.Length)
        {
            ClearTutorialStep(--_currentTutorialStepId);
            GetTutorialStep(_currentTutorialStepId);
        }
        else
        {
            ClearTutorial(--_currentTutorialStepId);
        }
    }

    private void ClearTutorialStep(int stepId)
    {
        _currentTutorial.step[stepId].stepGO.SetActive(false);

        _currentTween?.Kill();
        _handSequence?.Kill();
    }

    private void ClearTutorial(int stepId)
    {
        Time.timeScale = 1f;

        _currentTutorial.tutorialGO.SetActive(false);
        _currentTutorial.step[stepId].stepGO.SetActive(false);
        Hand.gameObject.SetActive(false);

        _currentTween?.Kill();
        _handSequence?.Kill();
        _currentTutorial = null;
        _currentTween = null;
        _handSequence = null;
    }
}
