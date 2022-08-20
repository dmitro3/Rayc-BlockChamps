using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycSpots : MonoBehaviour
{
    [SerializeField] Image spotOne;
    [SerializeField] Image spotTwo;
    [SerializeField] Image spotThree;

    public void AssignRaycs(List<Sprite> raycs)
    {
        int size = raycs.Count;
        spotOne.sprite = null;
        spotTwo.sprite = null;
        spotThree.sprite = null;
        if (0 < size) spotOne.sprite = raycs[0];
        if (1 < size) spotTwo.sprite = raycs[1];
        if (2 < size) spotThree.sprite = raycs[2];
    }
}
