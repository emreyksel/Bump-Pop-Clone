using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isgameStart = false;
    public bool isGameFinish = false;
    public bool isGameOver = false;
    public bool isGameWin = false;

    private void Awake()
    {
        instance = this;
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
