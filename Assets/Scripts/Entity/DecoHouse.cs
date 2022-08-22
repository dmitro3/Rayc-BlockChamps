using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoHouse : MonoBehaviour
{
    private Sprite[] sprites;

    void Awake()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites/Shop/Deco House");
    }

    void OnMouseOver()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
    }

    void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
    }
}