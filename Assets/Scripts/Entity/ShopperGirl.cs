using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopperGirl : MonoBehaviour
{
    private Sprite[] sprites;

    [SerializeField] GameObject InventoryUI;

    // GameObject talkBar;
    // TMP_Text talkBarText;
    // public List<string> sentences;

    float timer;


    // Start is called before the first frame update
    void Start()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites/Shop/ShopperGirl");
        // talkBar = GameObject.Find("TalkBar");
        // talkBarText = GameObject.Find("ShopperGirlSays").GetComponent<TMP_Text>();
        // talkBar.SetActive(false);
        InventoryUI.SetActive(false);
    }

    void OnMouseOver()
    {
        gameObject.GetComponent<Image>().sprite = sprites[1];
        // if (sentences.Count != 0)
        // {
        //     talkBar.SetActive(true);
        //     talkBarText.text = sentences[Random.Range(0, sentences.Count)];
        // }
    }

    void OnMouseExit()
    {
        gameObject.GetComponent<Image>().sprite = sprites[0];
        // talkBar.SetActive(false);
    }


    public void OnMouseDown()
    {
        InventoryUI.SetActive(true);
    }
}