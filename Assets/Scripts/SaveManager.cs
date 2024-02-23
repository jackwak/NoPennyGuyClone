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
        if (!PlayerPrefs.HasKey("MoneyCount"))
        {
            PlayerPrefs.SetInt("MoneyCount", 0);
        }
    }


    public void IncreaseEarnedMoney(int money)
    {
        EarnedMoney += money;

        LevelManager.Instance.MoneyText.text = EarnedMoney.ToString();
    }

    public void SaveMoney()
    {
        PlayerPrefs.SetInt("MoneyCount", EarnedMoney);

        EarnedMoney = 0;
    }

    public int GetTotalMoney()
    {
        return PlayerPrefs.GetInt("MoneyCount");
    }
}
