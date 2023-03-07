using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpText : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = UpgradeManager.instance.moneyValue.ToString();
        Invoke(nameof(SendPool), 2);
    }

    public void SendPool()
    {
        ObjectPool.instance.SendPooledObject(2, gameObject);
    }
}
