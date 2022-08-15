using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConsumableItem : GameAsset, IEquatable<ConsumableItem>
{
    public bool Equals(ConsumableItem other)
    {
        return prefabName.Equals(other.prefabName);
    }

    public override bool Equals(object obj)
    {
        if (obj is ConsumableItem)
        {
            return Equals(obj as ConsumableItem);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return prefabName.GetHashCode();
    }
}
