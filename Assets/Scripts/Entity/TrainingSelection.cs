using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingSelection : MonoBehaviour
{
    Vector3 originalLocalScale;

    void Awake()
    {
        originalLocalScale = transform.localScale;
    }

    void OnMouseOver() 
    {
        transform.localScale = new Vector3(originalLocalScale.x * 1.15f, originalLocalScale.y * 1.15f, originalLocalScale.z);
    }

    void OnMouseExit()
    {
        transform.localScale = originalLocalScale;
    }
}
