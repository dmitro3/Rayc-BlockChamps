using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreeNode : MonoBehaviour
{
    public TreeNode parent;

    public List<TreeNode> children;

    public int requiredCoins;

    public GameObject nodePrefab;

    public bool mastered = false;

    public bool isActive = false;

    [SerializeField] ProfessionTree professionTree;

    [SerializeField] GameObject popout;

    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        popout.SetActive(false);
    }

    void Update()
    {
        button.interactable = isActive;
    }

    void OnMouseEnter()
    {
        TMP_Text popoutText = popout.GetComponentInChildren<TMP_Text>();
        Rayc rayc = nodePrefab.GetComponent<Rayc>();
        popoutText.text = "Require:\n" + "Strength: " + rayc.strength + "\n" + "Discovery: " + rayc.discovery + "\n" + "Coins: " + requiredCoins;
        popout.SetActive(true);
    }

    void OnMouseExit()
    {
        popout.SetActive(false);
    }

    public void TriggerProfessionChange()
    {
        if (nodePrefab != null)
        {
            professionTree.node = this;
            professionTree.CheckProfessionChange();
        }
    }
}