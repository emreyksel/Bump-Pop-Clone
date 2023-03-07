using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Throw : MonoBehaviour
{
    private Rigidbody rb;
    private LineRenderer line;
    public LayerMask wall;

    private Vector3 firstTouchPosition;
    private Vector3 curTouchPosition;
    [SerializeField] private float sensitivityMultiplier = 0.1f;
    private float rotateY;

    public float forceSpeed;
    [SerializeField] private float trajectoryLength;

    private bool controlVelocity = false;
    private bool aim = false;
    private bool shootReady = false;
    private bool strike = false;
    private bool shoot = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        line = transform.GetChild(0).GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        controlVelocity = true;
        strike = false;
    }

    private void Update()
    {
        if (GameManager.instance.isGameFinish || GameManager.instance.isGameOver || !GameManager.instance.isgameStart)
            return;

        Rotate();
        ThrowBall();
        Trajectory();
        VelocityControl();       
    }

    private void VelocityControl()
    {
        if (rb.velocity.magnitude > 1f)
        {
            controlVelocity = true;
        }

        if (controlVelocity)
        {
            if (rb.velocity.magnitude < 1f)
            {
                if (!strike && shoot)
                {
                    UIManager.instance.FailPanel();
                }

                aim = true;
            }
        }
    }

    private void Trajectory()
    {
        line.SetPosition(0, transform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, trajectoryLength, wall))
        {
            line.positionCount = 3;
            line.SetPosition(1, hit.point);

            float degree = Mathf.Abs(Mathf.Atan(transform.forward.z / transform.forward.x) * Mathf.Rad2Deg);
            float degreeSin = Mathf.Sin(degree * Mathf.Deg2Rad);
            float zDistance = Mathf.Abs(hit.point.z - transform.position.z);
            float firstPartOfTrajectory = zDistance * (1 / degreeSin);

            Vector3 reflect = Vector3.Reflect(transform.forward, hit.normal);
            line.SetPosition(2, hit.point + reflect * (trajectoryLength - firstPartOfTrajectory));
        }
        else
        {
            line.positionCount = 2;
            line.SetPosition(1, transform.position + transform.forward * trajectoryLength);
        }
    }

    private void ThrowBall()
    {
        if (Input.GetMouseButtonDown(0) || aim)
        {
            shootReady = true;
            aim = false;
            controlVelocity = false;
            transform.eulerAngles = Vector3.zero;          
            line.enabled = true;

            foreach (var item in ThrowManager.instance.balls)
            {
                item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }

            foreach (var item in ThrowManager.instance.spawnerBalls)
            {
                item.transform.GetChild(1).gameObject.SetActive(true);
            }
        }

        if (Input.GetMouseButtonUp(0) && shootReady)
        {
            shootReady = false;
            rb.AddForce(transform.forward * forceSpeed, ForceMode.Impulse);
            shoot = true;
            line.enabled = false;

            foreach (var item in ThrowManager.instance.spawnerBalls)
            {
                item.transform.GetChild(1).gameObject.SetActive(false);
            }

            Camera.main.DOShakeRotation(0.35f, 0.35f, fadeOut: true).OnComplete(() => Camera.main.transform.eulerAngles = new Vector3(30, 0, 0));
        }
    }

    private void Rotate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstTouchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            curTouchPosition = Input.mousePosition;

            Vector2 touchDelta = (curTouchPosition - firstTouchPosition);

            rotateY = (transform.eulerAngles.y + (touchDelta.x * sensitivityMultiplier));

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotateY, transform.eulerAngles.z);

            firstTouchPosition = Input.mousePosition;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.CompareTag("Spawner")) // spawner ball hit
        {
            strike = true;
            collision.gameObject.tag = "Untagged";
            ThrowManager.instance.spawnerBalls.Remove(collision.gameObject);
            ThrowManager.instance.balls.Add(collision.gameObject);
            collision.transform.GetChild(1).gameObject.SetActive(false);
            
            for (int i = 0; i < UpgradeManager.instance.ballSpawnCount; i++) 
            {
                GameObject newBall = ObjectPool.instance.GetPooledObject(0);
                newBall.GetComponent<MeshRenderer>().material.color = collision.gameObject.GetComponent<MeshRenderer>().material.color;
                newBall.transform.position = collision.transform.position; 
                newBall.GetComponent<Rigidbody>().AddForce(Vector3.forward * forceSpeed, ForceMode.Impulse);
                if (!ThrowManager.instance.balls.Contains(newBall))
                {
                    ThrowManager.instance.balls.Add(newBall);

                    UIManager.instance.UpdateBallCount();
                    UpgradeManager.instance.UpdateMoney(UpgradeManager.instance.moneyValue);
                    UIManager.instance.Combo();
                }               
            }           
        }
    }
}
