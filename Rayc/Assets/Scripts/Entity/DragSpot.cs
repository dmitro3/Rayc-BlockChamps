using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragSpot : MonoBehaviour
{
    public bool isOccupied = false;
    
    protected Animator anim;

    public GameObject draggedObject;

    protected DragController dragController;

    protected bool isChildFloor = false;

    public virtual void Awake()
    {
        anim = GetComponent<Animator>();
        dragController = GameObject.Find("DragController").GetComponent<DragController>();
        if (transform.parent.name == "FloorSpots")
        {
            isChildFloor = true;
        }
    }

    public virtual void Update()
    {
        if (dragController._isDragActive && !isOccupied && !isChildFloor)
        {
            anim.SetBool("IsVisible", true);
        } 
        else 
        {
            anim.SetBool("IsVisible", false);
        }
    }

    public virtual void takenBy(GameObject obj)
    {
        isOccupied = true;
        draggedObject = obj;
        obj.GetComponent<Draggable>()._dragSpot = gameObject;
    }

    public virtual void clearObj()
    {
        isOccupied = false;
        draggedObject = null;
    }
}
