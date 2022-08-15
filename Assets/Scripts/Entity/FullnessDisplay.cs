using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullnessDisplay : MonoBehaviour
{
    Sprite[] heartSprites; // index 0 is filled, 1 is empty

    void Awake()
    {
        heartSprites = Resources.LoadAll<Sprite>("Sprites/UI/Heart");
    }
 
    public void ShowFullness(Rayc rayc)
    {
        int fullness = rayc.fullness;

        for (int i = 0; i < fullness; i++)
        {
            transform.GetChild(i).GetComponent<Image>().sprite = heartSprites[0];
        }
    }

    public void ResetFullness()
    {
        foreach (Image child in GetComponentsInChildren<Image>())
        {
            child.sprite = heartSprites[1];
        }
    }
}
