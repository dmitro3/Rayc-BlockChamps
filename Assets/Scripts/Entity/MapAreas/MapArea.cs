using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class MapArea : MonoBehaviour
{
    public new string name;

    [TextArea(3, 3)]
    public string description;

    public float baseCoins;

    public TMP_Text text;

    public Boss boss;

    public List<GameAsset> treasureItems;

    DialogueBox dialogueBox;

    ExpeditionManager expeditionManager;

    void Awake()
    {
        text.alpha = 0;
    }

    void Start()
    {
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        dialogueBox = FindObjectOfType<UIMonitor>().dialogueBox;
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
            GetComponent<Canvas>().sortingOrder = 2;
            GetComponent<RectTransform>().localScale = new Vector3(1.15f, 1.15f, 1.15f);
            text.alpha = 1f;
        }
    }

    void OnMouseExit()
    {
        if (expeditionManager.allowMapEffects)
        {
            ClearAreaHover();
        }
    }

    void ClearAreaHover()
    {
        expeditionManager.isMouseHovering = false;
        expeditionManager.hoveredArea = "";
        GetComponent<Canvas>().sortingOrder = 1;
        GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        text.alpha = 0f;
    }

    public void OnAreaClick()
    {
        // set up expedition manager
        expeditionManager.SetSelectedArea(this);

        // display dialogue box
        dialogueBox.SetFunctionToYesButton(expeditionManager.StartExpedition);
        dialogueBox.SetFunctionToCloseButton(expeditionManager.ResetSelectedArea);
        dialogueBox.SetFunctionToCloseButton(ClearAreaHover);
        dialogueBox.SetFunctionToCloseButton(dialogueBox.HideDialogue);
        dialogueBox.ShowDialogue(name, description + "\n Explore this area?", true);
    }
}
