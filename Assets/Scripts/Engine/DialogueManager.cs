using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    [SerializeField] TMP_Text dialogueContent;

    [SerializeField] GameAssetList gameAssetList;

    bool isTyping = false;

    string sentence = "";

    Dialogue dialogue;

    public Queue<string> sentences;

    public Master master;

    public CanvasGroup dojoInteractables;

    public bool duringDialogue = false;

    void Start()
    {
        sentences = new Queue<string>();
    }

    void Update()
    {
        // disables interactables during dialogue
        dojoInteractables.interactable = !duringDialogue;
    }

    public void StartDialogue(Dialogue _dialogue)
    {


        duringDialogue = true;

        gameAssetList.listType = ListType.Rayc;

        dialoguePanel.SetActive(true);

        dialogue = _dialogue;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {

        if (sentences.Count == 0 && !isTyping)
        {
            EndDialogue();
            return;
        }

        if (isTyping && sentence != "")
        {
            StopAllCoroutines();
            isTyping = false;
            dialogueContent.text = sentence;
            return;
        }

        if (HandleMasterDialogue()) return;

        sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    void EndDialogue()
    {
        if (gameAssetList.selectedRayc != null && master.trainingType != "") 
        {
            master.TrainRayc();
        }

        dialoguePanel.SetActive(false);

        if (dialogue.name == "Dojo Master 2")
        {
            if (gameAssetList.selectedRayc != null)
            {
                FindObjectOfType<ButtonHandler>().SwitchToTree(gameAssetList.selectedRayc);
            }
        }

        sentence = "";
        master.ExitTraining();
        gameAssetList.ClearSelectedRayc();
        duringDialogue = false;
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueContent.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueContent.text += letter;
            yield return null;
        }
        isTyping = false;
    }

    // returns a bool to determine if end early
    bool HandleMasterDialogue()
    {
        if (dialogue.name == "Dojo Master 1")
        {
            if (sentences.Count == 3)
            {
                if (!master.CheckTrainingCondition())
                {
                    sentences.Clear();
                    sentences.Enqueue("Oops, seems like you don't have enough coins to train...You need at least " + master.trainingCost + " coins to train.");
                    return false;
                }
            }

            if (sentences.Count == 2 && gameAssetList.selectedRayc == null)
            {
                dialoguePanel.SetActive(false);
                gameAssetList.Show(dojoInteractables);
                return true;
            }
            else if (sentences.Count == 1 && gameAssetList.selectedRayc != null && master.trainingType == "")
            {
                dialoguePanel.SetActive(false);
                master.StartTraining();
                return true;
            }
        }

        if (dialogue.name == "Dojo Master 2")
        {
            if (sentences.Count == 2 && gameAssetList.selectedRayc == null)
            {
                dojoInteractables.interactable = false;
                dialoguePanel.SetActive(false);
                gameAssetList.Show(dojoInteractables);
                return true;
            }
        }
        
        return false;
    }
}