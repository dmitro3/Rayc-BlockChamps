using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycHouse : MonoBehaviour
{
    private Sprite[] sprites;
    [SerializeField] private GameObject RaycShopInside;

    void Awake()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites/UI/Rayc House");
        RaycShopInside.SetActive(false);
    }

    void OnMouseOver()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[2];
    }

    void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
    }

    public void ShowShop()
    {
        RaycShopInside.SetActive(true);
    }

    public void CloseRaycShop()
    {
        RaycShopInside.SetActive(false);
    }
}
