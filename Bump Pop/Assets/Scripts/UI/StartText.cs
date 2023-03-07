using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartText : MonoBehaviour
{
    public GameObject shop;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && !IsMouseOverUI())
        {
            GameManager.instance.isgameStart = true;
            shop.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
