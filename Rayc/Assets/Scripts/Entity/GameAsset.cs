using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GameAsset : MonoBehaviour, System.IEquatable<GameAsset>
{
    public string id;
    
    public string prefabName;

    public string description;

    public bool Equals(GameAsset other)
    {
        return id.Equals(other.id);
    }

    public override bool Equals(object obj)
    {
        if (obj is GameAsset)
        {
            return Equals(obj as GameAsset);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return id.GetHashCode();
    }
}
