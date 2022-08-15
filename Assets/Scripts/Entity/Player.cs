using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int coins;

    public List<GameAsset> assets;

    public Dictionary<GameAsset, int> assetDict;

    void Awake()
    {
        coins = 100;
        assetDict = new Dictionary<GameAsset, int>();
    }

    void Start()
    {
        foreach (GameAsset asset in assets)
        {
            assetDict.Add(asset, 1);
        }
    }

    public void AddCoins(int addition)
    {
        coins += addition;
    }

    public bool DeductCoins(int deduction)
    {
        if (coins - deduction >= 0)
        {
            coins -= deduction;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddAsset(GameAsset asset)
    {
        assets.Add(asset);

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
                assets.Remove(asset);

                if (assetDict[asset] == 1)
                {
                    assetDict.Remove(asset);
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
