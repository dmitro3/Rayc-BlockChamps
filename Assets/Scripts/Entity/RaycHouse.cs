using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycHouse : MonoBehaviour
{
    private Sprite[] sprites;
    [SerializeField] private GameObject RaycHouseInside;

    void Awake()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites/Shop/Rayc House");
        RaycHouseInside.SetActive(false);
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
        RaycHouseInside.SetActive(true);
    }

    public void CloseRaycShop()
    {
        RaycHouseInside.SetActive(false);
    }
}
