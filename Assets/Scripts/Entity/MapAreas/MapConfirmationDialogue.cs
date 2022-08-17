using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapConfirmationDialogue : DialogueBox
{

    public TMP_Text areaTitle;
    public TMP_Text areaDescription;

    public MapArea mapArea;

    void Start()
    {
        toggleConfirmationDialogue(false);
    }

    public void updateText()
    {
        if (mapArea != null)
        {
            areaTitle.text = mapArea.name;
            areaDescription.text = mapArea.description + "\nDo you want to explore this area?";
        }
    }

    public void toggleConfirmationDialogue(bool value)
    {
        if (!value && mapArea != null)
        {
            mapArea.GetComponent<Canvas>().sortingOrder = 0;
            mapArea.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            mapArea.text.alpha = 0f;
        }
        gameObject.SetActive(value);
    }
}
