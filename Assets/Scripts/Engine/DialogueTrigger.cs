using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public List<Dialogue> dialogues;

    public int dialogueIndex;

    public CanvasGroup disableCanvasGroup;

    public void TriggerDialogue()
    {
        if (dialogues.Count > 0) FindObjectOfType<DialogueManager>().StartDialogue(dialogues[dialogueIndex]);
    }

    public void SetDialogueIndex(int index)
    {
        dialogueIndex = index;
    }
}
