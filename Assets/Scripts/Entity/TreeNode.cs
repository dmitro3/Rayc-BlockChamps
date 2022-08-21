using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TreeNode : MonoBehaviour
{
    public TreeNode parent;

    public List<TreeNode> children;

    public int requiredCoins;

    public GameObject nodePrefab;

    public bool mastered = false;

    public bool isActive = false;

    [SerializeField] ProfessionTree professionTree;

    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        button.interactable = isActive;
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