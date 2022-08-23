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
        spotOne.enabled = false;
        spotTwo.enabled = false;
        spotThree.enabled = false;
        if (0 < size)
        {
            spotOne.enabled = true;
            spotOne.sprite = raycs[0];
        }
        if (1 < size)
        {
            spotTwo.enabled = true;
            spotTwo.sprite = raycs[1];
        }
        if (2 < size)
        {
            spotThree.enabled = true;
            spotThree.sprite = raycs[2];
        }
    }
}
