using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{

    public string trainingType = "";

    public GameObject trainingSection;

    public GameAssetList gameAssetList;

    DialogueManager dialogueManager;

    Collider2D treeButtonCollider;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        treeButtonCollider = GameObject.Find("TreeButton").GetComponent<Collider2D>();
    }

    void Update()
    {
        if (trainingSection.activeSelf) WaitForTrainingInput();
    }

    public void StartTraining()
    {
        trainingSection.SetActive(true);
        treeButtonCollider.enabled = false;
    }

    public void ExitTraining()
    {
        trainingSection.SetActive(false);
        treeButtonCollider.enabled = true;
        trainingType = "";
    }

    public void WaitForTrainingInput()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos2D), -Vector2.up);
            if (hit.collider != null)
            {
                switch(hit.collider.gameObject.name)
                {
                    case "StrengthTraining":
                        trainingType = "strength";
                        break;
                    case "DiscoveryTraining":
                        trainingType = "discovery";
                        break;
                    case "SocialityTraining":
                        trainingType = "sociality";
                        break;
                    case "CloseTrainingSection":
                        trainingType = "none";
                        dialogueManager.sentences.Clear();
                        dialogueManager.sentences.Enqueue("Come on...Don't waste my time.");
                        break;
                    default:
                        break;
                }
            }
        }

        if (trainingType != "")
        {
            if (trainingSection.activeSelf) trainingSection.SetActive(false);
            dialogueManager.dialoguePanel.SetActive(true);
            dialogueManager.DisplayNextSentence();
        }
    }

    public void TrainRayc()
    {
        switch (trainingType)
        {
            case "strength":
                gameAssetList.selectedRayc.strength++;
                gameAssetList.selectedRayc.fullness--;
                break;
            case "discovery":
                gameAssetList.selectedRayc.discovery++;
                gameAssetList.selectedRayc.fullness--;
                break;
            default:
                break;
        }
    }
}
