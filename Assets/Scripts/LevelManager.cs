using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static Cinemachine.DocumentationSortingAttribute;
using System.Net;
using System;

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
    public List<Food> _taskFoods = new List<Food>();
    public List<GameObject> TaskFoodGO = new List<GameObject>();
    public Image DoorArrowImage;

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
            TaskFoodGO.Add(Instantiate(_taskFoods[j].Prefab, FoodHolder.transform.GetChild(j)));
        }

        FoodTaskPanelMove();


    }

    public void TickTheFoodOnUI(string name)
    {
        // set last level index
        int level = _currentHouse.LastOpenedLevelIndex;

        // set task food count
        int taskFoodCount = _currentHouse.Levels[level].TaskFoods.Length;

        for (int i = 0; i < taskFoodCount; i++)
        {
            GameObject imageGO = FoodTaskPanel.transform.Find("Images").transform.GetChild(i).gameObject;
            string imageName = imageGO.GetComponent<Image>().sprite.name;

            name = name.Replace("(Clone)", "");

            GameObject tickGO = imageGO.transform.GetChild(0).gameObject;


            if (tickGO.activeSelf == true) return;

            if (imageName == name)
            {
                


                Vector3 foodTaskPanelPosition = FoodTaskPanel.transform.position;
                Vector3 foodTaskPanelScale = FoodTaskPanel.transform.localScale;
                FoodTaskPanel.transform.DOMove(new Vector3(Screen.width * .5f, Screen.height * .75f, 0), 1f);
                FoodTaskPanel.transform.DOScale(1.5f, 1f).SetEase(Ease.InOutBack).OnComplete(() =>
                {
                    tickGO.SetActive(true);
                    tickGO.transform.DOScale(.5f, 1f).From(Vector3.zero).SetEase(Ease.InOutBack).OnComplete(()=>
                    {
                        //Back to first position
                        FoodTaskPanel.transform.DOMove(foodTaskPanelPosition, 1f).SetDelay(1f);
                        FoodTaskPanel.transform.DOScale(foodTaskPanelScale, 1f).SetDelay(1f);

                        if (IsTasksCompleted())
                        {
                            // back to door ui
                            DoorArrowImage.gameObject.SetActive(true);

                            DoorArrowImage.rectTransform.DOLocalMoveX(DoorArrowImage.rectTransform.localPosition.x + .1f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
                        }
                    });
                });

                
            }
        }
    }

    private bool IsTasksCompleted()
    {
        int completedTaskCount = 0;

        // set last level index
        int level = _currentHouse.LastOpenedLevelIndex;

        // set task food count
        int taskFoodCount = _currentHouse.Levels[level].TaskFoods.Length;

        for (int i = 0; i < taskFoodCount; i++)
        {
            GameObject imageGO = FoodTaskPanel.transform.Find("Images").transform.GetChild(i).gameObject;

            if (imageGO.transform.GetChild(0).gameObject.activeSelf == true)
            {
                completedTaskCount++;
            }
        }
        if (completedTaskCount == taskFoodCount) return true;
        return false;
    }

    public void FoodTaskPanelMove()
    {
        //set to middle of camera the Food task panel
        Vector3 foodTaskPanelPosition = FoodTaskPanel.transform.position;
        Vector3 foodTaskPanelScale = FoodTaskPanel.transform.localScale;
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
        TaskFoodGO.Clear();
    }
}
