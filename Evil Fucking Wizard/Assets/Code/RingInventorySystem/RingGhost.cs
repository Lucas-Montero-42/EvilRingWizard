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
    public InventoryGridSystem hand;
    private Grid<Item> grid;

    void Start()
    {
        //Crea el objeto para que siga al cursor
        visual = Instantiate(defaultItem.itemVisual, new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("Canvas").transform);
        //Asigna al hijo que es el que tiene la información
        visualChild = GetChild(visual);
    }

    // Update is called once per frame
    void Update()
    {
        if (hand.GetCurrentItem())
        {
            //Actualiza el current item
            currentRingItem = hand.GetCurrentItem();
            //Actualiza la posición del item en con el ratón
            visual.position = hand.GetMousePosition() - new Vector2(50,50);
            //Asigna la imagen, el tamaño y el offset del anillo en cuestión
            visual.GetComponentInChildren<Image>().sprite = currentRingItem.itemVisual.GetComponentInChildren<Image>().sprite;
            visualChild.sizeDelta = GetChild(currentRingItem.itemVisual).sizeDelta;
            visualChild.localPosition = GetChild(currentRingItem.itemVisual).localPosition;
            //DISPLAYEAR SI ESTÁN ROTADOS------------------------------------------------------------------------------------------
            if (hand.dir == RingItem.Dir.Up)
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

    }
    private RectTransform GetChild(RectTransform parent)
    {
        List<RectTransform> childList = new List<RectTransform>();
        childList.AddRange(parent.GetComponentsInChildren<RectTransform>());
        childList.Remove(parent.GetComponent<RectTransform>());
        return childList[0];
    }
    private RectTransform ElCircoDeUnity(RectTransform parent)
    {
        List<RectTransform> childList = new List<RectTransform>();
        childList.AddRange(parent.GetComponentsInChildren<RectTransform>());
        childList.Remove(parent.GetComponent<RectTransform>());
        return childList[0];
    }
}
