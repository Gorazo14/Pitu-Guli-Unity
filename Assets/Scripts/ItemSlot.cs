using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    private RectTransform item;
    private RectTransform itemSlotRectTransform;

    private void Start()
    {
        itemSlotRectTransform = GetComponent<RectTransform>(); 
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            SetItem(eventData.pointerDrag.GetComponent<RectTransform>());
            eventData.pointerDrag.GetComponent<Item>().SetParentItemSlot(itemSlotRectTransform);
        }
    }
    public void SetItem (RectTransform rectTransform)
    {
        item = rectTransform;
    }
    public RectTransform GetItem ()
    {
        return item;
    }
    public void ClearItem ()
    {
        item = null;
    }
    public bool HasItem ()
    {
        return item != null;
    }
}
