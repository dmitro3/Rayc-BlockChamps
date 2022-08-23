using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoralisUnity.Platform.Objects;

public class InteractableData : MoralisObject
{
    public string prefabName { get; set; }

    public string imageUrl { get; set; }

    public InteractableData() : base("InteractableData") {}
}