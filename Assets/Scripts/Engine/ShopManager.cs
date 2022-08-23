using System.IO.Enumeration;
using System.Diagnostics;
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
using MoralisUnity.Web3Api.Models;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;
using TMPro;

class ShopManager : MonoBehaviour
{
    public string ContractAddress;
    public string ContractAbi;

    public ChainList chainList = ChainList.mumbai;

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
        ContractAddress = "0x9C0387c5180bbe2E0e5aab25EA1C646608aB2580";
        ContractAbi = "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"ApprovalForAll\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"indexed\":false,\"internalType\":\"uint256[]\",\"name\":\"values\",\"type\":\"uint256[]\"}],\"name\":\"TransferBatch\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"TransferSingle\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":false,\"internalType\":\"string\",\"name\":\"value\",\"type\":\"string\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"URI\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address[]\",\"name\":\"accounts\",\"type\":\"address[]\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"}],\"name\":\"balanceOfBatch\",\"outputs\":[{\"internalType\":\"uint256[]\",\"name\":\"\",\"type\":\"uint256[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"_tokenId\",\"type\":\"uint256\"},{\"internalType\":\"string\",\"name\":\"_tokenUrl\",\"type\":\"string\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"buyItem\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"}],\"name\":\"isApprovedForAll\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"renounceOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"internalType\":\"uint256[]\",\"name\":\"amounts\",\"type\":\"uint256[]\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeBatchTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"setApprovalForAll\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes4\",\"name\":\"interfaceId\",\"type\":\"bytes4\"}],\"name\":\"supportsInterface\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"uri\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
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
        _callbacksRayc.OnConnectedEvent += (() => { UnityEngine.Debug.Log("Connection To RaycData Established."); });
        _callbacksRayc.OnSubscribedEvent += ((requestId) => { UnityEngine.Debug.Log($"Subscription {requestId} created to RaycData."); });
        _callbacksRayc.OnUnsubscribedEvent += ((requestId) => { UnityEngine.Debug.Log($"Unsubscribed from {requestId}. (RaycData)"); });
        _callbacksRayc.OnCreateEvent += ((item, requestId) =>
        {
            UnityEngine.Debug.Log("New data created on RaycData");
            newRaycList.Add(item);
        });
        _callbacksRayc.OnUpdateEvent += ((item, requestId) =>
        {
            UnityEngine.Debug.Log("RaycData updated");
            updatedRaycList.Add(item);
        });
        _callbacksRayc.OnDeleteEvent += ((item, requestId) =>
        {
            UnityEngine.Debug.Log("RaycData deleted");
            deleteRaycList.Add(item.objectId);
        });
        
        MoralisLiveQueryController.AddSubscription<RaycData>("RaycData", _getRaycQuery, _callbacksRayc);
    }

    public async void SubscrbeToInteractableDatabaseEvents()
    {
        _getInteractableQuery = await Moralis.GetClient().Query<InteractableData>();

        _callbacksInteractable = new MoralisLiveQueryCallbacks<InteractableData>();
        _callbacksInteractable.OnConnectedEvent += (() => { UnityEngine.Debug.Log("Connection To InteractableData Established."); });
        _callbacksInteractable.OnSubscribedEvent += ((requestId) => { UnityEngine.Debug.Log($"Subscription {requestId} created to InteractableData."); });
        _callbacksInteractable.OnUnsubscribedEvent += ((requestId) => { UnityEngine.Debug.Log($"Unsubscribed from {requestId}. (InteractableData)"); });
        _callbacksInteractable.OnCreateEvent += ((item, requestId) =>
        {
            UnityEngine.Debug.Log("New data created on InteractableData");
            newInteractableList.Add(item);
        });
        _callbacksInteractable.OnUpdateEvent += ((item, requestId) =>
        {
            UnityEngine.Debug.Log("InteractableData updated");
            updateInteractableList.Add(item);
        });
        _callbacksInteractable.OnDeleteEvent += ((item, requestId) =>
        {
            UnityEngine.Debug.Log("InteractableData deleted");
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

    public async void DeleteRaycFromDB(string id)
    {
        IEnumerable<RaycData> result = await _getRaycQuery.FindAsync();
        foreach (RaycData raycData in result)
        {
            if (raycData.objectId.Equals(id))
            {
                await raycData.DeleteAsync();
                break;
            }
        }
    }

    public async void DeleteInteractableFromDB(string id)
    {
        IEnumerable<InteractableData> result = await _getInteractableQuery.FindAsync();
        foreach (InteractableData interactableData in result)
        {
            if (interactableData.objectId.Equals(id))
            {
                await interactableData.DeleteAsync();
                break;
            }
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
        rayc.GetComponent<BoxCollider2D>().size = raycItemList.cellSize;
    }

    void PopulateShopInteractableItem(InteractableData interactableData)
    {
        GameObject interactableObj = Instantiate(Resources.Load("Prefabs/InteractablePrefabs/" + interactableData.prefabName) as GameObject, interactableItemList.transform);
        InteractableItem interactable = interactableObj.GetComponent<InteractableItem>();
        interactable.SetData(interactableData);
        interactable.gameObject.GetComponent<Draggable>().enabled = false;
        interactable.ChangeToImageSpecs();
        interactable.gameObject.layer = LayerMask.NameToLayer("ShopItem");
        interactable.GetComponent<BoxCollider2D>().size = interactableItemList.cellSize;
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
            UnityEngine.Debug.Log(e);
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
            UnityEngine.Debug.Log(e);
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
            UnityEngine.Debug.Log(e);
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
            UnityEngine.Debug.Log(e);
        }
    }

    #endregion

    #region NFT_METHODS

    public async void PurchaseItem(TradableAsset tradableAsset)
    {
        UnityEngine.Debug.Log("tradable asset image url is: " + tradableAsset.imageUrl);
        dialogueBox.HideDialogue();
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

        UnityEngine.Debug.Log(result);

        if (result is null)
        {
            dialogueBox.dialogueText.text = "Transaction failed";
            dialogueBox.SetFunctionToCloseButton(dialogueBox.HideDialogue);
            return;
        }
                
        dialogueBox.dialogueText.text = "Transaction completed!";


        dialogueBox.SetFunctionToCloseButton(() => {
            OfferOpenSeaView(tokenId.ToString());
        });

        // Delete from database
        if (tradableAsset.CompareTag("Rayc"))
        {
            DeleteRaycFromDB(tradableAsset.id);
        }
        else
        {
            DeleteInteractableFromDB(tradableAsset.id);
        }

        // Add to player inventory
        Player player = FindObjectOfType<Player>();
        player.PutToInventory(tradableAsset, false);

    }

    public void OfferOpenSeaView(string tokenId)
    {
        dialogueBox.HideDialogue();
        ShopManager shopManager = FindObjectOfType<ShopManager>();
        dialogueBox.SetFunctionToYesButton(() => {
            MoralisTools.CheckNftOnOpenSea(shopManager.ContractAddress, shopManager.chainList.ToString(), tokenId);
        });
        dialogueBox.SetFunctionToYesButton(dialogueBox.HideDialogue);

        dialogueBox.SetFunctionToCloseButton(dialogueBox.HideDialogue);
        dialogueBox.ShowDialogue("View On OpenSea", "Do you want to view this NFT on OpenSea?", true);
    }

    async UniTask<string> CreateIpfsMetadata(TradableAsset tradableAsset)
    {
        // 1. Build Metadata
        object metadata = null;
        // if (tradableAsset.CompareTag("Rayc"))
        // {
        //     Rayc rayc = (Rayc) tradableAsset;
        //     metadata = MoralisTools.BuildRaycMetadata(rayc.raycName, rayc.prefabName, rayc.fullness, rayc.strength, rayc.discovery, rayc.imageUrl);
        // }
        // else
        // {
        //     InteractableItem interactable = (InteractableItem) tradableAsset;
        //     metadata = MoralisTools.BuildInteractableMetadata(interactable.prefabName, tradableAsset.imageUrl);
        // }

        metadata = MoralisTools.BuildMetadata(tradableAsset.prefabName, tradableAsset.prefabName, tradableAsset.imageUrl);

        string metadataName = $"{tradableAsset.name}_{tradableAsset.id}.json";

        // 2. Encoding JSON
        string json = JsonConvert.SerializeObject(metadata);
        UnityEngine.Debug.Log(json);
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