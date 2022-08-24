using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bin : MonoBehaviour
{
    private Sprite[] sprites;

    void Awake()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites/UI/Dustbin");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.GetComponent<Image>().sprite = sprites[1];
        gameObject.transform.localScale = new Vector3(1f, 1.2f, 1f);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        gameObject.GetComponent<Image>().sprite = sprites[0];
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}