using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventRarity
{
    Common,
    Uncommon,
    Rare,
    VeryRare,
    Legendary,
}

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] string specificRaycPrefabName;
    [SerializeField] string interactablePrefabName;

    [SerializeField] Vector2 raycBodyOffset;
    [SerializeField] Quaternion bodyRotation;
    [SerializeField] Vector3 raycBodyScale;

    [SerializeField] bool allowUpdate = false;

    Quaternion originalBodyRotation;
    Vector3 originalBodyScale;

    public bool isPlaying = false;

    public Poise poise;

    public EventRarity eventRarity;

    Rayc rayc = null;

    InteractableItem interactableItem;

    Rayc specificRayc;

    // for testing and setting up offset purpose
    void Update()
    {
        if (allowUpdate)
        {
            UpdateRaycSpecs();
        }
    }

    void UpdateRaycSpecs()
    {
        if (rayc != null && interactableItem != null && !GameObject.Find("DragController").GetComponent<DragController>()._isDragActive)
        {
            rayc.GetComponent<RectTransform>().anchoredPosition = interactableItem.GetComponent<RectTransform>().anchoredPosition + raycBodyOffset;
            rayc.transform.localScale = raycBodyScale;
        }
    }

    public bool CheckPlayable()
    {
        if (interactablePrefabName == "") 
        {
            return false;
        }
        GameObject placedItems = GameObject.Find("PlacedItems");
        List<Rayc> placedRaycList = new List<Rayc>();
        foreach (Transform child in placedItems.transform)
        {
            if (specificRaycPrefabName != "" && child.gameObject.GetComponent<Rayc>().prefabName == specificRaycPrefabName)
            {
                specificRayc = child.gameObject.GetComponent<Rayc>();
            }

            if (child.gameObject.GetComponent<GameAsset>().prefabName == interactablePrefabName)
            {
                interactableItem = child.gameObject.GetComponent<InteractableItem>();
            }
            
            if (child.gameObject.CompareTag("Rayc") && child.GetComponent<Rayc>().interactionEvent == null)
            {
                rayc = child.gameObject.GetComponent<Rayc>();
                if (rayc != null && !rayc.idleOnly) placedRaycList.Add(rayc);
            }
        }

        // randomly choose a placed rayc
        if (placedRaycList.Count > 0)
        {
            rayc = placedRaycList[Random.Range(0, placedRaycList.Count)];
        }
        
        if (rayc != null && rayc.interactionEvent == null && !GameObject.Find("DragController").GetComponent<DragController>()._isDragActive)
        {
            if (specificRayc != null)
            {
                rayc = specificRayc;
            }
            if (interactableItem != null)
            {
                originalBodyRotation = rayc.spriteRotation;
                originalBodyScale = rayc.spriteLocalScale;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        
    }

    public void PlayInteraction()
    {
        isPlaying = true;
        DragController.LeaveSpot(rayc.GetComponent<Draggable>());
        Draggable raycDraggable = rayc.GetComponent<Draggable>();
        raycDraggable._dragSpot = null;
        raycDraggable._movementDestination = null;
        rayc.GetComponent<Animator>().enabled = false;
        rayc.ChangePoise(poise);
        rayc.GetComponent<RectTransform>().anchoredPosition = interactableItem.GetComponent<RectTransform>().anchoredPosition + raycBodyOffset;
        rayc.transform.rotation = bodyRotation;
        rayc.transform.localScale = raycBodyScale;
        rayc.interactionEvent = this;

        interactableItem.GetComponent<Collider2D>().enabled = false;
        interactableItem.GetComponent<Draggable>().enabled = false;
        interactableItem.GetComponent<SpriteRenderer>().sortingOrder = 1;
        interactableItem.GetComponent<Animator>().Play("Interact");
    }

    public void EndInteraction()
    {
        isPlaying = false;
        rayc.RevertToOriginalSprite();
        rayc.GetComponent<Animator>().enabled = true;
        rayc.transform.rotation = originalBodyRotation;
        rayc.transform.localScale = originalBodyScale;
        rayc.interactionEvent = null;
        rayc.justEndedInteraction = true;

        interactableItem.GetComponent<Collider2D>().enabled = true;
        interactableItem.GetComponent<Draggable>().enabled = true;
        interactableItem.GetComponent<SpriteRenderer>().sortingOrder = 0;
        interactableItem.GetComponent<Animator>().Play("Idle");

        originalBodyRotation = Quaternion.identity;
        originalBodyScale = Vector3.zero;
    }
}