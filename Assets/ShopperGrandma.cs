using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopperGrandma : MonoBehaviour
{
    private Sprite[] sprites;

    [SerializeField] GameObject DecoInventoryUI;

    GameObject talkBar;
    TMP_Text talkBarText;
    public List<string> sentences;

    //float timer;


    // Start is called before the first frame update
    void Start()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites/Shop/ShopperGrandma");
        talkBar = GameObject.Find("TalkBar");
        talkBarText = GameObject.Find("ShopperGrandmaSays").GetComponent<TMP_Text>();
        talkBar.SetActive(false);
        DecoInventoryUI.SetActive(false);
    }

    void OnMouseOver()
    {
        gameObject.GetComponent<Image>().sprite = sprites[1];
        if (sentences.Count != 0)
        {
            talkBar.SetActive(true);
            talkBarText.text = sentences[Random.Range(0, sentences.Count)];
        }
    }

    void OnMouseExit()
    {
        gameObject.GetComponent<Image>().sprite = sprites[0];
        talkBar.SetActive(false);
    }


    public void OnMouseDown()
    {
        DecoInventoryUI.SetActive(true);
    }
}