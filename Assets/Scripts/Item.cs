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
        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<Item>().GetParentItemSlot().GetItemSlotRectTransform().anchoredPosition;
        eventData.pointerDrag.GetComponent<Item>().GetPreviousParentItemSlot().SetItem(eventData.pointerDrag.GetComponent<Item>());
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
}
