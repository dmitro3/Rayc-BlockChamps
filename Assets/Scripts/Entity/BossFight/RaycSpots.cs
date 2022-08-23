using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycSpots : MonoBehaviour
{
    [SerializeField] Image spotOne;
    [SerializeField] Image spotTwo;
    [SerializeField] Image spotThree;
    List<Rayc> raycs = new List<Rayc>();

    void Start()
    {
        spotOne.enabled = false;
        spotTwo.enabled = false;
        spotThree.enabled = false;
    }

    public void AssignRaycs(List<Rayc> raycs)
    {
        DestroyPrevRaycs();
        int size = raycs.Count;
        if (0 < size)
            InstantiateRaycAtPos(raycs[0], spotOne.transform);
        if (1 < size)
            InstantiateRaycAtPos(raycs[1], spotTwo.transform);
        if (2 < size)
            InstantiateRaycAtPos(raycs[2], spotThree.transform);
    }

    private void DestroyPrevRaycs()
    {
        raycs.ForEach(rayc => Object.Destroy(rayc.gameObject));
        raycs.Clear();
    }

    private void InstantiateRaycAtPos(Rayc raycPrefab, Transform transform)
    {
        Rayc rayc = Instantiate(raycPrefab, transform).GetComponent<Rayc>();
        rayc.GetComponent<Image>().enabled = false;
        SpriteRenderer spriteRenderer = rayc.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        spriteRenderer.sortingLayerName = "Expedition";
        rayc.GetComponent<RectTransform>().localScale = new Vector3(10.0f, 10.0f, 1.0f);
        rayc.clickable = false;
        rayc.ChangePoise(Poise.Back);
        raycs.Add(rayc);
    }
}
