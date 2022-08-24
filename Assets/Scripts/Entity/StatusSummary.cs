using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusSummary : MonoBehaviour
{
    [SerializeField] GameObject list;

    [SerializeField] GameObject raycSelection;

    [SerializeField] GameObject listPrefab;

    public void UpdateStatusSummary()
    {
        ClearStatusList();

        foreach (Transform child in raycSelection.transform)
        {
            GameObject draggedObject = child.GetComponent<SelectionSpot>().draggedObject;
            if (draggedObject != null)
            {
                Rayc rayc = draggedObject.GetComponent<Rayc>();
                GameObject listObj = Instantiate(listPrefab, list.transform);
                listObj.name = rayc.gameObject.name;
                ListItem listItem = listObj.GetComponent<ListItem>();
                listItem.SetRaycValues(rayc); 
            }
        }
    }

    public void ClearStatusList()
    {
        foreach (Transform child in list.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
