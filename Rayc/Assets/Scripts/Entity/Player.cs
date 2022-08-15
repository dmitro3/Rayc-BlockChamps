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

    public void addCoins(int addition)
    {
        coins += addition;
    }

    public bool deductCoins(int deduction)
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

    public void addAsset(GameAsset asset)
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

    public void removeAsset(GameAsset assetToRemove)
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
}
