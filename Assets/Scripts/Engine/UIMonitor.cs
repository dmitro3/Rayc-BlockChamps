using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMonitor : MonoBehaviour
{
    [SerializeField] GameObject uiMask;

    [SerializeField] Inventory inventory;

    [SerializeField] GameObject topBar;

    public GameObject expeditionPage;

    public GameObject shopPage;

    public DialogueBox dialogueBox;

    public GameAssetList gameAssetList;

    public AssetStats assetStats;

    Camera mainCamera;

    Vector3 originalCameraPosition;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = false;

        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        originalCameraPosition = mainCamera.transform.position;
    }

    void Start()
    {
        // setting all pages to inactive
        expeditionPage.SetActive(false);
        shopPage.SetActive(false);
    }

    void Update()
    {
        uiMask.SetActive(inventory.inventoryOnDisplay);
    }

    public void ShiftCamera(float displacementX, float displacementY)
    {
        mainCamera.transform.position = new Vector3(originalCameraPosition.x + displacementX, originalCameraPosition.y + displacementY, originalCameraPosition.z);
    }

    public void ToggleTopBar(bool value)
    {
        topBar.SetActive(value);
    }
}
