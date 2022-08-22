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

    [SerializeField] Button buyButton;

    DialogueBox dialogueBox;

    GameAsset current;

    TradableAsset tradableAsset;

    void Start()
    {
        dialogueBox = FindObjectOfType<UIMonitor>().dialogueBox;
    }

    public void Close()
    {
        if (fullnessDisplay.gameObject.activeSelf) fullnessDisplay.ResetFullness();
        gameObject.SetActive(false);
    }

    public void ShowStats(GameAsset asset)
    {
        current = asset;
        title.text = current.gameObject.name;
        description.text = current.description;
        ToggleButtons(asset);
        profileImage.sprite = current.GetComponent<Image>().sprite;
        gameObject.SetActive(true);
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
    }

    void ToggleButtons(GameAsset asset)
    {
        ToggleBuySellButton(asset);
        useButton.interactable = asset.CompareTag("RecoveryItem");
    }

    void ToggleBuySellButton(GameAsset asset)
    {
        if (asset.gameObject.layer == LayerMask.NameToLayer("ShopItem"))
        {
            sellButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(true);
        }
        else if (asset.CompareTag("RuneItem") || asset.CompareTag("RecoveryItem"))
        {
            sellButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(false);
        }
        else 
        {
            sellButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);
        }
    }

    public void OnSellButtonClick()
    {
        // TODO: Link with NFT transactions later
        Close();
    }

    public void OnBuyButtonClick()
    {
        dialogueBox.SetFunctionToYesButton(() =>
        {
            ShopManager shopManager = FindObjectOfType<ShopManager>();
            shopManager.PurchaseItem((TradableAsset) current);
        });
        dialogueBox.SetFunctionToYesButton(dialogueBox.HideDialogue);
        dialogueBox.SetFunctionToCloseButton(dialogueBox.HideDialogue);

        dialogueBox.ShowDialogue("Purchase Item", "Do you want to buy this NFT?", true);
        Close();
    }

    public void OnUseButtonClick()
    {
       GameAssetList gameAssetList = FindObjectOfType<UIMonitor>().gameAssetList;
       gameAssetList.listType = ListType.Rayc;
       gameAssetList.isHealing = true;
       gameAssetList.gameObject.SetActive(true);
       Close();        
    }

}
