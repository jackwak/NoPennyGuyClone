using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    [SerializeField] private GameObject[] _scenes; // start scene is 0 index
    private GameObject _currentScene;
    Cinemachine.CinemachineBrain _cinemachineBrain;


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
        _currentScene = _scenes[0];
        _cinemachineBrain = GameObject.Find("Main Camera").GetComponent<Cinemachine.CinemachineBrain>();
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
        _currentScene.SetActive(false);

        // levelın datalarını house a yükle
        LevelManager.Instance.InitializeLevel(house);





        // appear loaded scene
        house.GetHouseScene.SetActive(true);

        // save loaded scene to current scene
        _currentScene = house.gameObject;
    }

    public void LoadStartScene()
    {
        //

        //disappear current scene
        _currentScene.SetActive(false);






        // appear start scene
        _scenes[0].SetActive(true);

        // save loaded scene to current scene
        _currentScene = _scenes[0];
    }


}
