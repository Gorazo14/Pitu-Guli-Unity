using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Item : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IInitializePotentialDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform parentItemSlot;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        parentItemSlot = null;

        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;

        eventData.pointerDrag.GetComponent<Item>().ClearParentItemSlot();
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

    public void SetParentItemSlot (RectTransform itemSlot)
    {
        parentItemSlot = itemSlot;

        rectTransform.anchoredPosition = itemSlot.anchoredPosition;
    }
    public RectTransform GetParentItemSlot()
    {
        return parentItemSlot;
    }
    public void ClearParentItemSlot()
    {
        parentItemSlot = null;
    }
    public bool HasParentItemSlot ()
    {
        return parentItemSlot != null;
    }
}
