using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{

    [SerializeField] Inventory inventory;

    [SerializeField] Button backButton;

    [SerializeField] Button shopButton;

    [SerializeField] private GameObject RaycHouseInside;

    [SerializeField] ProfessionTree professionTree;

    ExpeditionManager expeditionManager;

    public UIMonitor uiMonitor;

    Camera mainCamera;

    void Start()
    {
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
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
        DisplayButton(shopButton);
        if (!uiMonitor.expeditionPage.activeSelf)
        {
            uiMonitor.expeditionPage.SetActive(true);
            inventory.SetContentMode(ContentMode.RaycOnly);
            uiMonitor.ToggleTopBar(false);
            uiMonitor.ShiftCamera(CameraDisplacement.EXPEDITION, 0);
            if (expeditionManager.hasPendingResult)
            {
                expeditionManager.PlayBossFight();
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
        uiMonitor.ShiftCamera(CameraDisplacement.SHOP, 0);
        DisplayBackButton(shopButton);
    }

    public void OnShopBackButtonPressed()
    {
        uiMonitor.ShiftCamera(0, 0);
        DisplayButton(shopButton);
        if (uiMonitor.shopPage.activeSelf) uiMonitor.shopPage.SetActive(false);
    }

    void DisplayButton(Button button)
    {
        button.gameObject.SetActive(true);
        backButton.gameObject.SetActive(false);
    }

    void DisplayBackButton(Button button)
    {
        button.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
    }

    public void CloseRaycShop()
    {
        RaycHouseInside.SetActive(false);
    }
    
    public void OnDojoButtonClicked()
    {
        DisplayButton(shopButton);
        uiMonitor.ToggleTopBar(false);
        uiMonitor.ShiftCamera(0, CameraDisplacement.DOJO);
    }

    public void OnDojoBackButtonClicked()
    {
        uiMonitor.ToggleTopBar(true);
        uiMonitor.ShiftCamera(0, 0);
    }

    public void SwitchToTree(Rayc _rayc)
    {
        professionTree.rayc = _rayc;
        professionTree.gameObject.SetActive(true);
        uiMonitor.ShiftCamera(0, CameraDisplacement.TREE);
    }

    public void OnTreeBackButtonClicked()
    {
        professionTree.gameObject.SetActive(false);
        uiMonitor.ShiftCamera(0, CameraDisplacement.DOJO);
    }
}
