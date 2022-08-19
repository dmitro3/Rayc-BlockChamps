using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfessionTree : MonoBehaviour
{
    public Rayc rayc;

    [SerializeField] GameObject tree;

    // needs optimization
    void Update()
    {
        if (rayc != null) UpdateTreeNode();
    }

    void OnDisable()
    {
        rayc = null;
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

    public void ChangeProfession(TreeNode node)
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        GameObject raycObj = inventory.InstantiateToInventory(node.nodePrefab);
        Rayc raycObjScript = raycObj.GetComponent<Rayc>();
        raycObjScript.SetData(rayc);
        raycObjScript.ChangeToImageSpecs();
        raycObj.name = rayc.raycName;
        raycObj.GetComponent<Draggable>().enabled = true;
        Player player = FindObjectOfType<Player>();
        player.AddAsset(raycObjScript);
        player.RemoveAsset(rayc);
        inventory.RemoveTradableFromInventory(rayc);

        rayc = raycObj.GetComponent<Rayc>();
        TurnOffNodes();
        TurnOnNodes(node);
    }
}