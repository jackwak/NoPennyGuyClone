using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

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

    private GameObject FoodHolder;
    private House _currentHouse;
    private GameObject FoodTaskPanel;
    private List<Food> _taskFoods = new List<Food>();

    public void InitializeLevel(House house)
    {
        // set loaded house
        _currentHouse = house;

        // set food Holder
        FoodHolder = _currentHouse.GetHouseScene.transform.Find("Food Holder").gameObject;

        FoodTaskPanel = _currentHouse.GetHouseScene.transform.Find("Canvas").transform.Find("Food Task Panel").gameObject;

        // appear house scene
        _currentHouse.GetHouseScene.gameObject.SetActive(true);

        // set last level index
        int level = _currentHouse.LastOpenedLevelIndex;

        // set task food count
        int taskFoodCount = _currentHouse.Levels[level].TaskFoods.Length;

        // initialize task food ui
        for (int i = 0; i < taskFoodCount; i++)
        {
            Image foodImage = FoodTaskPanel.transform.GetChild(i).GetComponent<Image>();
            foodImage.gameObject.SetActive(true);

            foodImage.sprite = _currentHouse.Levels[i].TaskFoods[i].Sprite;
            _taskFoods.Add(_currentHouse.Levels[level].TaskFoods[i]);
        }


        // initialize foods on the table
        for (int j = 0; j < FoodHolder.transform.childCount; j++)
        {
            Instantiate(_taskFoods[j].Prefab, FoodHolder.transform.GetChild(j));
        }


    }


    public void ClearVariables()
    {
        _taskFoods.Clear();
        FoodTaskPanel = null;
        FoodHolder = null;
        _currentHouse = null;
    }



}
