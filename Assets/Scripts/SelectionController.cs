using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using DG.Tweening;
using System;

public class SelectionController : MonoBehaviour
{
    public static SelectionController Instance;
    [SerializeField] private int _houseCount;     
    
    [SerializeField] int _currentCameraIndex = 0;  // 0 is player cam

    [SerializeField] public List<House> Houses;
    [SerializeField] private House _currentHouse;
    [SerializeField] private CinemachineVirtualCamera _playerCamera;
    [SerializeField] private Button _nextButton, _previousButton, _playButton, _playButton2, _upgradeButton;
    [SerializeField] private RectTransform _arrowRectTransform;
    [SerializeField] private List<Coroutine> _cameraSwitchCoroutines = new List<Coroutine>();

    public House GetCurrentHouse { get => _currentHouse; }

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

    public void InitializeSelectionData()
    {
        Debug.Log("a");
        GameObject startScene = GameObject.Find("Start Scene(Clone)");

        SceneManager.Instance.StartScene = startScene;

        for (int i = 0; i < _houseCount; i++)
        {
            Houses.Add(startScene.transform.Find("House" + (i + 1)).GetComponent<House>());
        }

        //initialize house data


        _playerCamera = startScene.transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();

        Transform canvas = startScene.transform.Find("Canvas");

        _nextButton = canvas.transform.Find("Next Button").GetComponent<Button>();
        _playButton = canvas.transform.Find("Play Button").GetComponent<Button>();
        _playButton2 = canvas.transform.Find("Play Button2").GetComponent<Button>();
        _upgradeButton = canvas.transform.Find("Upgrade Button").GetComponent<Button>();
        _previousButton = canvas.transform.Find("Previous Button").GetComponent<Button>();

        //on clicks
        _nextButton.onClick.AddListener(NextCamera);
        _playButton2.onClick.AddListener(SceneManager.Instance.LoadLevel);
        _previousButton.onClick.AddListener(PreviousCamera);

        _arrowRectTransform = startScene.transform.Find("World Space Canvas").transform.GetChild(0).GetComponent<RectTransform>();


        _previousButton.gameObject.SetActive(false);

        _arrowRectTransform.DOAnchorPosY(4.7f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

        UIIdleAnimation(_nextButton.transform);
        UIIdleAnimation(_previousButton.transform);
        UIIdleAnimation(_playButton.transform);
        UIIdleAnimation(_upgradeButton.transform);
        UIIdleAnimation(_playButton2.transform);
    }

    public void SetCurrentHouse(int houseIndex)
    {
        _currentHouse = Houses[houseIndex - 1];
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

            _currentHouse = Houses[_currentCameraIndex - 1];
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
            // hold coroutine for we need cancel
            Coroutine a = StartCoroutine(DelayButtonSetActive(SetButtonsActiveOnHouse, 2f,true));
            _cameraSwitchCoroutines.Add(a);


            _currentHouse = Houses[_currentCameraIndex - 1];
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

    IEnumerator DelayButtonSetActive(Action action, float delayTime, bool PerviousButtonActive)
    {
        _nextButton.gameObject.SetActive(false);
        _previousButton.gameObject.SetActive(false);
        _playButton.gameObject.SetActive(false);
        _playButton2.gameObject.SetActive(false);
        _upgradeButton.gameObject.SetActive(false);

        yield return new WaitForSeconds(delayTime);
        if (PerviousButtonActive == true)
        {
            _previousButton.gameObject.SetActive(PerviousButtonActive);
        }

        if (_currentCameraIndex == Houses.Count)
        {
            _nextButton.gameObject.SetActive(false);
        }
        else
        {
            _nextButton.gameObject.SetActive(true);
        }
        action();
    }


    public void UIIdleAnimation(Transform t)
    {
        t.DOScale(t.localScale + new Vector3(.2f, .2f, .2f), .5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public void SetSelectionControllerToStart()
    {
        _currentCameraIndex = 0;

        _previousButton.gameObject.SetActive(false);
        _nextButton.gameObject.SetActive(true);
    }
}
