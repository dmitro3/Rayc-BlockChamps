using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TradableAsset : GameAsset, IEquatable<TradableAsset>
{
    public string id;

    public bool Equals(TradableAsset other)
    {
        return id.Equals(other.id);
    }

    public override bool Equals(object obj)
    {
        if (obj is TradableAsset)
        {
            return Equals(obj as TradableAsset);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return id.GetHashCode();
    }
}
