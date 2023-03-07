using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyUI : MonoBehaviour
{
    private Transform moneyImage;
    [SerializeField] private Ease easeType;
    private int counter;

    private void Awake()
    {
        moneyImage = GameObject.FindGameObjectWithTag("MoneyImage").transform;
    }

    private void Start()
    {
        StartCoroutine(nameof(MoneyMove));
    }

    private void OnEnable()
    {
        if (counter > 0)
        {
            StartCoroutine(nameof(MoneyMove));
        }
        counter++;
    }

    private IEnumerator MoneyMove()
    {
        yield return new WaitForSeconds(1);
        transform.DOMove(moneyImage.position, 2).SetEase(easeType).OnComplete(() =>
        {
            UpgradeManager.instance.UpdateMoney(UpgradeManager.instance.moneyValue);
        });
    }
}
