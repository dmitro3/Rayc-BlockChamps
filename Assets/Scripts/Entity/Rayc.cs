using System.Collections;
using System.Collections.Generic;
using System;
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
    public int fullness;

    public int strength, discovery;

    public InteractionEvent interactionEvent = null;

    public bool justEndedInteraction = false;

    // need more variables for interaction events

    public virtual void Start()
    {
        anim = GetComponent<Animator>();
        effect = new Serendipity();
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
