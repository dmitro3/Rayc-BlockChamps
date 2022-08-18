using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    UIMonitor uiMonitor;

    GameObject storeMovable;

    [SerializeField] GameObject BackButton;
    
    Button shopButton;

    void Start()
    {
        uiMonitor = GameObject.Find("UIMonitor").GetComponent<UIMonitor>();
        storeMovable = GameObject.Find("StoreUI");
        BackButton.SetActive(false);
        shopButton = GetComponent<Button>();
    }
    public void OnStoreButtonPressed()
    {
        uiMonitor.ShiftCamera(-211.6f, 0);
        BackButton.SetActive(true);
        shopButton.gameObject.SetActive(false);
    }

    public void OnStoreBackButtonPressed()
    {
        uiMonitor.ShiftCamera(0,0);
        shopButton.gameObject.SetActive(true);
        BackButton.SetActive(false);
    }
}
