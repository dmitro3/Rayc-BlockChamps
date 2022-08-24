using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoHouse : MonoBehaviour
{
    private Sprite[] sprites;
    [SerializeField] private GameObject DecoHouseInside;
    [SerializeField] GameObject DecoInventoryUI;

    void Awake()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites/Shop/Deco House");
        DecoHouseInside.SetActive(false);
    }

    void OnMouseOver()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
    }

    void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
    }

    public void ShowDecoShop()
    {
        Debug.Log("DecoShop clicked");
        DecoHouseInside.SetActive(true);
        DecoInventoryUI.SetActive(false);
    }

    public void CloseDecoShop()
    {
        DecoHouseInside.SetActive(false);
    }
}
