using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ContentMode
{
    All,
    RaycOnly,
    InteractableItemOnly,
    RecoveryItemOnly,
    RuneItemOnly,
}

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject content;

    [SerializeField] Player player;

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

    public void RemoveFromInventory(GameAsset asset)
    {
        foreach (Transform child in content.transform)
        {
            if (child.GetComponent<GameAsset>().id == asset.id)
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }

    public void ToggleInvetoryOnDisplay()
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
    void UpdateContentMode(ContentMode mode)
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
}
