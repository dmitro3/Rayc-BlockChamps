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

public class TabController : MonoBehaviour
{
    string[] allowedTabs { get; set; }

    string[] ALLTABS = { "All", "Raycs", "InteractableItems", "Rune", "RecoveryItems" };

    void Awake()
    {
        allowedTabs = ALLTABS;
    }

    void Update()
    {
        foreach (Tab tab in GetComponentsInChildren<Tab>())
        {
            tab.gameObject.SetActive(System.Array.IndexOf(allowedTabs, tab.gameObject.name) != -1);
        }
    }

    public void SwitchToAll()
    {
        allowedTabs = ALLTABS;
    }
}
