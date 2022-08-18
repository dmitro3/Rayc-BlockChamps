using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    public TMP_Text dialogueTitle;

    public TMP_Text dialogueText;

    public Button yesButton;

    public void ShowDialogue(string title, string text, bool showYesButton)
    {
        dialogueText.text = text;
        yesButton.gameObject.SetActive(showYesButton);
        gameObject.SetActive(true);
    }

    public void HideDialogue()
    {
        yesButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}
