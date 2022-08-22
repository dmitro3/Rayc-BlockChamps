using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;
using MoralisUnity;
using MoralisUnity.Platform.Queries;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;
using TMPro;

class ShopManager : MonoBehaviour
{
    public string ContractAddress = "";
    public string ContractAbi = "";

    [SerializeField] FlexibleGridLayout raycItemList;

    [SerializeField] FlexibleGridLayout interactableItemList;

    [SerializeField] Player player;

    DialogueBox dialogueBox;

    MoralisQuery<RaycData> _getRaycQuery;

    MoralisQuery<InteractableData> _getInteractableQuery;

    MoralisLiveQueryCallbacks<RaycData> _callbacksRayc;

    MoralisLiveQueryCallbacks<InteractableData> _callbacksInteractable;

    List<RaycData> newRaycList = new List<RaycData>();
    List<RaycData> updatedRaycList = new List<RaycData>();
    List<string> deleteRaycList = new List<string>();

    List<InteractableData> newInteractableList = new List<InteractableData>();
    List<InteractableData> updateInteractableList = new List<InteractableData>();
    List<string> deleteInteractableList = new List<string>();

    void Start()
    {
        dialogueBox = FindObjectOfType<UIMonitor>().dialogueBox;
        SubscrbeToInteractableDatabaseEvents();
        SubscribeToRaycDatabaseEvents();
        GetGameAssetsFromDB();

    }

    void Update()
    {
        if (newRaycList.Count > 0)
        {
            foreach (RaycData raycData in newRaycList)
            {
                PopulateShopRaycItem(raycData);
            }
            newRaycList.Clear();
        }

        if (updatedRaycList.Count > 0)
        {
            foreach (RaycData raycData in updatedRaycList)
            {
                UpdateRaycItem(raycData.objectId, raycData);
            }
            updatedRaycList.Clear();
        }

        if (deleteRaycList.Count > 0)
        {
            foreach (string raycId in deleteRaycList)
            {
                DeleteRaycItem(raycId);
            }
            deleteRaycList.Clear();
        }

        if (newInteractableList.Count > 0)
        {
            foreach (InteractableData interactableData in newInteractableList)
            {
                PopulateShopInteractableItem(interactableData);
            }
            newInteractableList.Clear();
        }

        if (updateInteractableList.Count > 0)
        {
            foreach (InteractableData interactableData in updateInteractableList)
            {
                UpdateInteractableItem(interactableData.objectId, interactableData);
            }
            updateInteractableList.Clear();
        }

        if (deleteInteractableList.Count > 0)
        {
            foreach (string interactableId in deleteInteractableList)
            {
                DeleteInteractableItem(interactableId);
            }
            deleteInteractableList.Clear();
        }
    }

    #region MORALIS_SUBSCRIBERS
    
    public async void SubscribeToRaycDatabaseEvents()
    {
        _getRaycQuery = await Moralis.GetClient().Query<RaycData>();

        _callbacksRayc = new MoralisLiveQueryCallbacks<RaycData>();
        _callbacksRayc.OnConnectedEvent += (() => { Debug.Log("Connection To RaycData Established."); });
        _callbacksRayc.OnSubscribedEvent += ((requestId) => { Debug.Log($"Subscription {requestId} created to RaycData."); });
        _callbacksRayc.OnUnsubscribedEvent += ((requestId) => { Debug.Log($"Unsubscribed from {requestId}. (RaycData)"); });
        _callbacksRayc.OnCreateEvent += ((item, requestId) =>
        {
            Debug.Log("New data created on RaycData");
            newRaycList.Add(item);
        });
        _callbacksRayc.OnUpdateEvent += ((item, requestId) =>
        {
            Debug.Log("RaycData updated");
            updatedRaycList.Add(item);
        });
        _callbacksRayc.OnDeleteEvent += ((item, requestId) =>
        {
            Debug.Log("RaycData deleted");
            deleteRaycList.Add(item.objectId);
        });
        
        MoralisLiveQueryController.AddSubscription<RaycData>("RaycData", _getRaycQuery, _callbacksRayc);
    }

