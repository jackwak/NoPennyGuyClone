using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private GameObject FoodHolder;
    private House _currentHouse;
    private GameObject FoodTaskPanel;
    private List<Food> _taskFoods;

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
            FoodTaskPanel.transform.GetChild(i).gameObject.SetActive(true);
            _taskFoods[i] = _currentHouse.Levels[level].TaskFoods[i];
        }


        // initialize foods on the table
        for (int j = 0; j < FoodHolder.transform.childCount; j++)
        {
            Instantiate(_taskFoods[j].Prefab, FoodHolder.transform.GetChild(j));
        }


    }
}
