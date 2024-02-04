using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public RectTransform Hand;

    public static TutorialManager Instance;
    [SerializeField] private Tutorial[] _tutorials;
    private Tutorial _currentTutorial;
    private Tween _currentTween;
    private int _currentTutorialStepId = 0;

    private void Start()
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

    public void GetTutorial(int id)
    {
        _currentTutorial = _tutorials[id];

        GetTutorialStep(0);

        _currentTutorialStepId = 0;
    }

    public void GetTutorialStep(int id)
    {
        Hand.gameObject.SetActive(true);

        if (_currentTutorial.step[id].HandRectTransforms.Length == 1)
        {
            Hand.position = _currentTutorial.step[id].HandRectTransforms[0].position;

            _currentTween = Hand.DOScale(Hand.localScale, 1f).From(Hand.localScale * 0.75f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true).SetEase(Ease.Linear);

            // maskeleme iþini yap etraf karanlýk olsun el aydýnlýk sonra da birden fazla hand positionlýyý yap
        }
        else
        {
            // move hand between positions
        }
    }

    public void NextTutorialStep()
    {
        GetTutorialStep(++_currentTutorialStepId);
    }




}
