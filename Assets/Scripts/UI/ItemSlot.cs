using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    private List<Item> itemList;
    private int itemsOnSlotCount = 0;
    private void Awake()
    {
        itemList = new List<Item>();

        gameObject.SetActive(true);
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Item item = eventData.pointerDrag.GetComponent<Item>();
            if ((item.GetPickUpSO().isStackable && itemsOnSlotCount < item.GetPickUpSO().maxStack && HasItem() && GetItem().GetPickUpSO().isStackable) || (!HasItem()))
            {
                item.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

                SetItem(item);
                item.SetItemSlot(this);
            }else
            {
                item.GetComponent<RectTransform>().anchoredPosition = item.GetItemSlot().GetComponent<RectTransform>().anchoredPosition;

                item.GetItemSlot().SetItem(item);
            }
        }
    }
    public void SetItem(Item item)
    {
        itemList.Add(item);
        itemsOnSlotCount++;
    }
    public Item GetItem()
    {
        return itemList[0];
    }
    public bool HasItem()
    {
        return itemList.Count > 0;
    }
    public void ClearItem()
    {
        itemList.RemoveAt(itemList.Count - 1);
        itemsOnSlotCount--;
    }
    public void IncreaseItemsOnSlotCount()
    {
        itemsOnSlotCount++;
    }
    public int GetItemsOnSlotCount()
    {
        return itemsOnSlotCount;
    }
    
}
