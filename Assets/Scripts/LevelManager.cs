using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static Cinemachine.DocumentationSortingAttribute;
using System.Net;
using System;
using TMPro;
using Unity.VisualScripting;

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

    public GameObject FoodHolder;
    [SerializeField] private House _currentHouse;
    [SerializeField]private GameObject FoodTaskPanel;
    public List<Food> _taskFoods = new List<Food>();
    public List<GameObject> TaskFoodGO = new List<GameObject>();
    public Image DoorArrowImage;
    public List<Transform> MoneyTransforms;
    public Transform MoneyImage;
    public List<GameObject> MiniMoneysGO;
    public GameObject MoneyPanel;
    public GameObject EscapePanel;
    public List<BoxCollider> rangeMeshes;

    public Button StreetButton, NextButton;

    public TextMeshProUGUI MoneyText;

    public void InitializeLevel(House house, GameObject currentScene)
    {
        // current house missing hatas� ��nk� house start scenede ve biz onu destroyluyoruz

        // set loaded house
        _currentHouse = house;

        // set food Holder
        FoodHolder = currentScene.transform.Find("Food Holder").gameObject;

        FoodTaskPanel = currentScene.transform.Find("Canvas").transform.Find("Food Task Panel").gameObject;

        //set money panel
        MoneyPanel = currentScene.transform.Find("Canvas").transform.Find("Money Panel").gameObject;
        MoneyImage = MoneyPanel.transform.Find("Money Image");

        Transform miniMoneyHolder = MoneyPanel.transform.Find("Mini Money Holder");
        foreach (Transform item in miniMoneyHolder)
        {
            MiniMoneysGO.Add(item.gameObject);
        }

        Transform moneyPositions = MoneyPanel.transform.Find("Money Positions");
        foreach (Transform item in moneyPositions)
        {
            MoneyTransforms.Add(item);
        }

        // set door arrow image
        DoorArrowImage = GameObject.Find("World Space Canvas").transform.Find("Door Arrow").GetComponent<Image>();

        // set money text
        MoneyText = currentScene.transform.Find("Canvas").transform.Find("Money Panel").transform.Find("Money Image").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        //set escape panel
        EscapePanel = currentScene.transform.Find("Canvas").transform.Find("Escape Panel").gameObject;

        //set buttons
        StreetButton = EscapePanel.transform.Find("Street Button").GetComponent<Button>();
        NextButton = EscapePanel.transform.Find("Next Button").GetComponent<Button>();

        // set button onclick
        StreetButton.onClick.AddListener(OnClickedStreetButton);
        NextButton.onClick.AddListener(OnClickedNextButton);

        // set money count
        MoneyText.text = SaveManager.Instance.GetTotalMoney().ToString();


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
            GameObject a = Instantiate(_taskFoods[j].Prefab, FoodHolder.transform.GetChild(j));

            TaskFoodGO.Add(a);
        }

        Transform securitys = currentScene.transform.Find("Securities");
        Transform waiters = currentScene.transform.Find("Waiters");
        //initialize all range
        for (int i = 0; i < securitys.childCount; i++)
        {
             rangeMeshes.Add(securitys.GetChild(i).transform.Find("Range").GetComponent<BoxCollider>());
        }
        for (int i = 0; i < waiters.childCount; i++)
        {
             rangeMeshes.Add(waiters.GetChild(i).transform.Find("Range").GetComponent<BoxCollider>());
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

            if (tickGO.activeSelf == true) continue;


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
                        FoodTaskPanel.transform.DOScale(foodTaskPanelScale, 1f).SetDelay(1f).OnComplete(() =>
                        {
                            if (IsTasksCompleted())
                            {
                                // back to door ui
                                DoorArrowImage.gameObject.SetActive(true);

                                DoorArrowImage.rectTransform.DOLocalMoveX(DoorArrowImage.rectTransform.localPosition.x + .1f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

                                TextMeshProUGUI x = FoodTaskPanel.transform.Find("Exit Text").GetComponent<TextMeshProUGUI>();

                                x.gameObject.SetActive(true); // level bitti�nide kapat

                                x.DOFade(0, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                            }
                        });
                    });
                });
            }
        }
    }

    public bool IsTasksCompleted()
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
        // set last level index
        int level = _currentHouse.LastOpenedLevelIndex;

        // set task food count
        int taskFoodCount = _currentHouse.Levels[level].TaskFoods.Length;

        //Clear food task
        for (int i = 0; i < taskFoodCount; i++)
        {
            Image foodImage = FoodTaskPanel.transform.Find("Images").transform.GetChild(i).GetComponent<Image>();
            foodImage.gameObject.SetActive(false);
        }
        FoodTaskPanel.transform.Find("Exit Text").gameObject.SetActive(false);

        // clear range meshes
        rangeMeshes.Clear();


        // clear foods on the table
        for (int j = 0; j < FoodHolder.transform.childCount; j++)
        {
            string clone = "(Clone)";

            foreach (Transform item in FoodHolder.transform.GetChild(j))
            {
                if (item.name.Contains(clone))
                {
                    Destroy(item.gameObject);
                }
            }
        }


        _taskFoods.Clear();
        FoodTaskPanel = null;
        FoodHolder = null;
        //_currentHouse = null;
        TaskFoodGO.Clear();
        DoorArrowImage = null;
        MiniMoneysGO.Clear();
        MoneyText = null;
        NextButton = null;
        StreetButton = null;
        EscapePanel = null;
        MoneyPanel = null;
        MoneyTransforms.Clear();
    }


    public void MoneyAnimation(int miniMoneyCount)
    {
        for (int i = 0; i < miniMoneyCount; i++)
        {
            int index = i;

            MiniMoneysGO[index].SetActive(true);

            MiniMoneysGO[index].transform.position = MoneyTransforms[i].position;

            float random = UnityEngine.Random.Range(1f, 1.5f);
            float random2 = UnityEngine.Random.Range(1f, 1.5f);


            MiniMoneysGO[index].transform.DOScale(1, random).From(0f).SetEase(Ease.OutCirc);
            MiniMoneysGO[index].transform.DOMove(MoneyImage.transform.position, random2).SetEase(Ease.InBack).OnComplete(() =>
            {
                SaveManager.Instance.IncreaseEarnedMoney(10);
            });



        }
    }

    public void OpenEscapePanel()
    {
        EscapePanel.SetActive(true);

        EscapePanel.transform.DOScale(1, 1f).From(0).SetEase(Ease.OutBack).SetUpdate(true);
    }

    public void OnClickedNextButton()
    {
        Time.timeScale = 1f;

        int nextLevel = SaveManager.Instance.LastOpenedHouseIndex;

        EscapePanel?.SetActive(false);


        if (SelectionController.Instance.GetCurrentHouse.LevelCount < nextLevel)
        {
            // reset levels
            SaveManager.Instance.LastOpenedHouseIndex = 0;

            // get currrent house index
            int a = SelectionController.Instance.GetCurrentHouse._houseIndex;

            // set next house to current house
            SelectionController.Instance.SetCurrentHouse(a + 1);

            House house = SelectionController.Instance.GetCurrentHouse;

            SceneManager.Instance.LoadLevel();
        }
        else
        {
            House house = SelectionController.Instance.GetCurrentHouse;


            SceneManager.Instance.LoadLevel();
        }

        SceneManager.Instance.LoadLevel();
        
    }

    public void OnClickedStreetButton()
    {
        Time.timeScale = 1f;

        EscapePanel?.SetActive(false);

        _currentHouse.gameObject.transform.Find("CM vcam1").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority--;

        SelectionController.Instance.SetSelectionControllerToStart();

        SceneManager.Instance.LoadStartScene();

        //YEN�DEN AYNI LEVELA BA�LADI�INDA KARAKTERLER�N OLDU�U YERLER� AYNI AYARLA
    }

    public void IncreaseLastOpenedLevelIndex()
    {
        _currentHouse.LastOpenedLevelIndex++;
    }

    public void DisenableToConvexRangeMeshes()
    {
        foreach (var item in rangeMeshes)
        {
            item.isTrigger = false;
        }
    }
}