    public async void SubscrbeToInteractableDatabaseEvents()
    {
        _getInteractableQuery = await Moralis.GetClient().Query<InteractableData>();

        _callbacksInteractable = new MoralisLiveQueryCallbacks<InteractableData>();
        _callbacksInteractable.OnConnectedEvent += (() => { Debug.Log("Connection To InteractableData Established."); });
        _callbacksInteractable.OnSubscribedEvent += ((requestId) => { Debug.Log($"Subscription {requestId} created to InteractableData."); });
        _callbacksInteractable.OnUnsubscribedEvent += ((requestId) => { Debug.Log($"Unsubscribed from {requestId}. (InteractableData)"); });
        _callbacksInteractable.OnCreateEvent += ((item, requestId) =>
        {
            Debug.Log("New data created on InteractableData");
            newInteractableList.Add(item);
        });
        _callbacksInteractable.OnUpdateEvent += ((item, requestId) =>
        {
            Debug.Log("InteractableData updated");
            updateInteractableList.Add(item);
        });
        _callbacksInteractable.OnDeleteEvent += ((item, requestId) =>
        {
            Debug.Log("InteractableData deleted");
            deleteInteractableList.Add(item.objectId);
        });

        MoralisLiveQueryController.AddSubscription<InteractableData>("InteractableData", _getInteractableQuery, _callbacksInteractable);
    }

    #endregion

    #region SHOP_METHODS

    public async void GetGameAssetsFromDB()
    {
        IEnumerable<RaycData> raycData = await _getRaycQuery.FindAsync();
        IEnumerable<InteractableData> interactableData = await _getInteractableQuery.FindAsync();

        var raycDataList = raycData.ToList();
        var interactableDataList = interactableData.ToList();

        if (!raycDataList.Any() && !interactableDataList.Any()) return;

        foreach (var raycItem in raycDataList)
        {
            PopulateShopRaycItem(raycItem);
        }

        foreach (var interactableItem in interactableDataList)
        {
            PopulateShopInteractableItem(interactableItem);
        }
    }

    void PopulateShopRaycItem(RaycData raycData)
    {
        GameObject raycObj = Instantiate(Resources.Load("Prefabs/RaycPrefabs/" + raycData.prefabName) as GameObject, raycItemList.transform);
        Rayc rayc = raycObj.GetComponent<Rayc>();
        rayc.SetData(raycData);
        rayc.gameObject.GetComponent<Draggable>().enabled = false;
        rayc.ChangeToImageSpecs();
        rayc.gameObject.name = raycData.raycName;
        rayc.gameObject.layer = LayerMask.NameToLayer("ShopItem");
    }

    void PopulateShopInteractableItem(InteractableData interactableData)
    {
        GameObject interactableObj = Instantiate(Resources.Load("Prefabs/InteractablePrefabs/" + interactableData.prefabName) as GameObject, interactableItemList.transform);
        InteractableItem interactable = interactableObj.GetComponent<InteractableItem>();
        interactable.SetData(interactableData);
        interactable.gameObject.GetComponent<Draggable>().enabled = false;
        interactable.ChangeToImageSpecs();
        interactable.gameObject.layer = LayerMask.NameToLayer("ShopItem");
    }

