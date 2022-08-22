using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MoralisUnity;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Web3Api.Models;
using TMPro;

[Serializable]
public class RaycNftMetadata
{
    public string raycName;
    public string prefabName;
    public int fullness;
    public int strength;
    public int discovery;
    public string image;
}

[Serializable]
public class InteractableNftMetadata
{
    public string prefabName;
    public string image;
}

public class Player : MonoBehaviour
{
    public int coins;

    public List<GameAsset> assets;

    public Dictionary<GameAsset, int> assetDict;

    [SerializeField] TMP_Text coinAmount;

    void Awake()
    {
        assetDict = new Dictionary<GameAsset, int>();
    }

    void Start()
    {

        foreach (GameAsset asset in assets)
        {
            AddAsset(asset);
        }

        assets = assets.Distinct().ToList();

        UpdateCoinDisplay();
    }

    void UpdateCoinDisplay()
    {
        coinAmount.text = "Coins: " + coins.ToString();
    }

    public void AddCoins(int addition)
    {
        coins += addition;
        UpdateCoinDisplay();
    }

    public bool DeductCoins(int deduction)
    {
        if (coins - deduction >= 0)
        {
            coins -= deduction;
            UpdateCoinDisplay();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddAsset(GameAsset asset)
    {
        if (!assets.Contains(asset)) assets.Add(asset);

        if (!assetDict.ContainsKey(asset))
        {
            assetDict.Add(asset, 1);
        }
        else
        {
            assetDict[asset]++;
        }
    }

    public void RemoveAsset(GameAsset assetToRemove)
    {
        foreach(GameAsset asset in assets)
        {
            if (asset.gameObject.name == assetToRemove.gameObject.name)
            {
                if (assetDict[asset] == 1)
                {
                    assetDict.Remove(asset);
                    assets.Remove(asset);
                }
                else
                {
                    assetDict[asset]--;
                }
                break;
            }
        }
    }

    public void PutToInventory(GameAsset asset, bool rewardFromExpedition)
    {
        asset.gameObject.GetComponent<Draggable>()._dragSpot = null;
        GameObject newObject = FindObjectOfType<Inventory>().InstantiateToInventory(asset.gameObject);
        newObject.GetComponent<GameAsset>().ChangeToImageSpecs();
        newObject.name = asset.gameObject.name;
        newObject.GetComponent<Draggable>().enabled = true;
        AddAsset(newObject.GetComponent<GameAsset>());
        if (!rewardFromExpedition)
        {
            Destroy(asset.gameObject);
        }
    }

    public GameObject TakeOutFromInventory(GameAsset asset, Vector3 worldPosition, string parentName)
    {
        RemoveAsset(asset);
        GameObject newObject = Instantiate(asset.gameObject as GameObject, GameObject.Find(parentName).transform);
        newObject.GetComponent<GameAsset>().ChangeToSpriteSpecs();
        newObject.transform.localPosition = worldPosition;
        newObject.name = asset.gameObject.name;
        newObject.GetComponent<Draggable>().enabled = true;
        return newObject;
    }
}
