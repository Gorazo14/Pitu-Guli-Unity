using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Item : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IInitializePotentialDragHandler, IDropHandler
{
    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private ItemSlot itemSlot;
    private PickUpSO pickUpSO;
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();  
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;

        eventData.pointerDrag.GetComponent<Item>().GetItemSlot().ClearItem();
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

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Item item = eventData.pointerDrag.GetComponent<Item>();
            if ((item.GetPickUpSO().isStackable && GetItemSlot().GetItemsOnSlotCount() < item.GetPickUpSO().maxStack && GetItemSlot().HasItem() && GetItemSlot().GetItem().GetPickUpSO().isStackable) || (!GetItemSlot().HasItem()))
            {
                item.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

                GetItemSlot().SetItem(item);
                item.SetItemSlot(GetItemSlot());
            }
            else
            {
                item.GetComponent<RectTransform>().anchoredPosition = item.GetItemSlot().GetComponent<RectTransform>().anchoredPosition;

                item.GetItemSlot().SetItem(item);
            }
        }
    }
    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }
    public CanvasGroup GetCanvasGroup()
    {
        return canvasGroup;
    }

    public void SetItemSlot(ItemSlot itemSlot)
    {
        this.itemSlot = itemSlot;
    }
    public ItemSlot GetItemSlot()
    {
        return itemSlot;
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
