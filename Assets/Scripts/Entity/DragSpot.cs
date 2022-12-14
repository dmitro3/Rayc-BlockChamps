using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragSpot : MonoBehaviour
{
    public bool isOccupied = false;
    
    protected Animator anim;

    public GameObject draggedObject;

    protected DragController dragController;

    public virtual void Awake()
    {
        anim = GetComponent<Animator>();
        dragController = GameObject.Find("DragController").GetComponent<DragController>();
    }

    public virtual void Update()
    {
        if (dragController._isDragActive && !isOccupied)
        {
            anim.SetBool("IsVisible", true);
        } 
        else 
        {
            anim.SetBool("IsVisible", false);
        }
    }

    public virtual void TakenBy(GameObject obj)
    {
        isOccupied = true;
        draggedObject = obj;
        obj.GetComponent<Draggable>()._dragSpot = gameObject;
        obj.transform.position = gameObject.transform.position;
    }

    public virtual void ClearObj()
    {
        isOccupied = false;
        draggedObject = null;
    }
}
