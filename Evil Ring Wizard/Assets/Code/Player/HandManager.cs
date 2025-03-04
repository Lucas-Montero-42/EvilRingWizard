using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public InventoryGridSystem handsInventory;
    public Transform[] HANDS;
    public bool[,] occupiedSpaces = new bool[14,2];
    private void Awake()
    {
        handsInventory.dropItem += AddRings;
        handsInventory.pickUpItem += RemoveRings;
        for (int x = 0; x < 14; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                occupiedSpaces[x,y] = false;
            }
        }
    }
    private void Start()
    {
        //handsInventory = GameManager.instance.HandsMenuScreen.GetComponentInChildren<InventoryGridSystem>();
        //handsInventory.dropItem += AddRings;
        //handsInventory.pickUpItem += RemoveRings;
    }

    private void AddRings()
    {
        Grid<Item> newHand = handsInventory.GetHand();

        for (int x = 0; x < handsInventory.width; x++)
        {
            for (int y = 0; y < handsInventory.height; y++)
            {
                if (newHand.GetGridObject(x, y).GetPlacedItem() != null && !occupiedSpaces[x,y])
                {
                    GameObject newRingPrefab = Instantiate(handsInventory.GetHand().GetGridObject(x, y).GetPlacedItem().GetRingItem().ringPrefab3D, HANDS[x + (y * 14)].transform);

                    int w = handsInventory.GetHand().GetGridObject(x, y).GetPlacedItem().GetRingItem().width;
                    int h = handsInventory.GetHand().GetGridObject(x, y).GetPlacedItem().GetRingItem().height;

                    Ocupation(true, x, y, w, h);

                }
            }
        }
    }
    private void RemoveRings()
    {
        StartCoroutine(RemoveTimer());
    }
    IEnumerator RemoveTimer()
    {
        yield return new WaitForEndOfFrame();

        for (int x = 0; x < handsInventory.width; x++)
        {
            for (int y = 0; y < handsInventory.height; y++)
            {
                // Si estaba ocupado antes pero ahora está vacío, eliminamos el anillo
                if (occupiedSpaces[x, y] && handsInventory.GetHand().GetGridObject(x, y).GetPlacedItem() == null)
                {
                    Transform ringTransform = HANDS[x + (y * 14)].transform;

                    // Eliminar el anillo si existe en el punto clave del dedo
                    if (ringTransform.childCount > 0)
                    {
                        foreach (Transform child in ringTransform)
                        {
                            Destroy(child.gameObject);
                        }
                    }

                    // Marcar la posición como libre
                    occupiedSpaces[x, y] = false;
                }
            }
        }
    }
    /*
    private void RemoveRings()
    {
        Grid<Item> newHand = handsInventory.GetHand();

        for (int x = 0; x < handsInventory.width; x++)
        {
            for (int y = 0; y < handsInventory.height; y++)
            {
                if (newHand.GetGridObject(x, y).GetPlacedItem() != null && !occupiedSpaces[x, y])
                {
                    GameObject newRingPrefab = Instantiate(handsInventory.GetHand().GetGridObject(x, y).GetPlacedItem().GetRingItem().ringPrefab3D, HANDS[x + (y * 14)].transform);

                    int w = handsInventory.GetHand().GetGridObject(x, y).GetPlacedItem().GetRingItem().width;
                    int h = handsInventory.GetHand().GetGridObject(x, y).GetPlacedItem().GetRingItem().height;

                    Ocupation(false,x,y,w,h);

                }
            }
        }
    }

     */
    private void Ocupation(bool occupied, int x, int y, int w, int h)
    {
        occupiedSpaces[x, y] = occupied;
        for (int i = 0; i < w * h; i++)
        {
            if (w == 1 || h == 1)
            {
                if (w > h)
                {
                    occupiedSpaces[x + (w - 1), y] = occupied;
                }
                else
                {
                    occupiedSpaces[x, y + (h - 1)] = occupied;
                }
            }
            else
            {
                for (int z = 0; z < w; z++)
                {
                    for (int a = 0; a < h; a++)
                    {
                        occupiedSpaces[x + z, y + a] = occupied;
                    }
                }
            }
        }
    }
}
