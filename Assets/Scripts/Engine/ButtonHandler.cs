using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{

    [SerializeField] Inventory inventory;

    [SerializeField] Button backButton;
    
    [SerializeField] Button shopButton;

    ExpeditionManager expeditionManager;

    public UIMonitor uiMonitor;

    void Start()
    {
        expeditionManager = FindObjectOfType<ExpeditionManager>();
    }

    public void OnBagButtonPressed()
    {
        Animator anim = inventory.GetComponent<Animator>();
        inventory.ToggleInventoryOnDisplay();
        if (inventory.inventoryOnDisplay)
        {
            anim.Play("Open");
        }
        else
        {
            anim.Play("Close");
        }
    }

    public void OnExpeditionButtonPressed()
    {
         if (!uiMonitor.expeditionPage.activeSelf)
        {
            uiMonitor.expeditionPage.SetActive(true);
            inventory.SetContentMode(ContentMode.RaycOnly);
            uiMonitor.ToggleTopBar(false);
            uiMonitor.ShiftCamera(CameraDisplacement.EXPEDITION, 0);
            if (expeditionManager.hasPendingResult)
            {
                // TODO: add boss cut scene
                expeditionManager.GrantPlayerRewards();
            }
        }
        else
        {
            uiMonitor.expeditionPage.SetActive(false);
            inventory.SetContentMode(ContentMode.All);
            uiMonitor.ToggleTopBar(true);
            uiMonitor.ShiftCamera(0, 0);
        }
    }

    public void OnMapBackButtonPressed()
    {
        if (uiMonitor.dialogueBox.gameObject.activeSelf) uiMonitor.dialogueBox.closeButton.onClick?.Invoke();
        uiMonitor.ShiftCamera(CameraDisplacement.EXPEDITION, 0);
    }

    public void OnShopButtonPressed()
    {
        if (!uiMonitor.shopPage.activeSelf) uiMonitor.shopPage.SetActive(true);
        uiMonitor.ShiftCamera(-211.6f, 0);
        backButton.gameObject.SetActive(true);
        shopButton.gameObject.SetActive(false);
    }

    public void OnShopBackButtonPressed()
    {
        uiMonitor.ShiftCamera(0,0);
        shopButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(false);
        if (uiMonitor.shopPage.activeSelf) uiMonitor.shopPage.SetActive(false);
    }
}
