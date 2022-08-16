using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragController : MonoBehaviour
{
    public Draggable lastDragged => _lastDragged;

    public bool _isDragActive = false;

    Vector2 _screenPosition;

    Vector3 _worldPosition;

    Draggable _lastDragged;

    Inventory inventory;

    Bin bin;

    float requiredPressDuration = 0.4f;

    bool isPressed = false;

    float pressDuration = 0f;

    Vector3 scaleChange = new Vector3(1.3f, 1.3f, 1f);

    GameObject unsettledUIDrag = null;

    Player player;

    int PLACEDITEM;

    int INVENTORYITEM;

    int DRAGGING;
    

    void Awake()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        bin = GameObject.Find("Bin").GetComponent<Bin>();
        bin.gameObject.SetActive(false);

        // getting layer integers
        PLACEDITEM = LayerMask.NameToLayer("PlacedItem");
        INVENTORYITEM = LayerMask.NameToLayer("InventoryItem");
        DRAGGING = LayerMask.NameToLayer("Dragging");
    }

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        if ((Input.GetMouseButtonUp(0)))
        {
            AssetStats assetStats = FindObjectOfType<UIMonitor>().assetStats;
            if (pressDuration < requiredPressDuration && !assetStats.gameObject.activeSelf) 
            {
                Vector3 mousePos = Input.mousePosition;
                _screenPosition = new Vector2(mousePos.x, mousePos.y);
                RaycastHit2D[] rays = Physics2D.RaycastAll(_worldPosition, Vector2.zero, Mathf.Infinity);

                // prioritize rayc over items
                Rayc raycFound = null;
                GameAsset itemFound = null;
                foreach (RaycastHit2D ray in rays)
                {
                    if (ray.collider.GetComponent<GameAsset>() != null) 
                    {
                        if (ray.collider.CompareTag("Rayc"))
                        {
                            raycFound = ray.collider.GetComponent<Rayc>();
                            break;
                        }
                        else
                        {
                            itemFound = ray.collider.GetComponent<GameAsset>();
                        }
                    }
                }

                if (raycFound != null)
                {
                    assetStats.ShowStats(raycFound);
                }
                else if (itemFound != null)
                {
                    assetStats.ShowStats(itemFound);
                }
            }

            isPressed = false;
            pressDuration = 0f;
            if (_isDragActive)
            {
                Drop();
                _lastDragged.transform.localScale -= scaleChange;
                return;
            }
        }
        if (Input.GetMouseButton(0)) {
            isPressed = true;
            Vector3 mousePos = Input.mousePosition;
            _screenPosition = new Vector2(mousePos.x, mousePos.y);
        } else if (Input.touchCount > 0) {
            isPressed = true;
            _screenPosition = Input.GetTouch(0).position;
        } else {
            return;
        }

        _worldPosition = Camera.main.ScreenToWorldPoint(_screenPosition);

        if (_isDragActive) {
            Drag();
        } else {
            if (isPressed) 
            {
                int layerMask = 0;

                // TODO: change to detect game asset list later

                layerMask = 1 << INVENTORYITEM;
                layerMask |= (1 << PLACEDITEM);

                RaycastHit2D ray = Physics2D.Raycast(_worldPosition, Vector2.zero, Mathf.Infinity, layerMask);
                if (ray.collider != null)
                {
                    Draggable draggable = ray.collider.gameObject.GetComponent<Draggable>();
                    if (draggable != null && draggable.enabled) pressDuration += Time.deltaTime;
                }

                if (pressDuration >= requiredPressDuration) 
                {
                    RaycastHit2D[] hits;

                    // prevent raycast to characters if inventory is on
                    if (inventory.inventoryOnDisplay)
                    {
                        int layers = 1 << PLACEDITEM;
                        hits = Physics2D.RaycastAll(_worldPosition, Vector2.zero, Mathf.Infinity, ~layers);
                    } 
                    else 
                    {
                        hits = Physics2D.RaycastAll(_worldPosition, Vector2.zero);
                    }

                    foreach (RaycastHit2D hit in hits)
                    {
                        GameObject hittedObject = hit.collider.gameObject;
                        if (hittedObject.layer == INVENTORYITEM) 
                        {
                            unsettledUIDrag = hittedObject;

                            hittedObject = player.TakeOutFromInventory(hittedObject.GetComponent<GameAsset>(), _worldPosition, "PlacedItems");

                            unsettledUIDrag.SetActive(false);

                            inventory.ToggleInvetoryOnDisplay();
                            inventory.gameObject.SetActive(false);
                        }
                        if (hittedObject.layer == PLACEDITEM || hittedObject.layer == INVENTORYITEM) 
                        {
                            Draggable draggable = hittedObject.GetComponent<Draggable>();
                            if (draggable != null) {
                                _lastDragged = draggable;

                                //TODO: ending ongoing interaction when this happens

                                InitDrag();
                                hittedObject.transform.localScale += scaleChange;
                            }
                            break;
                        }
                    }       
                }
            }   
        }
    }

    void InitDrag()
    {
        _lastDragged.lastPosition = _lastDragged.transform.position;
        UpdateDragStatus(true);
    }

    void Drag()
    {
        _lastDragged.transform.position = new Vector2(_worldPosition.x, _worldPosition.y);
    }

    void Drop()
    {
        if (!inventory.inventoryOnDisplay && unsettledUIDrag != null) {
            inventory.ToggleInvetoryOnDisplay();
            inventory.gameObject.SetActive(true);
        }
        RaycastHit2D[] hits = Physics2D.RaycastAll(_lastDragged.transform.position, Vector2.zero);
        foreach (RaycastHit2D hit in hits) 
        {
            GameObject obj = hit.collider.gameObject;

            if (obj.CompareTag("DragSpot") && !obj.GetComponent<DragSpot>().isOccupied) 
            {
                MoveSpots(_lastDragged, obj);
                _lastDragged.transform.position = obj.transform.position;

                // TODO: adjust pivot of the object relative to selection spot in the future
                
                UpdateDragStatus(false);
                Destroy(unsettledUIDrag);
                unsettledUIDrag = null;
                return;
            }

            if (obj.CompareTag("Bin"))
            {
                UpdateDragStatus(false);
                LeaveSpot(_lastDragged);
                player.PutToInventory(_lastDragged.gameObject.GetComponent<GameAsset>(), false);
                return;
            }
        }

        // previously taken from inventory
        if (unsettledUIDrag != null)
        {
            Destroy(_lastDragged.gameObject);
            unsettledUIDrag.SetActive(true);
            unsettledUIDrag = null;
            UpdateDragStatus(false);
            return;
        }

        // TODO: consider what if it was previously involved in interactions
        _lastDragged._movementDestination = _lastDragged.lastPosition;

        UpdateDragStatus(false);
    }

    void UpdateDragStatus(bool isDragging)
    {
        bin.gameObject.SetActive(isDragging && _lastDragged.gameObject.GetComponent<Draggable>()._dragSpot != null);
        _isDragActive = _lastDragged.isDragging = isDragging;
        _lastDragged.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = isDragging ? "Dragging" : "PlacedItem";
        _lastDragged.gameObject.GetComponent<SpriteRenderer>().sortingOrder = isDragging ? 1 : 0;
    }

    public static void MoveSpots(Draggable _lastDragged, GameObject newSpot)
    {
        GameObject oldSpot = _lastDragged.gameObject.GetComponent<Draggable>()._dragSpot;
        if (oldSpot != null && oldSpot.GetComponent<DragSpot>().draggedObject != null && oldSpot.GetComponent<DragSpot>().draggedObject.gameObject == _lastDragged.gameObject)
        {
            oldSpot.GetComponent<DragSpot>().clearObj();
        }
        newSpot.GetComponent<DragSpot>().takenBy(_lastDragged.gameObject);
    }

    public static void LeaveSpot(Draggable _lastDragged)
    {
        GameObject oldSpot = _lastDragged.gameObject.GetComponent<Draggable>()._dragSpot;
        if (oldSpot != null && oldSpot.GetComponent<DragSpot>().draggedObject != null)
        {
            oldSpot.GetComponent<DragSpot>().clearObj();
        }
    }
}
