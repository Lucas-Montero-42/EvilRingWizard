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
