using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{

    [SerializeField] Inventory inventory;

    [SerializeField] Button backButton;
    
    [SerializeField] Button shopButton;

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

    public void OnDojoButtonClicked()
    {
        if (mainCamera.transform.position.y > 0)
        {
            uiMonitor.ShiftCamera(0, 0);
        }
        else
        {
            uiMonitor.ShiftCamera(0, CameraDisplacement.DOJO);
        }
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
