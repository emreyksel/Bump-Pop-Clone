using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject popUpText;
    public bool finishBall = false;
    private int counter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (counter>0)
        {
            Invoke(nameof(PopUpText), 0.25f);
        }
        counter++;
    }

    private void FixedUpdate()
    {
        if (finishBall)
        {
            rb.MovePosition(rb.position + Vector3.forward * Time.deltaTime * 5);
        }
    }

    private void PopUpText()
    {
        GameObject newPopUpText = ObjectPool.instance.GetPooledObject(2);
        newPopUpText.transform.position = transform.position + Vector3.up*2 ;
    }
}
