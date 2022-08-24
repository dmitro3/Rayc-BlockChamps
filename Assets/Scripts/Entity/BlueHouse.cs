using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueHouse : MonoBehaviour
{
    private Sprite[] sprites;
    private Sprite[] BlueHouse2;
    [SerializeField] private GameObject BlueHouseInside;

    void Awake()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites/Shop/Shop Blue House");
        BlueHouse2 = Resources.LoadAll<Sprite>("Sprites/Shop/BlueHouse2");
        BlueHouseInside.SetActive(false);
    }

    void OnMouseOver()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = BlueHouse2[0];
    }

    void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
    }

     public void ShowBlueShop()
    {
        BlueHouseInside.SetActive(true);
    }

    public void CloseBlueShop()
    {
        BlueHouseInside.SetActive(false);
    }

}