using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssetStats : MonoBehaviour
{
    [SerializeField] TMP_Text title;

    [SerializeField] TMP_Text description;

    [SerializeField] TMP_Text statsDisplay;

    [SerializeField] TMP_Text numberDisplay;

    [SerializeField] FullnessDisplay fullnessDisplay;

    [SerializeField] Image profileImage;

    [SerializeField] Button useButton;

    [SerializeField] Button sellButton;

    GameAsset current;

    public void Close()
    {
        if (fullnessDisplay.gameObject.activeSelf) fullnessDisplay.ResetFullness();
        gameObject.SetActive(false);
    }

    public void ShowStats(GameAsset asset)
    {
        gameObject.SetActive(true);
        current = asset;
        title.text = current.gameObject.name;
        description.text = current.description;
        if (asset.CompareTag("Rayc"))
        {
            title.text = current.GetComponent<Rayc>().raycName;
            numberDisplay.text = "";
            fullnessDisplay.gameObject.SetActive(true);
            fullnessDisplay.ShowFullness((Rayc)asset);
            statsDisplay.text = "Strength: " + ((Rayc)current).strength + "\n" + "Discovery: " + ((Rayc)current).discovery;
        }
        else if (asset.CompareTag("InteractableItem"))
        {
            fullnessDisplay.gameObject.SetActive(false);
            numberDisplay.text = "";
            statsDisplay.text = "";
        }
        else
        {
            fullnessDisplay.gameObject.SetActive(false);
            numberDisplay.text = FindObjectOfType<Player>().assetDict[current].ToString();
            statsDisplay.text = "";
        }
        ToggleButtons(asset);
        profileImage.sprite = current.GetComponent<Image>().sprite;
    }

    void ToggleButtons(GameAsset asset)
    {
        if (asset.CompareTag("Rayc"))
        {
            sellButton.interactable = true;
            useButton.interactable = false;
        }
        else if (asset.CompareTag("RecoveryItem"))
        {
            sellButton.interactable = false;
            useButton.interactable = true;
        }
        else if (!asset.CompareTag("RuneItem"))
        {
            sellButton.interactable = true;
            useButton.interactable = false;
        }
        else
        {
            sellButton.interactable = false;
            useButton.interactable = false; 
        }
    }

    public void OnSellButtonClick()
    {
        // TODO: Link with NFT transactions later
        Close();
    }

    public void OnUseButtonClick()
    {
       GameAssetList gameAssetList = FindObjectOfType<UIMonitor>().gameAssetList;
       gameAssetList.listType = ListType.Rayc;
       gameAssetList.gameObject.SetActive(true);
       Close();        
    }

}
