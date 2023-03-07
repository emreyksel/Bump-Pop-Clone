using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject winPanel;
    public GameObject failPanel;
    public GameObject comboPanel;

    public Image fillImage;
    public GameObject finishLine;
    private float fullDistance;

    public TextMeshProUGUI ballText;

    [HideInInspector] public int ballCounter =1;
    [HideInInspector] public float timer;
    [HideInInspector] public float comboScore;
    private float comboInterval = 2f;
    private TextMeshProUGUI comboScoreText;


    private void Awake()
    {
        instance = this;

        comboScoreText = comboPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        fullDistance = GetDistance();
    }

    private void Update()
    {
        if (!GameManager.instance.isGameFinish)
        {
            float newDistance = GetDistance();
            float progressValue = Mathf.InverseLerp(fullDistance, 0, newDistance);
            UpdateProgressFill(progressValue);

            timer += Time.deltaTime;
            if (timer > comboInterval)
            {
                comboPanel.SetActive(false);
                comboScore = 0;
            }
        }
        else
        {
            comboPanel.SetActive(false);
        }
    }

    private void UpdateProgressFill(float value)
    {
        fillImage.fillAmount = value;
    }

    private float GetDistance()
    {
        Vector3 zThrowableBall = new Vector3(0, 0, ThrowManager.instance.throwableBall.transform.position.z);
        Vector3 zFinishLine = new Vector3(0, 0, finishLine.transform.position.z);
        return Vector3.Distance(zThrowableBall, zFinishLine);
    }

    public void FailPanel()
    {
        GameManager.instance.isGameOver = true;
        failPanel.SetActive(true);
    }

    public void WinPanel()
    {
        GameManager.instance.isGameWin = true;
        winPanel.SetActive(true);
    }

    public void UpdateBallCount()
    {
        ballCounter++;
        ballText.text = ballCounter.ToString();
        ballText.transform.DOScale(Vector3.one * 1.25f, 0.25f).OnComplete(() => ballText.transform.DOScale(Vector3.one, 0.25f));
    }

    public void Combo()
    {
        timer = 0;       
        comboScore++;
        comboScoreText.text = comboScore.ToString();
        comboPanel.SetActive(true);
    }
}
