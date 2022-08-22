using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class EffectSummary : MonoBehaviour
{
    private TMP_Text effectSummary;
    private GameObject raycSelection;
    public List<Effect> summaryList;

    Dictionary<string, int> effectDictionary;

    void Start()
    {
        effectSummary = GameObject.Find("EffectSummary").GetComponent<TMP_Text>();
        raycSelection = GameObject.Find("RaycSelection");
        summaryList = GameObject.Find("ExpeditionManager").GetComponent<ExpeditionManager>().effectList;
        effectDictionary = GameObject.Find("ExpeditionManager").GetComponent<ExpeditionManager>().effectDictionary;
    }

    void Update()
    {
        string summary = "Drop a Rayc to view effects...";
        if (summaryList.Count > 0)
        {
            summary = "";
            foreach (Effect effect in summaryList.GroupBy(x => x.name).Select(x => x.First()))
            {
                summary += "* " + effect.name + EffectChanceDescriptor(effect.name) + ": " + effect.description + "\n";
            }
        }
        effectSummary.text = summary;
    }

    string EffectChanceDescriptor(string effectName)
    {
        if (effectDictionary[effectName] == 1) {
            return "";
        } else if (effectDictionary[effectName] == 2) {
            return "+";
        } else {
            return "++";
        }
    }
}
