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
    private ItemSlot previousItemSlot;

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

        previousItemSlot = eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot();
        eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().ClearItem();

        if (eventData.pointerDrag.GetComponent<Item>().GetPreviousParentItemSlot().itemsOnSlotCount > 0)
        {
            eventData.pointerDrag.GetComponent<Item>().GetPreviousParentItemSlot().itemsOnSlotCount--;
        }
        if (eventData.pointerDrag.GetComponent<Item>().GetPreviousParentItemSlot().itemsOnSlotCount <= 0)
        {
            eventData.pointerDrag.GetComponent<Item>().GetPreviousParentItemSlot().transform.GetChild(0).gameObject.SetActive(false);
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
            if (GetParentItemSlot().itemsOnSlotCount < GetPickUpSO().maxStack)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetParentItemSlot().GetItemSlotRectTransform().anchoredPosition;
                eventData.pointerDrag.GetComponent<Item>().SetParentItemSlot(GetParentItemSlot());

                eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().itemsOnSlotCount++;
                eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().transform.GetChild(0).gameObject.SetActive(true);
            }else
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().GetItemSlotRectTransform().anchoredPosition;
                eventData.pointerDrag.GetComponent<Item>().GetPreviousParentItemSlot().SetItem(eventData.pointerDrag.GetComponent<Item>());

                eventData.pointerDrag.GetComponent<Item>().GetPreviousParentItemSlot().transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        else
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().GetItemSlotRectTransform().anchoredPosition;
            eventData.pointerDrag.GetComponent<Item>().GetPreviousParentItemSlot().SetItem(eventData.pointerDrag.GetComponent<Item>());
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
    public ItemSlot GetPreviousParentItemSlot()
    {
        return previousItemSlot;
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
