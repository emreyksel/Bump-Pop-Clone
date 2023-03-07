using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    public Image shopPanel;

    public Button[] buttons = new Button[2];
    public Button cloneCountButton;
    public Button ýncomeButton;

    public TextMeshProUGUI moneyText;
    private float money;
    [HideInInspector] public float moneyValue = 0.1f;

    [HideInInspector] public int ballSpawnCount = 10;

    private int cloneCountCost = 5;
    private int ýncomeCost = 6;
    private int raise = 2;

    private int cloneCountLevel = 1;
    private int ýncomeLevel = 1;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey(nameof(money)))
        {
            money = PlayerPrefs.GetFloat(nameof(money));
        }
        if (PlayerPrefs.HasKey(nameof(ballSpawnCount)))
        {
            ballSpawnCount = PlayerPrefs.GetInt(nameof(ballSpawnCount));
        }
        if (PlayerPrefs.HasKey(nameof(moneyValue)))
        {
            moneyValue = PlayerPrefs.GetFloat(nameof(moneyValue));
        }
        if (PlayerPrefs.HasKey(nameof(cloneCountLevel)))
        {
            cloneCountLevel = PlayerPrefs.GetInt(nameof(cloneCountLevel));
        }
        if (PlayerPrefs.HasKey(nameof(cloneCountCost)))
        {
            cloneCountCost = PlayerPrefs.GetInt(nameof(cloneCountCost));
        }
        if (PlayerPrefs.HasKey(nameof(ýncomeLevel)))
        {
            ýncomeLevel = PlayerPrefs.GetInt(nameof(ýncomeLevel));
        }
        if (PlayerPrefs.HasKey(nameof(ýncomeCost)))
        {
            ýncomeCost = PlayerPrefs.GetInt(nameof(ýncomeCost));
        }       
    }

    private void Start()
    {
        cloneCountButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = cloneCountCost.ToString();
        cloneCountButton.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = cloneCountLevel.ToString() + " LEVEL";

        ýncomeButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = ýncomeCost.ToString();
        ýncomeButton.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = ýncomeLevel.ToString() + " LEVEL";

        moneyText.text = money.ToString("0.0");
    }

    public void CloneCountButton()
    {
        moneyValue += moneyValue;
        PlayerPrefs.SetFloat(nameof(moneyValue), moneyValue);

        UpdatePanel(cloneCountCost, cloneCountButton);
        cloneCountCost += raise;
        PlayerPrefs.SetInt(nameof(cloneCountCost), cloneCountCost);

        cloneCountLevel++;
        PlayerPrefs.SetInt(nameof(cloneCountLevel), cloneCountLevel);

        cloneCountButton.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = cloneCountLevel.ToString() + " LEVEL";
    }

    public void IncomeButton()
    {
        ballSpawnCount++;
        PlayerPrefs.SetInt(nameof(ballSpawnCount), ballSpawnCount);

        UpdatePanel(ýncomeCost, ýncomeButton);
        ýncomeCost += raise;
        PlayerPrefs.SetInt(nameof(ýncomeCost), ýncomeCost);

        ýncomeLevel++;
        PlayerPrefs.SetInt(nameof(ýncomeLevel), ýncomeLevel);

        ýncomeButton.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = ýncomeLevel.ToString() + " LEVEL";
    }

    public void UpdateMoney(float value)
    {
        money += value;
        PlayerPrefs.SetFloat(nameof(money), money);
        moneyText.text = money.ToString("0.0");
        moneyText.transform.DOScale(Vector3.one * 1.25f, 0.25f).OnComplete(() => moneyText.transform.DOScale(Vector3.one, 0.25f));
    }

    public void UpdatePanel(int price, Button buton)
    {
        UpdateMoney(-price);
        price += raise;
        buton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = price.ToString();

        foreach (var button in buttons)
        {
            int.TryParse(buton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text, out int value); 

            if (value > money)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
        }
    }

    /*public void GetData()
    {
        ballSpawnCount = PlayerPrefs.GetInt(nameof(ballSpawnCount));
        money = PlayerPrefs.GetFloat(nameof(money));
        moneyValue = PlayerPrefs.GetFloat(nameof(moneyValue));

        cloneCountLevel = PlayerPrefs.GetInt(nameof(cloneCountLevel));
        cloneCountCost = PlayerPrefs.GetInt(nameof(cloneCountCost));
        ýncomeLevel = PlayerPrefs.GetInt(nameof(ýncomeLevel));
        ýncomeCost = PlayerPrefs.GetInt(nameof(ýncomeCost));
    }*/
}