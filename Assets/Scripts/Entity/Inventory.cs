using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject content;

    [SerializeField] Player player;

    [SerializeField] CanvasGroup tabCanvasGroup;

    ContentMode mode = ContentMode.All;

    ContentMode prevMode = ContentMode.All;

    public bool inventoryOnDisplay = false;

    void Start()
    {
        // initialize player assets
        foreach (GameAsset asset in player.assets)
        {
            GameObject newObject = Instantiate(asset.gameObject as GameObject, content.transform);
 
            newObject.name = asset.gameObject.name;
        }
    }

    void Update()
    {
        if (mode != prevMode)
        {
            prevMode = mode;
            UpdateContentMode(mode);
        } 
    }

    public GameObject InstantiateToInventory(GameObject obj)
    {
        return Instantiate(obj as GameObject, content.transform);
    }

    public void RemoveConsumableFromInventory(ConsumableItem item)
    {
        foreach (Transform child in content.transform)
        {
            ConsumableItem cur = child.GetComponent<ConsumableItem>();
            if (cur != null && cur.Equals(item))
            {
                Destroy(cur.gameObject);
                break;
            }
        }
    }

    public void RemoveTradableFromInventory(TradableAsset asset)
    {
        foreach (Transform child in content.transform)
        {
            TradableAsset cur = child.GetComponent<TradableAsset>();
            if (cur != null && cur.Equals(asset))
            {
                Destroy(cur.gameObject);
                break;
            }
        }
    }

    public void ToggleInventoryOnDisplay()
    {
        inventoryOnDisplay = !inventoryOnDisplay;
    }

    public void SetContentMode(ContentMode mode)
    {
        this.mode = mode;
    }

    void CompareAndToggle(string tag, GameObject objectToToggle)
    {
        if (objectToToggle.tag == tag)
        {
            objectToToggle.SetActive(true);
        } 
        else
        {
            objectToToggle.SetActive(false);
        }
    }
    public void UpdateContentMode(ContentMode mode)
    {
        foreach (Transform child in content.transform)
        {
            string tag = child.gameObject.tag;

            switch (mode)
            {
                case ContentMode.RaycOnly:
                    CompareAndToggle("Rayc", child.gameObject);
                    break;
                case ContentMode.InteractableItemOnly:
                    CompareAndToggle("InteractableItem", child.gameObject);
                    break;
                case ContentMode.RuneItemOnly:
                    CompareAndToggle("RuneItem", child.gameObject);
                    break;
                case ContentMode.RecoveryItemOnly:
                    CompareAndToggle("RecoveryItem", child.gameObject);
                    break;
                case ContentMode.All:
                    child.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }

    public void EnableTab()
    {
        tabCanvasGroup.interactable = true;
    }

    public void DisableTab()
    {
        tabCanvasGroup.interactable = false;
    }
}
