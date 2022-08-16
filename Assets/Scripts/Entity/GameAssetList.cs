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
    [SerializeField] GameObject list;

    [SerializeField] GameObject listItemReference;

    [SerializeField] GameObject listPrefab;

    CanvasGroup disableCanvasGroup = null;

    public Rayc selectedRayc;

    public ListType listType;

    void OnEnable()
    {
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
                GameObject listObj = Instantiate(listPrefab, list.transform);
                listObj.name = child.gameObject.name;
                ListItem listItem = listObj.GetComponent<ListItem>();
                listItem.SetRaycValues(child.GetComponent<Rayc>());
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
            Vector3 mousePos = Input.mousePosition;
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos2D), -Vector2.up);
            if (hit.collider != null)
            {
                if (gameObject.activeSelf && listType == ListType.Rayc)
                {
                    done = WaitForRaycInput(hit);
                }
            }
        }

        // TODO: Add in logic to control dialogue system
    }

    bool WaitForRaycInput(RaycastHit2D hit)
    {
        ListItem listItem = hit.collider.gameObject.GetComponent<ListItem>();
        if (listItem != null && listItem.rayc.gameObject.CompareTag("Rayc"))
        {
            Rayc rayc = listItem.rayc;
            selectedRayc = rayc;
            return true;
        }
        else if (hit.collider.gameObject.name == "CloseGameAssetList")
        {
            selectedRayc = null;

            //TODO: handles further dialogue if need be

            return true;
        }
        else
        {
            return false;
        }
    }
}
