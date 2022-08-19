using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopperGirl : MonoBehaviour
{
    private Sprite[] sprites;
    GameObject talkBar;
    TMP_Text talkBarText;
    public List<string> sentences;
    GameObject InventoryUI;

    float timer;


    // Start is called before the first frame update
    void Start()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites/Shop/ShopperGirl");
        talkBar = GameObject.Find("talkBar");
        talkBarText = GameObject.Find("ShopperGirlSays").GetComponent<TMP_Text>();
        talkBar.SetActive(false);
        InventoryUI = GameObject.Find("InventoryUI");
        InventoryUI.SetActive(false);
    }

    void Update()
    {
        // if (talkBar.activeSelf)
        // {
        //     timer += Time.deltaTime;
        //     if (timer >= 2f)
        //     {
        //         talkBar.SetActive(false);
        //         timer = 0;
        //     }
        // }
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
        Debug.Log("Clicked");
        InventoryUI.SetActive(true);
    }
}