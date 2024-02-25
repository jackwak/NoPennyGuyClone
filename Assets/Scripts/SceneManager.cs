using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    [SerializeField] private GameObject[] _scenes; // start scene is 0 index
    public GameObject StartScene;
    [SerializeField] private GameObject _currentScene;
    Cinemachine.CinemachineBrain _cinemachineBrain;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera _startCamera;

    public GameObject GetCurrentScene { get => _currentScene; }


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


        //initialize game
        _currentScene = Instantiate(_scenes[0]);
        _cinemachineBrain = GameObject.Find("Main Camera").GetComponent<Cinemachine.CinemachineBrain>();
        StartCoroutine(DelaySelectionData());
    }
    IEnumerator DelaySelectionData()
    {
        yield return new WaitForSeconds(.1f);

        SelectionController.Instance.InitializeSelectionData();

    }

    public void LoadLevel()
    {
        House house;
        if (SelectionController.Instance.GetCurrentHouse != null)
        {
            house = SelectionController.Instance.GetCurrentHouse;
        }
        else
        {
            house = SelectionController.Instance.Houses[SaveManager.Instance.LastOpenedHouseIndex];
        }

        // set to 0 the cinemachine brain's switch camera time
        _cinemachineBrain.enabled = false;
        _cinemachineBrain.m_DefaultBlend.m_Time = 0;
        _cinemachineBrain.enabled = true;

        // disappear start scene
        //Destroy(_currentScene);
        _currentScene.gameObject.SetActive(false);

        // appear loaded scene
        _currentScene = Instantiate(house.GetHouseScene);

        // levelın datalarını house a yükle
        LevelManager.Instance.InitializeLevel(house, _currentScene);
    }

    public void LoadStartScene()
    {
        /*House currentHouse = _currentScene.GetComponent<House>();

        //disappear current scene
        Destroy(currentHouse.GetHouseScene);*/
        Destroy(_currentScene);


        _currentScene = StartScene;

        _currentScene.SetActive(true);

        _startCamera = _currentScene.transform.Find("CM vcam1").GetComponent<Cinemachine.CinemachineVirtualCamera>(); // bunu start scene olşt sonra setle

        _startCamera.Priority++;

        // set to 2 the cinemachine brain's switch camera time
        _cinemachineBrain.enabled = false;
        _cinemachineBrain.m_DefaultBlend.m_Time = 2;
        _cinemachineBrain.enabled = true;
    }

    

}
