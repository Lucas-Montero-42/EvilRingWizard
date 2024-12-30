using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingGhost : MonoBehaviour
{
    private RectTransform visual;
    private RectTransform visualChild;
 
    public RingItem defaultItem;
    private RingItem currentRingItem;
    public InventoryGridSystem hands;
    public InventoryGridSystem chest;
    private Grid<Item> grid;

    void Start()
    {
        //Crea el objeto para que siga al cursor
        visual = Instantiate(defaultItem.itemVisual, new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("Canvas").transform);
        //Asigna al hijo que es el que tiene la información
        visualChild = GetChild(visual);
        hands.pickUpItem += ChestItemPickup;
        chest.pickUpItem += HandsItemPickup;
        hands.dropItem += ChestItemDrop;
        chest.dropItem += HandsItemDrop;
    }

    // Update is called once per frame
    void Update()
    {
        ItemHandler(hands);
        ItemHandler(chest);
        /*
        if (hands.GetCurrentItem())
        {
            //Actualiza el current item
            currentRingItem = hands.GetCurrentItem();
            //Actualiza la posición del item en con el ratón
            visual.position = hands.GetMousePosition() - new Vector2(50,50);
            //Asigna la imagen, el tamaño y el offset del anillo en cuestión
            visual.GetComponentInChildren<Image>().sprite = currentRingItem.itemVisual.GetComponentInChildren<Image>().sprite;
            visualChild.sizeDelta = GetChild(currentRingItem.itemVisual).sizeDelta;
            visualChild.localPosition = GetChild(currentRingItem.itemVisual).localPosition;
            //DISPLAYEAR SI ESTÁN ROTADOS------------------------------------------------------------------------------------------
            if (hands.dir == RingItem.Dir.Up)
            {
                visualChild.rotation = Quaternion.Euler(0,0,180);
            }
            else
            {
                visualChild.rotation = Quaternion.identity;
            }
        }
        else
        {
            visualChild.GetComponent<Image>().sprite = GetChild(defaultItem.itemVisual).GetComponent<Image>().sprite;
        }
         */

    }
    private void ChestItemPickup() 
    { 
        chest.setRingItem(hands.GetRingItem());
    }
    private void ChestItemDrop()
    {
        chest.setRingItem(null);
    }
    private void HandsItemPickup()
    {
        hands.setRingItem(chest.GetRingItem());
    }
    private void HandsItemDrop()
    {
        hands.setRingItem(null);
    }
    private void ItemHandler(InventoryGridSystem grid)
    {
        if (grid.GetCurrentItem())
        {
            //Actualiza el current item
            currentRingItem = grid.GetCurrentItem();
            //Actualiza la posición del item en con el ratón
            visual.position = grid.GetMousePosition() - new Vector2(50, 50);
            //Asigna la imagen, el tamaño y el offset del anillo en cuestión
            visual.GetComponentInChildren<Image>().sprite = currentRingItem.itemVisual.GetComponentInChildren<Image>().sprite;
            visualChild.sizeDelta = GetChild(currentRingItem.itemVisual).sizeDelta;
            visualChild.localPosition = GetChild(currentRingItem.itemVisual).localPosition;
            //DISPLAYEAR SI ESTÁN ROTADOS------------------------------------------------------------------------------------------
            if (grid.dir == RingItem.Dir.Up)
            {
                visualChild.rotation = Quaternion.Euler(0, 0, 180);
            }
            else
            {
                visualChild.rotation = Quaternion.identity;
            }
        }
        else
        {
            visualChild.GetComponent<Image>().sprite = GetChild(defaultItem.itemVisual).GetComponent<Image>().sprite;
        }
    }
    private RectTransform GetChild(RectTransform parent)// Musica de circo por como unity funciona con su "expected behavior"
    {
        List<RectTransform> childList = new List<RectTransform>();
        childList.AddRange(parent.GetComponentsInChildren<RectTransform>());
        childList.Remove(parent.GetComponent<RectTransform>());
        return childList[0];
    }
}
