using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Rarity {
    Common,
    Rare,
    Epic,
    Legendary
}

[RequireComponent(typeof(Collider2D))]
public class GameAsset : MonoBehaviour
{
    public string prefabName;

    [TextArea(3, 3)]
    public string description;

    public Rarity rarity;

    // attributes for image/sprite transition
    public Quaternion imageRotation;
    public Quaternion spriteRotation;

    public Vector3 imageLocalScale;
    public Vector3 spriteLocalScale;

    public Vector2 imageBoxColliderSize;
    public Vector2 spriteBoxColliderSize;

    public Vector2 imageBoxColliderOffset;
    public Vector2 spriteBoxColliderOffset;

    protected AssetStats assetStats;

    protected GameAssetList gameAssetList;

    Button button;

    public bool clickable = true;

    void OnEnable()
    {
        assetStats = FindObjectOfType<UIMonitor>().assetStats;
        gameAssetList = FindObjectOfType<UIMonitor>().gameAssetList;
        if (gameObject.GetComponent<Button>() == null)
        {
            button = gameObject.AddComponent<Button>();
            button.onClick.AddListener(ShowAssetStats);
        }
    }

    void Update()
    {
        Draggable draggable = GetComponent<Draggable>();
        if (draggable != null && assetStats != null && gameAssetList != null)
        {
            bool isCameraOnShop = FindObjectOfType<Camera>().transform.position.x == CameraDisplacement.SHOP;
            draggable.enabled = !assetStats.gameObject.activeSelf && !gameAssetList.gameObject.activeSelf && !isCameraOnShop;
        }
    }

    public void ShowAssetStats()
    {
        if (clickable) assetStats.ShowStats(this);
    }

    public void ChangeToImageSpecs()
    {
        GetComponent<Image>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = false;
        gameObject.layer = LayerMask.NameToLayer("InventoryItem");
        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().enabled = false;
        }
        transform.localScale = imageLocalScale;
        transform.rotation = imageRotation;
        GetComponent<BoxCollider2D>().size = imageBoxColliderSize;
        GetComponent<BoxCollider2D>().offset = imageBoxColliderOffset;
    }

    public void ChangeToSpriteSpecs()
    {
        GetComponent<Image>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = true;
        gameObject.layer = LayerMask.NameToLayer("PlacedItem");
        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().enabled = true;
        }
        transform.localScale = spriteLocalScale;
        transform.rotation = spriteRotation;
        GetComponent<BoxCollider2D>().size = spriteBoxColliderSize;
        GetComponent<BoxCollider2D>().offset = spriteBoxColliderOffset;
    }
}
