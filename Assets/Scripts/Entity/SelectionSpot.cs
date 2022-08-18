using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionSpot : DragSpot
{
    SpriteRenderer sr;

    List<Effect> effectList;
    Dictionary<string, int> effectDictionary;
    ExpeditionManager expeditionManager;

    public GameObject deleteIcon;


    public void Start()
    {
        dragController = GameObject.Find("DragController").GetComponent<DragController>();
        sr = GetComponent<SpriteRenderer>();
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        effectList = expeditionManager.effectList;
        effectDictionary = expeditionManager.effectDictionary;
        ToggleDeleteIcon(false);
    }

    public override void Update()
    {
        if (isOccupied)
        {
            sr.color = new Color(1, 1, 1, 0);
            if (!draggedObject.GetComponent<Draggable>().isDragging)
            {
                ToggleDeleteIcon(true);
            } else 
            {
                ToggleDeleteIcon(false);
            }
        } 
        else 
        {
            sr.color = new Color(1, 1, 1, 1);
            ToggleDeleteIcon(false);
        }
    }

    public override void TakenBy(GameObject obj)
    {
        isOccupied = true;
        draggedObject = obj;
        obj.GetComponent<Rayc>().idleOnly = true;
        Rayc raycScript = obj.GetComponent<Rayc>();
        effectList.Add(raycScript.effect);
        AddToDictionary(raycScript.effect);
        obj.GetComponent<Draggable>()._dragSpot = gameObject;
        FindObjectOfType<StatusSummary>().UpdateStatusSummary();
    }

    public void AddToDictionary(Effect effect)
    {
        if (effectDictionary.ContainsKey(effect.name))
        {
            effectDictionary[effect.name] += 1;
        } else 
        {
            effectDictionary.Add(effect.name, 1);
        }
    }

    public void RemoveFromDictionary(Effect effect)
    {
        if (effectDictionary.ContainsKey(effect.name))
        {
            effectDictionary[effect.name] -= 1;
            if (effectDictionary[effect.name] == 0)
            {
                effectDictionary.Remove(effect.name);
            }
        }
    }

    public override void ClearObj()
    {
        isOccupied = false;
        if (draggedObject != null)
        {
            effectList.Remove(draggedObject.GetComponent<Rayc>().effect);
            RemoveFromDictionary(draggedObject.GetComponent<Rayc>().effect);
        }
        draggedObject = null;
    }

    public void RemoveSelection()
    {
        expeditionManager.player.PutToInventory(draggedObject.GetComponent<GameAsset>(), false);
        ClearObj();
        FindObjectOfType<StatusSummary>()?.UpdateStatusSummary();
    }

    void ToggleDeleteIcon(bool isOn)
    {
        deleteIcon.SetActive(isOn);
    }
}
