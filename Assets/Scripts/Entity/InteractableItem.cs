using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using MoralisUnity;

public class InteractableItem : TradableAsset
{
    public void SetData(InteractableData interactableData)
    {
        id = interactableData.objectId;
        prefabName = interactableData.prefabName;
        description = interactableData.description;
    }

     public async void SaveInteractableItemToDB()
    {
        try 
        {
            InteractableData interactableData = Moralis.Create<InteractableData>();
            interactableData.prefabName = prefabName;
            interactableData.description = description;
            await interactableData.SaveAsync();
            Debug.Log("InteractableData uploaded to database.");
        }
        catch (Exception e)
        {
            Debug.Log("InteractableData upload failed: " + e.Message);
        }
    }
}