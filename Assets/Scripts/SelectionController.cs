using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using DG.Tweening;
using System;

public class SelectionController : MonoBehaviour
{
    [SerializeField] int _currentCameraIndex = 0;  // 0 is player cam

    [SerializeField] private List<House> _houses;
    [SerializeField] private House _currentHouse;
    [SerializeField] private CinemachineVirtualCamera _playerCamera;
    [SerializeField] private Button _nextButton, _previousButton, _playButton, _playButton2, _upgradeButton;
    [SerializeField] private RectTransform _arrowRectTransform;
    private List<Coroutine> _cameraSwitchCoroutines = new List<Coroutine>();

    private void Start()
    {
        _previousButton.gameObject.SetActive(false);

        _arrowRectTransform.DOAnchorPosY(4.7f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }


    public void NextCamera()
    {
        for (int i = 0; i < _cameraSwitchCoroutines.Count; i++) // gereksiz
        {
            StopCoroutine(_cameraSwitchCoroutines[i]);
        }

        // decrease previous camera's priority before next
        DecreasePreviousCameraPriority();

        // set next camera
        _currentCameraIndex++;

        // increase next camera's priority

        if (_currentCameraIndex == 0)
        {
            Coroutine a = StartCoroutine(DelayButtonSetActive(SetButtonsActiveOnPlayer, 2f, false));
            _cameraSwitchCoroutines.Add(a);

            
            _playerCamera.Priority++;
        }
        else
        {
            StartCoroutine(DelayButtonSetActive(SetButtonsActiveOnHouse, 2f, true));

            _currentHouse = _houses[_currentCameraIndex - 1];
            _currentHouse.IncreaseCameraPriority();
        }
    }

    public void PreviousCamera()
    {
        //stop all camera switch coroutines
        for (int i = 0; i < _cameraSwitchCoroutines.Count; i++) // gereksiz
        {
            StopCoroutine(_cameraSwitchCoroutines[i]);
        }

        // decrease previous camera's priority before next
        DecreasePreviousCameraPriority();

        // set next camera
        _currentCameraIndex--;

        // increase next camera's priority
        if (_currentCameraIndex == 0)
        {
            Coroutine a = StartCoroutine(DelayButtonSetActive(SetButtonsActiveOnPlayer, 2f,false));
            _cameraSwitchCoroutines.Add(a);

            _playerCamera.Priority++;
        }
        else
        {
            // hold switch camera coroutine for we need cancel
            Coroutine a = StartCoroutine(DelayButtonSetActive(SetButtonsActiveOnHouse, 2f,true));
            _cameraSwitchCoroutines.Add(a);


            _currentHouse = _houses[_currentCameraIndex - 1];
            _currentHouse.IncreaseCameraPriority();
        }
    }

    public void DecreasePreviousCameraPriority()
    {
        if (_currentCameraIndex == 0)
        {
            _playerCamera.Priority--;
        }
        else
        {
            _currentHouse.DecreaseCameraPriority();
        }
    }

    public void SetButtonsActiveOnHouse()
    {
        _playButton2.gameObject.SetActive(true);
    }

    public void SetButtonsActiveOnPlayer()
    {
        _playButton.gameObject.SetActive(true);
        _upgradeButton.gameObject.SetActive(true);
    }

    IEnumerator DelayButtonSetActive(Action action, float delayTime, bool isPerviousButtonActive)
    {
        _nextButton.gameObject.SetActive(false);
        _previousButton.gameObject.SetActive(false);
        _playButton.gameObject.SetActive(false);
        _playButton2.gameObject.SetActive(false);
        _upgradeButton.gameObject.SetActive(false);

        yield return new WaitForSeconds(delayTime);

        _previousButton.gameObject.SetActive(isPerviousButtonActive);
        if (_currentCameraIndex == _houses.Count)
        {
            _nextButton.gameObject.SetActive(false);
        }
        else
        {
            _nextButton.gameObject.SetActive(true);
        }
        action();
    }

}
