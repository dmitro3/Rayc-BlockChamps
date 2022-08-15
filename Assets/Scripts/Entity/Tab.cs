using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tab : MonoBehaviour
{
    public ContentMode mode;

    Inventory inventory;

    void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    public void SetContentModeToInventory()
    {
        inventory.SetContentMode(mode);
    }
}
