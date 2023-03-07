using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public static Finish instance;

    [HideInInspector] public int counter;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.isGameFinish = true;
        other.GetComponent<Ball>().finishBall = true;
        counter++;
    }
}
