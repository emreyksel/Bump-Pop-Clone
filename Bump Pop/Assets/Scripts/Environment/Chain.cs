using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Chain : MonoBehaviour
{
    private List<GameObject> incomingBalls = new List<GameObject>();
    private MeshRenderer[] mr;
    private TextMeshProUGUI neededBallText; 
    public Color startColor;
    public Color finalColor;
    public int neededBall;
    private Transform anchor1; 
    private Transform anchor2;
    private bool isBreak = false;
    private float timer;

    private void Awake()
    {
        mr = transform.GetComponentsInChildren<MeshRenderer>();
        neededBallText = GetComponentInChildren<TextMeshProUGUI>();

        foreach (var item in mr)
        {
            item.material.color = startColor;
        }

        anchor1 = transform.GetChild(0).transform;
        anchor2 = transform.GetChild(1).transform;
    }

    private void Start()
    {
        UpdateNeededBallText();
    }

    private void Update()
    {
        if (GameManager.instance.isGameWin)
            return;

        if (!isBreak && incomingBalls.Count>0)
        {
            timer += Time.deltaTime;
            if (timer>5)
            {
                UIManager.instance.WinPanel();
            }
        }
    }

    public void Break()
    {
        isBreak = true;

        foreach (var item in transform.GetComponentsInChildren<BoxCollider>())
        {
            item.enabled = false;
        }

        foreach (var item in transform.GetComponentsInChildren<Rigidbody>())
        {
            item.isKinematic = true;
        }

        anchor1.transform.DORotate(new Vector3(0, -70, 0), 0.35f); 
        anchor2.transform.DORotate(new Vector3(0, 70, 0), 0.35f);

        foreach (var item in incomingBalls)
        {
            item.GetComponent<Rigidbody>().AddForce(Vector3.forward * 2, ForceMode.Impulse);
        }

        Camera.main.DOShakeRotation(1, 2, fadeOut: true).OnComplete(() => Camera.main.transform.eulerAngles = new Vector3(30, 0, 0));
    }

    private void OnTriggerEnter(Collider other)
    {       
        if (!incomingBalls.Contains(other.gameObject))
        {          
            incomingBalls.Add(other.gameObject);
            UpdateNeededBallText();

            timer = 0;

            foreach (var item in mr)
            {
                item.material.color = Color.Lerp(startColor, finalColor, (float)incomingBalls.Count/(float)neededBall);
            }
        }
       
        if (incomingBalls.Count >= neededBall && !isBreak)
        {
            Break();
        }    
    }

    public void UpdateNeededBallText()
    {
        int remainingBall = neededBall - incomingBalls.Count;
        neededBallText.text = Mathf.Max(0, remainingBall).ToString();
    }
}
