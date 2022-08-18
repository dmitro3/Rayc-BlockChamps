using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    public TMP_Text dialogueText;

    public void ShowDialogue(string text)
    {
        gameObject.SetActive(true);
        dialogueText.text = text;
    }

    public void HideDialogue()
    {
        gameObject.SetActive(false);
    }
}
