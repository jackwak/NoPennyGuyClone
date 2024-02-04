using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
            Image foodImage = FoodTaskPanel.transform.Find("Images").transform.GetChild(i).GetComponent<Image>();
            foodImage.gameObject.SetActive(true);

            foodImage.sprite = _currentHouse.Levels[i].TaskFoods[i].Sprite;
            _taskFoods.Add(_currentHouse.Levels[level].TaskFoods[i]);
        }


        // initialize foods on the table
        for (int j = 0; j < FoodHolder.transform.childCount; j++)
        {
            Instantiate(_taskFoods[j].Prefab, FoodHolder.transform.GetChild(j));
        }

        FoodTaskPanelMove();


    }

    public void FoodTaskPanelMove()
    {
        //set to middle of camera the Food task panel
        Vector3 foodTaskPanelPosition = FoodTaskPanel.transform.position;
        Vector3 foodTaskPanelScale = FoodTaskPanel.transform.localScale;
        Vector3 middleOfCamera = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        FoodTaskPanel.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        FoodTaskPanel.transform.DOScale(2f, 1f).From(Vector3.zero).SetUpdate(true);


        //set time scale to 0 for goal anim
        Time.timeScale = 0;

        FoodTaskPanel.transform.DOMove(foodTaskPanelPosition, 1f).SetUpdate(true).SetDelay(2f);
        FoodTaskPanel.transform.DOScale(foodTaskPanelScale, 1f).SetUpdate(true).SetDelay(2f).OnComplete(()=>TutorialManager.Instance.GetTutorial(0));
    }


    public void ClearVariables()
    {
        _taskFoods.Clear();
        FoodTaskPanel = null;
        FoodHolder = null;
        _currentHouse = null;
    }

    void GetTutorial()
    {
    }
}
