using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfessionTree : MonoBehaviour
{
    public Rayc rayc;

    public TreeNode node;

    [SerializeField] GameObject tree;

    // needs optimization
    void Update()
    {
        if (rayc != null) UpdateTreeNode();
    }

    void OnDisable()
    {
        rayc = null;
        ClearNodeSelection();
        TurnOffNodes();
    }

    void UpdateTreeNode()
    {
        foreach (Transform child in tree.transform)
        {
            if (child.gameObject.name.Equals(rayc.prefabName))
            {
                TurnOnNodes(child.GetComponent<TreeNode>());
            }
        }
    }

    void TurnOffNodes()
    {
        foreach (Transform child in tree.transform)
        {
            child.GetComponent<TreeNode>().isActive = false;
        }
    }

    void TurnOnNodes(TreeNode source)
    {
        TreeNode cur = source;
        while (cur != null)
        {
            cur.isActive = true;
            cur.mastered = true;
            cur = cur.parent;
        }

        foreach (TreeNode child in source.children)
        {
            child.GetComponent<TreeNode>().isActive = true;
        }
    }

    void ClearNodeSelection()
    {
        node = null;
    }

    public void CheckProfessionChange()
    {
        Rayc prefabRayc = node.nodePrefab.GetComponent<Rayc>();

        DialogueBox dialogueBox = FindObjectOfType<UIMonitor>().dialogueBox;

        if (rayc.prefabName.Equals(prefabRayc.prefabName))
        {
            dialogueBox.SetFunctionToCloseButton(dialogueBox.HideDialogue);
            dialogueBox.ShowDialogue("Invalid Change", "Selected Rayc is already in this profession!", false);
            return;
        }

        int requiredStrength = prefabRayc.strength;
        int requiredDiscovery = prefabRayc.discovery;

        if (rayc.strength < requiredStrength || rayc.discovery < requiredDiscovery)
        {
            dialogueBox.SetFunctionToCloseButton(dialogueBox.HideDialogue);
            dialogueBox.ShowDialogue("Insufficient Stats", "Selected Rayc doesn't have enough strength or discovery to change to this profession!", false);
            return;
        }
        else if (FindObjectOfType<Player>().coins < node.requiredCoins)
        {
            dialogueBox.SetFunctionToCloseButton(dialogueBox.HideDialogue);
            dialogueBox.ShowDialogue("Insufficient Coins", "You don't have enough coins to change professions!", false);
            return;
        }
        else
        {
            dialogueBox.SetFunctionToCloseButton(ClearNodeSelection);
            dialogueBox.SetFunctionToCloseButton(dialogueBox.HideDialogue);
            
            dialogueBox.SetFunctionToYesButton(ChangeProfession);
            dialogueBox.SetFunctionToYesButton(dialogueBox.HideDialogue);
            dialogueBox.ShowDialogue("Change of Profession", "Are you sure you want to change to this profession with " + node.requiredCoins + " coins?", true);
        }
    }

    public void ChangeProfession()
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        GameObject raycObj = inventory.InstantiateToInventory(node.nodePrefab);
        Rayc raycObjScript = raycObj.GetComponent<Rayc>();
        raycObjScript.SetData(rayc);
        raycObjScript.fullness--;
        raycObjScript.ChangeToImageSpecs();
        raycObj.name = rayc.raycName;
        raycObj.GetComponent<Draggable>().enabled = true;
        Player player = FindObjectOfType<Player>();
        player.AddAsset(raycObjScript);
        player.RemoveAsset(rayc);
        player.DeductCoins(node.requiredCoins);
        inventory.RemoveTradableFromInventory(rayc);

        rayc = raycObj.GetComponent<Rayc>();
        TurnOffNodes();
        TurnOnNodes(node);
        ClearNodeSelection();
    }
}