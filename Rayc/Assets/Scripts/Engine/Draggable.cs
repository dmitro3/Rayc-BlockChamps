using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public bool isDragging;

    public Vector3 lastPosition;

    Collider2D _collider;

    UIMonitor _uiMonitor;

    public GameObject _dragSpot = null; 

    const float _movementTime = 15f;

    public System.Nullable<Vector2> _movementDestination;

    void Awake()
    {
        _collider = GetComponent<Collider2D>(); 
        _uiMonitor = FindObjectOfType<UIMonitor>();
    }

    void FixedUpdate()
    {
        if (_movementDestination.HasValue) 
        {
            if (isDragging)
            {
                _movementDestination = null;
                return;
            } 

            if (transform.position == _movementDestination)
            {
                gameObject.layer = LayerMask.NameToLayer("PlacedItem");
                _movementDestination = null;
            } 
            else
            {
                transform.position = Vector3.Lerp(transform.position, _movementDestination.Value, _movementTime * Time.fixedDeltaTime);
            }
        }
    }
}
