using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class SelectionController : MonoBehaviour
{
    [SerializeField] int _currentCameraIndex = 0;  // 0 is player cam

    [SerializeField] private List<House> _houses;
    [SerializeField] private House _currentHouse;
    [SerializeField] private CinemachineVirtualCamera _playerCamera;
    [SerializeField] private Button _nextButton, _previousButton;

    private void Start()
    {
        _previousButton.gameObject.SetActive(false);
    }

    public void NextCamera()
    {

        // decrease previous camera's priority before next
        if (_currentCameraIndex == 0)
        {
            _playerCamera.Priority--;
        }
        else
        {
            _currentHouse.DecreaseCameraPriority();
        }

        // set next camera
        _currentCameraIndex++;

        // increase next camera's priority

        if (_currentCameraIndex == 0) // bu gereksiz 
        {
            _previousButton.gameObject.SetActive(false);
            _playerCamera.Priority++;
        }
        else
        {
            _currentHouse = _houses[_currentCameraIndex - 1];
            _previousButton.gameObject.SetActive(true);
            _currentHouse.IncreaseCameraPriority();
        }

        // check is camera last?
        if (_currentCameraIndex == _houses.Count)
        {
            _nextButton.gameObject.SetActive(false);
        }

    }

    public void PreviousCamera()
    {

        // decrease previous camera's priority before next
        if (_currentCameraIndex == 0)
        {
            _playerCamera.Priority--;
        }
        else
        {
            _currentHouse.DecreaseCameraPriority();
        }

        // set next camera
        _currentCameraIndex--;

        // increase next camera's priority
        if (_currentCameraIndex == 0)
        {
            _previousButton.gameObject.SetActive(false);
            _playerCamera.Priority++;
        }
        else
        {
            _currentHouse = _houses[_currentCameraIndex - 1];
            _previousButton.gameObject.SetActive(true);
            _currentHouse.IncreaseCameraPriority();
        }

        // setactive true for if we set it false
        _nextButton.gameObject.SetActive(true);
    }

}
