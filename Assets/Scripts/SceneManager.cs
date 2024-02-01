using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    [SerializeField] private GameObject[] _scenes; // start scene is 0 index
    private GameObject _currentScene;


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
    }


    public void LoadLevel() // bu fonksşyonu karakter kamerasındayken oynaya basıldığında çalışıcak hale getir. bu haliyle sadece gördüğü sahneyi açar
    {// açık housela levelı save managera kaydet. eğer get curren house doluysa onun levelını yükle boşsa en sonki houseun levelını yükle
        House house = SelectionController.Instance.GetCurrentHouse;


        // disappear start scene
        _currentScene.SetActive(false);



        
        // appear loaded scene
        house.HouseScene.SetActive(true);

        // save loaded scene to current scene
        _currentScene = house.gameObject;
    }

    public void LoadStartScene()
    {
        //disappear current scene
        _currentScene.SetActive(false);






        // appear start scene
        _scenes[0].SetActive(true);

        // save loaded scene to current scene
        _currentScene = _scenes[0];
    }


}
