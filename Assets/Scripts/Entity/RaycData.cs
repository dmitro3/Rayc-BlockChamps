using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoralisUnity.Platform.Objects;

public class RaycData : MoralisObject
{
    public string prefabName { get; set;}
    public string raycName { get; set; }
    public int fullness { get; set; }
    public int strength { get; set; }
    public int discovery { get; set; }

    public RaycData() : base("RaycData") {}
}
