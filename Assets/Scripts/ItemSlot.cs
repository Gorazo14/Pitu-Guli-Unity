using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private RectTransform itemSlotRectTransform;

    private Item item;

    private void Start()
    {
        gameObject.SetActive(true);
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (!HasItem())
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.GetComponent<Item>().SetParentItemSlot(this);

                SetItem(eventData.pointerDrag.GetComponent<Item>());
            }else
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().GetItemSlotRectTransform().anchoredPosition;
                eventData.pointerDrag.GetComponent<Item>().GetPreviousParentItemSlot().SetItem(eventData.pointerDrag.GetComponent<Item>());
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
