using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public static Lava instance;

    public Transform canvas;
    public GameObject moneyUI;
    [HideInInspector] public int counter;

    private void Awake()
    {
        instance = this;    
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        other.GetComponent<Rigidbody>().useGravity = true;

        counter++;

        GameObject UIMoney = ObjectPool.instance.GetPooledObject(1);
        UIMoney.transform.SetParent(canvas);
        UIMoney.transform.position = RandomPos();

        if (counter == Finish.instance.counter)
        {
            UIManager.instance.WinPanel();
        }

        Destroy(other.gameObject, 2);
    }

    public Vector3 RandomPos()
    {
        float x = Random.Range(340, 740);
        float y = Random.Range(760, 1160);
        Vector3 randPos = moneyUI.transform.localPosition + new Vector3(x, y, 0);
        return randPos;
    }
}
