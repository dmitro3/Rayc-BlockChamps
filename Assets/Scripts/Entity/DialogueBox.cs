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

    public Button closeButton;

    public GameObject dialogueBoxMask;

    public void SetFunctionToYesButton(UnityAction yesFunction)
    {
        UnityAction action = null;
        action += yesFunction;
        yesButton.onClick.AddListener(action);
    }

    public void SetFunctionToCloseButton(UnityAction closeFunction)
    {
        UnityAction action = null;
        action += closeFunction;
        closeButton.onClick.AddListener(action);
    }

    public void ShowDialogue(string title, string text, bool showYesButton)
    {
        dialogueTitle.text = title;
        dialogueText.text = text;
        yesButton.gameObject.SetActive(showYesButton);
        gameObject.SetActive(true);
        dialogueBoxMask.SetActive(true);
    }

    public void HideDialogue()
    {
        yesButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
        dialogueBoxMask.SetActive(false);
    }
}