    void UpdateRaycItem(string id, RaycData raycData)
    {
        try 
        {
            foreach (Transform child in raycItemList.transform)
            {
                if (child.gameObject.tag == "Rayc")
                {
                    Rayc rayc = child.GetComponent<Rayc>();
                    if (rayc.id.Equals(id))
                    {
                        rayc.SetData(raycData);
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    void UpdateInteractableItem(string id, InteractableData interactableData)
    {
        try
        {
            foreach (Transform child in interactableItemList.transform)
            {
                if (child.gameObject.tag == "InteractableItem")
                {
                    InteractableItem interactable = child.GetComponent<InteractableItem>();
                    if (interactable.id.Equals(id))
                    {
                        interactable.SetData(interactableData);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    void DeleteRaycItem(string id)
    {
        try
        {
            foreach (Transform child in raycItemList.transform)
            {
                Rayc rayc = child.GetComponent<Rayc>();
                if (rayc != null && rayc.id.Equals(id))
                {
                    Destroy(child.gameObject);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    void DeleteInteractableItem(string id)
    {
        try
        {
            foreach (Transform child in interactableItemList.transform)
            {
                InteractableItem interactable = child.GetComponent<InteractableItem>();
                if (interactable != null && interactable.id.Equals(id))
                {
                    Destroy(child.gameObject);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    #endregion

    #region NFT_METHODS

    public async void PurchaseItem(TradableAsset tradableAsset)
    {

        dialogueBox.ShowDialogue("", "Creating and saving metadata to IPFS...", false);
        
        var metadataUrl = await CreateIpfsMetadata(tradableAsset);

        if (metadataUrl is null)
        {
            dialogueBox.dialogueText.text = "Metadata couldn't be saved to IPFS";
            dialogueBox.SetFunctionToCloseButton(dialogueBox.HideDialogue);
            return;
        }

        dialogueBox.dialogueText.text = "Metadata saved successfully";

        // I'm assuming that this is creating a different tokenId from the already minted tokens in the contract.
        // I can do that because I know I'm converting a unique id coming from the MoralisDB.
        long tokenId = MoralisTools.ConvertStringToLong(tradableAsset.id);
        
        dialogueBox.dialogueText.text = "Please confirm transaction in your wallet";
        
        var result = await PurchaseItemContract(tokenId, metadataUrl);

        if (result is null)
        {
            dialogueBox.dialogueText.text = "Transaction failed";
            dialogueBox.SetFunctionToCloseButton(dialogueBox.HideDialogue);
            return;
        }
                
        dialogueBox.dialogueText.text = "Transaction completed!";
        dialogueBox.SetFunctionToCloseButton(dialogueBox.HideDialogue);
    }

    async UniTask<string> CreateIpfsMetadata(TradableAsset tradableAsset)
    {
        // 1. Build Metadata
        object metadata = null;
        if (tradableAsset.CompareTag("Rayc"))
        {
            Rayc rayc = (Rayc) tradableAsset;
            metadata = MoralisTools.BuildRaycMetadata(rayc.raycName, rayc.prefabName, rayc.fullness, rayc.strength, rayc.discovery, rayc.imageUrl);
        }
        else
        {
            InteractableItem interactable = (InteractableItem) tradableAsset;
            metadata = MoralisTools.BuildInteractableMetadata(interactable.prefabName, tradableAsset.imageUrl);
        }

        string metadataName = $"{tradableAsset.name}_{tradableAsset.id}.json";

        // 2. Encoding JSON
        string json = JsonConvert.SerializeObject(metadata);
        string base64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
      
        // 3. Save metadata to IPFS
        string ipfsMetadataPath = await MoralisTools.SaveToIpfs(metadataName, base64Data);

        return ipfsMetadataPath;
    }

    async Task<string> PurchaseItemContract(BigInteger tokenId, string metadataUrl)
    {
        byte[] data = Array.Empty<byte>();
        
        object[] parameters = {
            tokenId.ToString("x"),
            metadataUrl,
            data
        };

        // Set gas estimate
        HexBigInteger value = new HexBigInteger("0x0");
        HexBigInteger gas = new HexBigInteger(0);
        HexBigInteger gasPrice = new HexBigInteger("0x0");

        ShopManager shopManager = FindObjectOfType<ShopManager>();

        string resp = await Moralis.ExecuteContractFunction(shopManager.ContractAddress, shopManager.ContractAbi, "buyItem", parameters, value, gas, gasPrice);
        
        return resp;
    }

    #endregion
}