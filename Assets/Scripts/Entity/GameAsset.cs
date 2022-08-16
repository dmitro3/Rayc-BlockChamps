using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class GameAsset : MonoBehaviour
{
    public string prefabName;

    [TextArea(3, 3)]
    public string description;

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

    void OnEnable()
    {
        assetStats = FindObjectOfType<UIMonitor>().assetStats;
        gameAssetList = FindObjectOfType<UIMonitor>().gameAssetList;
    }

    void Update()
    {
        Draggable draggable = GetComponent<Draggable>();
        if (draggable != null && assetStats != null && gameAssetList != null)
        {
            draggable.enabled = !assetStats.gameObject.activeSelf && !gameAssetList.gameObject.activeSelf;
        }
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
