using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private RectTransform itemSlotRectTransform;

    private Item item;
    public int itemsOnSlotCount;

    private void Awake()
    {
        gameObject.SetActive(true);
    }
    private void Update()
    {
        if (itemsOnSlotCount > 0)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (!HasItem())
            {
                // The item slot is empty

                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.GetComponent<Item>().SetParentItemSlot(this);

                SetItem(eventData.pointerDrag.GetComponent<Item>());
                if (eventData.pointerDrag.GetComponent<Item>().GetStackability())
                {
                    itemsOnSlotCount++;
                }
            }
            else
            {
                // There is an item on the item slot
                if (eventData.pointerDrag.GetComponent<Item>().GetStackability() && GetItem().GetStackability())
                {
                    // And both items are stackable
                    if (itemsOnSlotCount < eventData.pointerDrag.GetComponent<Item>().GetPickUpSO().maxStack)
                    {
                        // And there is space on the item slot
                        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetItemSlotRectTransform().anchoredPosition;
                        eventData.pointerDrag.GetComponent<Item>().SetParentItemSlot(this);

                        itemsOnSlotCount++;
                    }else
                    {
                        // There is no space on the item slot
                        eventData.pointerDrag.GetComponent<Item>().SetParentItemSlot(eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot());
                        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().GetItemSlotRectTransform().anchoredPosition;
                        eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().SetItem(eventData.pointerDrag.GetComponent<Item>());

                        eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().itemsOnSlotCount++;
                    }
                }
                else
                {
                    // At least one of the items is NOT stackable
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().GetItemSlotRectTransform().anchoredPosition;
                    eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().SetItem(eventData.pointerDrag.GetComponent<Item>());
                }
            } 
        }
    }
    public RectTransform GetItemSlotRectTransform()
    {
        return itemSlotRectTransform;
    }

    public void SetItem(Item item)
    {
        this.item = item;
    }
    public Item GetItem()
    {
        return item;
    }
    public bool HasItem()
    {
        return item != null;
    }
    public void ClearItem()
    {
        item = null;
    }
}
