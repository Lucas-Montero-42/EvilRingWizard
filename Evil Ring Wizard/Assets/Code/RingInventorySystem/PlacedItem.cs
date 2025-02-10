using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RingItem;

public class PlacedItem : MonoBehaviour
{
    public static PlacedItem Create(Vector3 worldPosition,Vector2Int origin, RingItem.Dir dir, RingItem ringItem)
    {
        RectTransform itemTransform = Instantiate(
                         ringItem.itemPrefab,
                         worldPosition,
                         Quaternion.Euler(0, 0, ringItem.GetRotationAngle(dir)),
                         GameObject.Find("Canvas").transform
                         );
        PlacedItem item = itemTransform.GetComponent<PlacedItem>();
        item.ringItem = ringItem;
        item.origin = origin;
        item.dir = dir;

        return item;
    }

    [SerializeField]private RingItem ringItem;
    private Vector2Int origin;
    private RingItem.Dir dir;

    public List<Vector2Int> GetGridPositionList()
    {
        return ringItem.GetGridPositionList(origin, dir);
    }
    public RingItem GetRingItem()
    {
        return ringItem;
    }
    internal Dir GetDir()
    {
        return dir;
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
