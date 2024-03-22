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
    public List<Tutorial> Tutorial;
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

        //ResetTutorails();
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

    public void CheckTutorials()
    {
        for (int i = 0; i < Tutorial.Count; i++)
        {
            string tutName = "Tutorial" + (i + 1);
            if (!PlayerPrefs.HasKey(tutName))
            {
                PlayerPrefs.SetInt(tutName, 0);
            }
        }
    }

    public void ResetTutorails()
    {
        PlayerPrefs.DeleteAll();
    }

    public void GetTutorial(int id)
    {
        if (PlayerPrefs.GetInt("Tutorial" + (id + 1)) != 1)
        {
            PlayerPrefs.SetInt("Tutorial" + (id + 1), 1);
            Time.timeScale = 0;

            _currentTutorial = Tutorial[id];
            _currentTutorial.tutorialGO?.SetActive(true);
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

        if (_currentTutorial.step[id].HandRectTransforms.Count == 1)
        {
            Hand.position = _currentTutorial.step[id].HandRectTransforms[0].position;
            _currentTween = Hand.DOScale(Hand.localScale, 1f).From(Hand.localScale * 0.75f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true).SetEase(Ease.Linear);

            // maskeleme iþini yap etraf karanlýk olsun el aydýnlýk sonra da birden fazla hand positionlýyý yap
        }
        else
        {
            // move hand between positions
            Hand.position = _currentTutorial.step[id].HandRectTransforms[0].position;

            // Sequence oluþtur
            _handSequence = DOTween.Sequence();
            int handPositionCount = _currentTutorial.step[id].HandRectTransforms.Count;
            for (int i = 0; i < handPositionCount; i++)
            {
                // Her pozisyon için Hand.DOMove animasyonu Sequence'e eklenir
                _handSequence.Append(Hand.DOMove(_currentTutorial.step[id].HandRectTransforms[i].position, 0.1f).SetEase(Ease.Linear));
            }
            _handSequence.SetLoops(-1, LoopType.Yoyo).SetUpdate(true); // Sonsuz döngü
        }
    }

    public void NextTutorialStep()
    {
        _currentTutorialStepId++;
        if (_currentTutorialStepId < _currentTutorial.step.Count)
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

    public void SetTutorials(Transform tutorialPanelTransfrom)
    {
        //set
        Hand = tutorialPanelTransfrom.transform.Find("Hand Cursor").GetComponent<RectTransform>();
        for (int i = 0; i < tutorialPanelTransfrom.childCount - 1; i++)
        {
            Tutorial tutorial = new Tutorial();
            Tutorial.Add(tutorial);

            Transform tutorialTransform = tutorialPanelTransfrom.transform.Find("Tutorial" + (i + 1));

            tutorial.tutorialGO = tutorialTransform.gameObject;

            for (int j = 0; j < tutorialTransform.childCount; j++)
            {
                TutorialStep tutorialStep = new TutorialStep();
                tutorial.step.Add(tutorialStep);

                Transform tutorialStepTransform = tutorialTransform.transform.Find("Step" + (j + 1));
                tutorialStep.stepGO = tutorialStepTransform.gameObject;

                for (int h = 0; h < tutorialStepTransform.childCount - 1; h++)
                {
                    tutorialStep.HandRectTransforms.Add(tutorialStepTransform.GetChild(h).GetComponent<RectTransform>());
                }
            }
        }

    }
}
