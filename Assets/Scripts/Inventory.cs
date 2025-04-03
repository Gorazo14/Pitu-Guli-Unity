using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Inventory : MonoBehaviour, IDropHandler
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private GameObject inventoryBackground;
    [SerializeField] private Player player;
    [SerializeField] private GameObject medkitPrefab;
    [SerializeField] private Transform itemSpawnPoint;

    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private Gun gun;

    [SerializeField] private ItemSlot[] slots;
    [SerializeField] private Item[] items;
 
    private void Start()
    {
        inventoryBackground.SetActive(false);
        player.enabled = true;
        gameInput.OnInventoryOpenClose += GameInput_OnInventoryOpenClose;
        player.OnItemPickedUp += Player_OnItemPickedUp;
    }
    
    private void Player_OnItemPickedUp(object sender, Player.OnItemPickedUpEventArgs e)
    {
        if (!e.medkit.GetPickUpSO().isStackable)
        {
            foreach (ItemSlot slot in slots)
            {
                if (!slot.HasItem())
                {
                    foreach (Item item in items)
                    {
                        if (!item.gameObject.activeSelf)
                        {
                            item.SetParentItemSlot(slot);
                            slot.SetItem(item);
                            item.GetItemRectTransform().anchoredPosition = slot.GetItemSlotRectTransform().anchoredPosition;

                            item.gameObject.SetActive(true);

                            break;
                        }
                    }

                    break;
                }
            }
        }else
        {
            foreach (ItemSlot slot in slots)
            {
                int itemsOnSlot = 0;
                if (itemsOnSlot < e.medkit.GetPickUpSO().maxStack)
                {
                    foreach (Item item in items)
                    {
                        if (!item.gameObject.activeSelf)
                        {
                            item.SetParentItemSlot(slot);
                            slot.SetItem(item);
                            item.GetItemRectTransform().anchoredPosition = slot.GetItemSlotRectTransform().anchoredPosition;

                            item.gameObject.SetActive(true);

                            itemsOnSlot++;
                            break;
                        }
                    }
                    break;
                }
            }
        }
        
    }
    
    private void GameInput_OnInventoryOpenClose(object sender, System.EventArgs e)
    {
        if (inventoryBackground.activeSelf)
        {
            inventoryBackground.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            mouseLook.enabled = true;
            gun.enabled = true;
        }else
        {
            inventoryBackground.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            mouseLook.enabled = false;
            gun.enabled = false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Instantiate(medkitPrefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
        eventData.pointerDrag.gameObject.SetActive(false);

        eventData.pointerDrag.gameObject.GetComponent<Item>().GetCanvasGroup().alpha = 1f;
        eventData.pointerDrag.gameObject.GetComponent<Item>().GetCanvasGroup().blocksRaycasts = true;
    }
}
