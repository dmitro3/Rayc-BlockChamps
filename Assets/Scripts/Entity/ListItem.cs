using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ListItem : MonoBehaviour
{
    [SerializeField] TMP_Text itemName;

    [SerializeField] TMP_Text itemDescription;

    [SerializeField] FullnessDisplay fullnessDisplay;

    [SerializeField] Image itemImage;

    public Rayc rayc;

    public RuneItem rune;

    public RecoveryItem recovery;

    public void SetRaycValues(Rayc _rayc)
    {
        rayc = _rayc;
        itemName.text = rayc.name;
        itemDescription.text = "Strength: " + rayc.strength + ", Discovery: " + rayc.discovery;
        itemImage.sprite = rayc.GetComponent<Image>().sprite;
        fullnessDisplay.gameObject.SetActive(true);
        fullnessDisplay.ShowFullness(rayc);
    }

    public void SetRuneValues(RuneItem _rune)
    {
        rune = _rune;
        itemName.text = rune.name;
        itemDescription.text = rune.description;
        itemImage.sprite = rune.GetComponent<Image>().sprite;
        fullnessDisplay.gameObject.SetActive(false);
    }

    public void SetRecoveryValues(RecoveryItem _recov)
    {
        recovery = _recov;
        itemName.text = recovery.name;
        itemDescription.text = recovery.description;
        itemImage.sprite = recovery.GetComponent<Image>().sprite;
        fullnessDisplay.gameObject.SetActive(false);
    }

}