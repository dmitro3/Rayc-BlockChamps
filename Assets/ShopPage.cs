using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPage : MonoBehaviour
{
    bool isUIOverlay = false;

    [SerializeField] GameObject street;
    //[SerializeField] GameObject gashapon;

    void Update()
    {
        foreach (Collider2D c in street.GetComponentsInChildren<Collider2D>())
        {
            c.enabled = !isUIOverlay;
        }

        //gashapon.GetComponent<Button>().interactable = !isUIOverlay;
    }

    public void SetIsUIOverlay(bool isUIOverlay)
    {
        this.isUIOverlay = isUIOverlay;
    }
}

