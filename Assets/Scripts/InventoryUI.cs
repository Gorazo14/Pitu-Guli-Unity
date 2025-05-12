using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private Transform inventoryTransform;
    [SerializeField] private Transform itemSpawnPoint;

    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private Gun gun;

    [SerializeField] private RectTransform itemPrefab;
    [SerializeField] private ItemSlot[] slots;

    private int i = 0;
    private void Start()
    {
        inventoryTransform.gameObject.SetActive(false);
        Player.Instance.enabled = true;
        GameInput.Instance.OnInventoryOpenClose += GameInput_OnInventoryOpenClose;
        Player.Instance.OnItemPickedUp += Player_OnItemPickedUp;
    }

    private void Player_OnItemPickedUp(object sender, Player.OnItemPickedUpEventArgs e)
    {
        for (int i=0; i < slots.Length; i++)
        {
            if (!slots[i].HasItem() || (slots[i].HasItem() && slots[i].GetItem().GetPickUpSO().isStackable && slots[i].GetItemsOnSlotCount() < e.pickUp.GetPickUpSO().maxStack))
            {
                this.i = i;
                break;
            }
        }

        RectTransform itemTransform = Instantiate(itemPrefab, inventoryTransform);
        Item item = itemTransform.GetComponent<Item>();

        item.SetPickUpSO(e.pickUp.GetPickUpSO());

        itemTransform.anchoredPosition = slots[i].GetComponent<RectTransform>().anchoredPosition;
        itemTransform.GetComponent<Image>().sprite = item.GetPickUpSO().pickUpSprite;
        itemTransform.GetComponent<Image>().color = Color.red;
        itemTransform.gameObject.SetActive(true);
        item.SetItemSlot(slots[i]);
        slots[i].SetItem(item);
    }

    private void GameInput_OnInventoryOpenClose(object sender, System.EventArgs e)
    {
        if (inventoryTransform.gameObject.activeSelf)
        {
            inventoryTransform.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            mouseLook.enabled = true;
            gun.enabled = true;
        }else
        {
            inventoryTransform.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            mouseLook.enabled = false;
            gun.enabled = false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Instantiate(eventData.pointerDrag.GetComponent<Item>().GetPickUpSO().pickUpPrefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
        Destroy(eventData.pointerDrag.gameObject);
    }
}
