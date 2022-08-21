using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TreeNode : MonoBehaviour
{
    public TreeNode parent;

    public List<TreeNode> children;

    public GameObject nodePrefab;

    public bool mastered = false;

    public bool isActive = false;

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
        Debug.Log("Change of profession triggered!");
        if (nodePrefab != null)
        {
            FindObjectOfType<ProfessionTree>().ChangeProfession(this);
        }
    }
}