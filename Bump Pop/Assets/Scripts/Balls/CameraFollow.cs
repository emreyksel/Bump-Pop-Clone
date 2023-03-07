using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset;
    public Transform target;
    private float smoothTime = 0.25f;
    private Vector3 currentVelocity;

    private void Awake()
    {
        offset = transform.position - target.position;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.isGameOver || Lava.instance.counter > 0)
            return;


        if (GameManager.instance.isGameFinish)
        {
            Vector3 targetPosition = target.position + offset;
            targetPosition.x = 0;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
        }
        else
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
        }
    }
}
