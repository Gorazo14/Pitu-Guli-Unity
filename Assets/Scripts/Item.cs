using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Item : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IInitializePotentialDragHandler, IDropHandler
{
    [SerializeField] private Canvas canvas;

    [SerializeField] private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private ItemSlot itemSlot;

    private bool isStackable;
    private PickUpSO pickUpSO;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;

        eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().ClearItem();

        if (eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().itemsOnSlotCount > 0)
        {
            eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().itemsOnSlotCount--;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }
    public CanvasGroup GetCanvasGroup()
    {
        return canvasGroup;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<Item>().GetStackability() && GetStackability())
        {
            // Both items are stackable
            if (GetParentItemSlot().itemsOnSlotCount < GetPickUpSO().maxStack)
            {
                // There is space on the item slot
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetParentItemSlot().GetItemSlotRectTransform().anchoredPosition;
                eventData.pointerDrag.GetComponent<Item>().SetParentItemSlot(GetParentItemSlot());

                eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().itemsOnSlotCount++;
            }else
            {
                // There is not space on the item slot
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().GetItemSlotRectTransform().anchoredPosition;
                eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().SetItem(eventData.pointerDrag.GetComponent<Item>());

                eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().itemsOnSlotCount++;
            }
        }
        else
        {
            // At least one of the items is not stackable
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().GetItemSlotRectTransform().anchoredPosition;
            eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().SetItem(eventData.pointerDrag.GetComponent<Item>());
        }
    }

    public void SetParentItemSlot(ItemSlot itemSlot)
    {
        this.itemSlot = itemSlot;
    }
    public ItemSlot GetParentItemSlot()
    {
        return itemSlot;
    }
    public void ClearParentItemSlot()
    {
        this.itemSlot = null;
    }
    public RectTransform GetItemRectTransform()
    {
        return rectTransform;
    }
    public bool GetStackability()
    {
        return isStackable;
    }
    public void SetStackability(bool isStackable)
    {
        this.isStackable = isStackable;
    }
    public void SetPickUpSO(PickUpSO pickUpSO)
    {
        this.pickUpSO = pickUpSO;
    }
    public PickUpSO GetPickUpSO()
    {
        return pickUpSO;
    }
}
