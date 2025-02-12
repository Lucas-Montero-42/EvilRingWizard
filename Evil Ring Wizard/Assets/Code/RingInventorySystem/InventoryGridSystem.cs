using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryGridSystem : MonoBehaviour
{
    public Action pickUpItem = delegate { };
    public Action dropItem = delegate { };
    public Action rotateItem = delegate { };

    [Header("Rings")]
    [SerializeField] private RingItem ringItem;
    public RingItem innateRing;
    private Grid<Item> hand;
    public RingItem.Dir dir = RingItem.Dir.Down;
    [SerializeField] private List<RingItem> debugRingItemList;

    [Header("Grid Size")]
    public int width = 5;
    public int height = 2;
    public float cellSize = 100f;
    private Vector3 gridPosition;
    public float yPosition;
    public Vector2Int[] disabledPositions;

    [Header("Visual Elements")]
    public Sprite backImage;
    public Color backColor;
    public Sprite cellImage;
    public Color cellColor;
    public Vector2 margin;
    public float gridOffset;

    // Start is called before the first frame update
    void Awake()
    {
        int gridWidth = width;
        int gridHeight = height;
        float gridCellSize = cellSize;

        float X = GetComponentInParent<RectTransform>().position.x - (width * gridCellSize / 2);
        float Y = GetComponentInParent<RectTransform>().position.y - (height * gridCellSize / 2);


        gridPosition = new Vector3(X, Y - 150 + yPosition, 0);

        

        hand = new Grid<Item>(gridWidth, gridHeight, gridCellSize, gridPosition, (Grid<Item> i, int x, int y) => new Item(0, i, x, y));

        foreach (Vector2Int dPos in disabledPositions)
        {
            hand.GetGridObject(dPos.x, dPos.y).enabled = false; 
        }
        VisualizeGrid();

        DebugSpawnRing();


    }
    private void DebugSpawnRing()
    {
        if (innateRing)
        {
            int x = 1;
            int y = 0;
            Item item = hand.GetGridObject(x, y);

            List<Vector2Int> gridPositionList = innateRing.GetGridPositionList(new Vector2Int(x, y), dir);

            Vector2Int rotationOffset = innateRing.GetRotationOffset(RingItem.Dir.Down);
            Vector3 ringItemWorldPosition = hand.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y, 0) * hand.GetCellSize();

            PlacedItem placedItem = PlacedItem.Create(ringItemWorldPosition, new Vector2Int(x, y), RingItem.Dir.Down, innateRing);
            placedItem.transform.SetParent(transform);

            foreach (Vector2Int gridPosition in gridPositionList)
            {
                hand.GetGridObject(gridPosition.x, gridPosition.y).SetTransform(placedItem);
            }
            item.SetTransform(placedItem);
            ringItem = null;
        }
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && ringItem)
        {
            Item item = hand.GetGridObject(GetMousePosition());
            if (item != null)
            {
                hand.GetXY(GetMousePosition(), out int x, out int y);

                List<Vector2Int> gridPositionList = ringItem.GetGridPositionList(new Vector2Int(x, y), dir);

                bool canBuild = true;
                foreach(Vector2Int gridPosition in gridPositionList)
                {
                    if (hand.GetGridObject(gridPosition.x, gridPosition.y) == null || !hand.GetGridObject(gridPosition.x, gridPosition.y).CanBuild() )
                    {
                        canBuild = false;
                        break;
                    }
                }

                if (canBuild)
                {
                    Vector2Int rotationOffset = ringItem.GetRotationOffset(dir);
                    Vector3 ringItemWorldPosition = hand.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y, 0) * hand.GetCellSize();

                    PlacedItem placedItem = PlacedItem.Create(ringItemWorldPosition, new Vector2Int(x,y), dir, ringItem);
                    placedItem.transform.SetParent(transform);
                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        hand.GetGridObject(gridPosition.x, gridPosition.y).SetTransform(placedItem);
                    }
                    item.SetTransform(placedItem);
                    //Remove Current Item
                    ringItem = null;
                    dropItem?.Invoke();
                    GameManager.instance.holdingObject = false;
                }
                else
                {
                    Debug.Log("Doesen't fit");
                }
            }
            
        }
        if (Input.GetMouseButtonDown(1) && !ringItem)
        {
            Item item = hand.GetGridObject(GetMousePosition());

            if (item != null)
            {
                PlacedItem placedItem = item.GetPlacedItem();
                if (placedItem != null)
                {
                    //"Pick up" item
                    ringItem = placedItem.GetRingItem();
                    pickUpItem?.Invoke();
                    GameManager.instance.holdingObject = true;

                    // Si es un anillo, guardarlo en GameManager para efectos persistentes
                    if (ringItem != null)
                    {
                        GameManager.instance.lastRingItem = ringItem;
                    }

                    dir = placedItem.GetDir();
                    placedItem.DestroySelf();

                    List<Vector2Int> gridPositionList = placedItem.GetGridPositionList();
                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        hand.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacefObject();
                    }
                }
            }
            
        }
        if (Input.GetKeyDown(KeyCode.R) && ringItem)
        {
            //Solo para los valientes que quieran debugar el sistema de rotación. Ultima revisión 15/1/2025
            //rotateItem?.Invoke();
            //Debug.Log("ItemRotated on: "+name);
            
        }
        //Eliminar, solo para debug
        /*
         */
        if (innateRing)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) 
            { 
                innateRing = debugRingItemList[0];
                DebugSpawnRing();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) 
            {
                innateRing = debugRingItemList[1];
                DebugSpawnRing();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) 
            {
                innateRing = debugRingItemList[2];
                DebugSpawnRing();
            }
        }
        
    }
    public Vector2 GetMousePosition()
    {
        return Input.mousePosition;
    }

    public Grid<Item> GetHand()
    {
        return hand;
    }
    public RingItem GetCurrentItem()
    {
        return ringItem;
    }
    private void VisualizeGrid()
    {
        //Crear la parte trasera
        CreateUIImage(this.transform, backImage, new Vector3(gridPosition.x + (width*cellSize/2), gridPosition.y + (height * cellSize / 2), 0), new Vector2(width * cellSize + margin.x, height * cellSize + margin.y), backColor);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //crear cada casilla
                if (!hand.GetGridObject(x,y).enabled) continue;
                CreateUIImage(this.transform, cellImage, new Vector3(gridPosition.x+ gridOffset + (cellSize*x), gridPosition.y+ gridOffset + (cellSize * y), 0), new Vector2(cellSize, cellSize), cellColor);
            }
        }
    }
    private Image CreateUIImage(Transform parent, Sprite sprite, Vector3 localPos, Vector2 size, Color color)
    {
        //GameObject canvas = GameObject.Find("Canvas");
        GameObject gameObject = new GameObject("Canvas_Image", typeof(Image));
        RectTransform transform = gameObject.GetComponent<RectTransform>();
        transform.SetParent(parent.transform, true);
        transform.position = localPos;
        Image image = gameObject.GetComponent<Image>();
        transform.sizeDelta = size;
        image.sprite = sprite;
        image.color = color;
        return image;

    }

    internal void setRingItem(RingItem rItem)
    {
        ringItem = rItem;
    }

    internal RingItem GetRingItem()
    {
        return ringItem;
    }
}
public class Item
{
    public bool enabled = true;
    public Grid<Item> grid;
    private int x, y;
    private PlacedItem pItem;
    public Item(int s, Grid<Item> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }
    public void SetTransform(PlacedItem pItem)
    {
        this.pItem = pItem;
        grid.TriggerGirdObjectChanged(x, y);
    }
    public RectTransform GetTransform()
    {
        return pItem.GetComponent<RectTransform>();
    }
    public bool CanBuild()
    {
        if (enabled)
        {
            return pItem == null;
        }
        else
        {
            return false;
        }
    }
    public void ClearPlacefObject()
    {
        pItem = null;
        grid.TriggerGirdObjectChanged(x, y);
    }

    public PlacedItem GetPlacedItem()
    {
        return pItem;
    }

    public override string ToString()
    {
        if(enabled)
            return x + "," + y;
        else 
            return string.Empty;
    }
    
}