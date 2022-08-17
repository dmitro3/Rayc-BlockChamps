using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapArea : MonoBehaviour
{
    public new string name;

    public string description;

    public int baseCoins;

    public TMP_Text text;

    public MapConfirmationDialogue mapConfirmationDialogue;

    public Boss boss;

    public List<Item> treasureItems;

    ExpeditionManager expeditionManager;

    void Awake()
    {
        text.alpha = 0;
    }

    void Start()
    {
        expeditionManager = GameObject.Find("ExpeditionManager").GetComponent<ExpeditionManager>();
    }

    void Update()
    {
        if (expeditionManager.isMouseHovering && expeditionManager.hoveredArea != this.name)
        {
            GetComponent<Button>().interactable = false;
        } else 
        {
            GetComponent<Button>().interactable = true;
        }
    }

    void OnMouseEnter()
    {
        if (expeditionManager.allowMapEffects)
        {
            expeditionManager.isMouseHovering = true;
            expeditionManager.hoveredArea = this.name;
            GetComponent<Canvas>().sortingOrder = 1;
            GetComponent<RectTransform>().localScale = new Vector3(1.15f, 1.15f, 1.15f);
            text.alpha = 1f;
        }
    }

    void OnMouseExit()
    {
        if (expeditionManager.allowMapEffects)
        {
            expeditionManager.isMouseHovering = false;
            expeditionManager.hoveredArea = "";
            GetComponent<Canvas>().sortingOrder = 0;
            GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            text.alpha = 0f;
        }
    }

    public void OnAreaClick()
    {
        mapConfirmationDialogue.mapArea = this;
        mapConfirmationDialogue.updateText();
        mapConfirmationDialogue.toggleConfirmationDialogue(true);
    }
}
