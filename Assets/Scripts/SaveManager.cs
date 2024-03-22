using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public int LastOpenedHouseIndex = 0;
    public int EarnedMoney;

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
        PlayerPrefs.SetInt("MoneyCount", 0);
        if (!PlayerPrefs.HasKey("MoneyCount"))
        {
            PlayerPrefs.SetInt("MoneyCount", 0);
        }
    }


    public void IncreaseEarnedMoney(int money)
    {
        EarnedMoney += money;

        int currentMoney = PlayerPrefs.GetInt("MoneyCount");

        LevelManager.Instance.MoneyText.text = (currentMoney + EarnedMoney).ToString();
    }

    public void SaveMoney()
    {
        int currentMoney = PlayerPrefs.GetInt("MoneyCount");
        PlayerPrefs.SetInt("MoneyCount", currentMoney + EarnedMoney);

        EarnedMoney = 0;
    }

    public int GetTotalMoney()
    {
        return PlayerPrefs.GetInt("MoneyCount");
    }
}
