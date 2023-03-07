using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowManager : MonoBehaviour
{
    public static ThrowManager instance;

    public List<GameObject> balls = new List<GameObject>();
    public List<GameObject> spawnerBalls = new List<GameObject>();
    public GameObject throwableBall;
    private Camera cam;
    private float z;

    private void Awake()
    {
        instance = this;

        cam = Camera.main;
    }

    private void Start()
    {
        GameObject[] spawnerBallsArray = GameObject.FindGameObjectsWithTag("Spawner");
        for (int i = 0; i < spawnerBallsArray.Length; i++)
        {
            spawnerBalls.Add(spawnerBallsArray[i]);
        }
    }

    private void Update()
    {
        if (Lava.instance.counter > 0)
            return;
      
        FindFirstBall();
    }

    public void FindFirstBall()
    {
        z = throwableBall.transform.position.z;

        for (int i = 0; i < balls.Count; i++)
        {
            if (balls[i].transform.position.z>z)
            {
                throwableBall = balls[i];
                cam.GetComponent<CameraFollow>().target = throwableBall.transform;
                foreach (var item in balls)
                {
                    item.GetComponent<Throw>().enabled = false;
                    item.transform.GetChild(0).GetComponent<LineRenderer>().enabled = false;
                }
                throwableBall.GetComponent<Throw>().enabled = true;
            }
        }
    }
}
