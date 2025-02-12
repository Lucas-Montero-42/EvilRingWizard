using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public InventoryGridSystem handsInventory;
    public Transform[] HANDS;
    private void Awake()
    {
        handsInventory.dropItem += ShowRings;
        handsInventory.pickUpItem += ShowRings;
    }

    private void ShowRings()
    {
        Grid<Item> newHand = handsInventory.GetHand();

        for (int x = 0; x < handsInventory.width; x++)
        {
            for (int y = 0; y < handsInventory.height; y++)
            {
                if (newHand.GetGridObject(x, y).GetPlacedItem() != null)
                {
                    
                    Debug.Log(x + (y * 14));
                    
                    GameObject newRingPrefab = Instantiate(handsInventory.GetHand().GetGridObject(x, y).GetPlacedItem().GetRingItem().ringPrefab3D, HANDS[x + (y * 14)].transform);
                }
            }
        }
    }
}
