using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ListType
{
    Rayc,
    Rune,
    Recovery
}

public class GameAssetList : MonoBehaviour
{
    public GameObject list;

    [SerializeField] GameObject listItemReference;

    [SerializeField] GameObject listPrefab;

    [SerializeField] GameObject mask;

    [SerializeField] DialogueManager dialogueManager;

    CanvasGroup disableCanvasGroup = null;

    public bool isHealing = false;

    public Rayc selectedRayc;

    public ListType listType;

    void OnEnable()
    {
        mask.SetActive(true);
        switch(listType)
        {
            case ListType.Rayc:
                InitializeRaycListItems();
                break;
            case ListType.Rune:
                InitializeRuneListItems();
                break;  
            case ListType.Recovery:
                InitializeRecoveryListItems();
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (gameObject.activeSelf) PromptUserForInput();
    }

    void OnDisable()
    {
        ClearListItems();
        mask.SetActive(false);
    }

    public void Show(CanvasGroup _disableCanvasGroup)
    {
        gameObject.SetActive(true);
        disableCanvasGroup = _disableCanvasGroup;
        if (disableCanvasGroup != null)
        {
            disableCanvasGroup.interactable = false;
        }
        disableCanvasGroup = _disableCanvasGroup;
    }

    public void Hide()
    {
        if (disableCanvasGroup != null)
        {
            disableCanvasGroup.interactable = true;
        }
        disableCanvasGroup = null;
        isHealing = false;
        gameObject.SetActive(false);
    }

    public void ClearSelectedRayc()
    {
        selectedRayc = null;
    }

    void InitializeRaycListItems()
    {
        foreach (Transform child in listItemReference.transform)
        {
            if (child.gameObject.CompareTag("Rayc"))
            {
                Rayc rayc = child.gameObject.GetComponent<Rayc>();
                if (rayc.fullness > 0 || isHealing)
                {
                    GameObject listObj = Instantiate(listPrefab, list.transform);
                    listObj.name = child.gameObject.name;
                    ListItem listItem = listObj.GetComponent<ListItem>();
                    listItem.SetRaycValues(rayc);
                }
            }
        }
    }

    void InitializeRuneListItems()
    {
        foreach (Transform child in listItemReference.transform)
        {
            if (child.gameObject.CompareTag("RuneItem"))
            {
                GameObject listObj = Instantiate(listPrefab, list.transform);
                listObj.name = child.gameObject.name;
                ListItem listItem = listObj.GetComponent<ListItem>();
                listItem.SetRuneValues(child.GetComponent<RuneItem>());
            }
        }
    }

    void InitializeRecoveryListItems()
    {
        foreach (Transform child in listItemReference.transform)
        {
            if (child.gameObject.CompareTag("RecoveryItem"))
            {
                GameObject listObj = Instantiate(listPrefab, list.transform);
                listObj.name = child.gameObject.name;
                ListItem listItem = listObj.GetComponent<ListItem>();
                listItem.SetRecoveryValues(child.GetComponent<RecoveryItem>());
            }
        }
    }

    void ClearListItems()
    {
        foreach (Transform child in list.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void PromptUserForInput()
    {
        bool done = false;

        if (Input.GetMouseButtonDown(0))
        {
            int ignoreLayers = 1 << LayerMask.NameToLayer("InventoryItem");
            Vector3 mousePos = Input.mousePosition;
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos2D), -Vector2.up, ~ignoreLayers);
            if (hit.collider != null)
            {
                if (gameObject.activeSelf && listType == ListType.Rayc)
                {
                    done = WaitForRaycInput(hit);
                }
            }
        }

        if (done)
        {
            if (dialogueManager.duringDialogue)
            {
                dialogueManager.dialoguePanel.SetActive(true);
                dialogueManager.DisplayNextSentence();
            }
            if (gameObject.activeSelf) Hide();
        }
    }

    bool WaitForRaycInput(RaycastHit2D hit)
    {
        ListItem listItem = hit.collider.gameObject.GetComponent<ListItem>();
        if (listItem != null && listItem.rayc.gameObject.CompareTag("Rayc"))
        {
            Rayc rayc = listItem.rayc;
            if (rayc.fullness > 0 || isHealing)
            {
                selectedRayc = rayc;
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (hit.collider.gameObject.name == "CloseGameAssetList")
        {
            selectedRayc = null;

            if (dialogueManager.duringDialogue)
            {
                dialogueManager.sentences.Clear();
                dialogueManager.sentences.Enqueue("So not training? Don't waste my time.");
            }

            return true;
        }
        else
        {
            return false;
        }
    }
}
