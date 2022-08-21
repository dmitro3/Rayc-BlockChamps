using System.Collections;
using System.Collections.Generic;
using System;
using MoralisUnity;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Poise
{
    Front,
    SideLeft,
    ProfileLeft,
    SideRight,
    ProfileRight,
    Back,
}

public class Rayc : TradableAsset
{
    Animator anim;

    public Effect effect;

    public bool idleOnly = false;

    public string raycName;

    public string raycDescription;
    public int fullness;

    public int strength, discovery;

    public InteractionEvent interactionEvent = null;

    public bool justEndedInteraction = false;

    public Vector3 anchorOffset;

    // need more variables for interaction events

    public virtual void Start()
    {
        anim = GetComponent<Animator>();
        effect = new Serendipity();
    }

    public void SetData(RaycData raycData)
    {
        id = raycData.objectId;
        prefabName = raycData.prefabName;
        raycName = raycData.raycName;
        raycDescription = raycData.raycDescription;
        fullness = raycData.fullness;
        strength = raycData.strength;
        discovery = raycData.discovery;
    }

    public async void SaveRaycToDB()
    {
        try 
        {
            RaycData raycData = Moralis.Create<RaycData>();
            raycData.prefabName = prefabName;
            raycData.raycName = raycName;
            raycData.raycDescription = raycDescription;
            raycData.fullness = fullness;
            raycData.strength = strength;
            raycData.discovery = discovery;
            await raycData.SaveAsync();
            Debug.Log("RaycData uploaded to database.");
        }
        catch (Exception e)
        {
            Debug.Log("RaycData upload failed: " + e.Message);
        }
    }


    public void SetData(Rayc rayc)
    {
        id = rayc.id;
        raycName = rayc.raycName;
        fullness = rayc.fullness;
        strength = rayc.strength;
        discovery = rayc.discovery;
    }

    public void ChangePoise(Poise poise)
    {
        Sprite[] allPoises = Resources.LoadAll<Sprite>("Sprites/Poise/" + prefabName);
        GetComponent<SpriteRenderer>().sprite = allPoises[(int)poise];
    }

    public void RevertToOriginalSprite()
    {
        Sprite[] allSprites = Resources.LoadAll<Sprite>("Sprites/Rayc/" + prefabName);
        GetComponent<SpriteRenderer>().sprite = allSprites[0];
    }
}
