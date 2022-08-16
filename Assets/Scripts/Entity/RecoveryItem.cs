using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RecoveryItem : ConsumableItem
{  
    GameAssetList gameAssetList;

    [SerializeField] int recoveryAmount;

    void Start()
    {
        gameAssetList = FindObjectOfType<UIMonitor>().gameAssetList;
    }

    void Update()
    {
        if (gameAssetList.selectedRayc != null)
        {
            // TODO: check if involved in dialogue in the future

            UseItem();
        }
    }

    void UseItem()
    {
        Player player = FindObjectOfType<Player>();
        if (player.assetDict[this] == 1) FindObjectOfType<Inventory>().RemoveConsumableFromInventory(this);
        player.RemoveAsset(this);
        gameAssetList.selectedRayc.fullness += Math.Min(recoveryAmount, 5 - gameAssetList.selectedRayc.fullness);
        gameAssetList.ClearSelectedRayc();
        gameAssetList.gameObject.SetActive(false);
    }
}
