using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Inventory : MonoBehaviour, IDropHandler
{
    [SerializeField] private Transform inventoryTransform;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private GameObject inventoryBackground;
    [SerializeField] private Player player;
    [SerializeField] private GameObject medkitPrefab;
    [SerializeField] private Transform itemSpawnPoint;

    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private Gun gun;

    [SerializeField] private ItemSlot[] slots;
    [SerializeField] private Item item;

    private int i = 0;
 
    private void Start()
    {
        inventoryBackground.SetActive(false);
        player.enabled = true;
        gameInput.OnInventoryOpenClose += GameInput_OnInventoryOpenClose;
        player.OnItemPickedUp += Player_OnItemPickedUp;
    }

    private void Player_OnItemPickedUp(object sender, Player.OnItemPickedUpEventArgs e)
    {
        /*
        if (!e.pickUp.GetPickUpSO().isStackable)
        {
            while (i < slots.Length)
            {
                if (slots[i].HasNonStackableItem())
                {
                    // Go to next slot
                    i++;
                }
                if (!slots[i].HasItem())
                {
                    foreach (Item item in items)
                    {
                        if (!item.gameObject.activeSelf)
                        {
                            item.SetParentItemSlot(slots[i]);
                            slots[i].SetItem(item);
                            item.GetItemRectTransform().anchoredPosition = slots[i].GetItemSlotRectTransform().anchoredPosition;

                            item.SetStackability(false);

                            item.SetPickUpSO(e.pickUp.GetPickUpSO());

                            item.gameObject.SetActive(true);

                            slots[i].itemsOnSlotCount++;
                            break;
                        }
                    }
                }
                break;
            }
        }
        else
        {
            while (i < slots.Length)
            {
                if (slots[i].itemsOnSlotCount >= e.pickUp.GetPickUpSO().maxStack || slots[i].HasNonStackableItem())
                {
                    // Go to next slot
                    i++;
                }
                if (!slots[i].HasItem() || slots[i].itemsOnSlotCount < e.pickUp.GetPickUpSO().maxStack)
                {
                    foreach (Item item in items)
                    {
                        if (!item.gameObject.activeSelf)
                        {
                            item.SetParentItemSlot(slots[i]);
                            slots[i].SetItem(item);
                            item.GetItemRectTransform().anchoredPosition = slots[i].GetItemSlotRectTransform().anchoredPosition;

                            item.SetStackability(true);

                            item.SetPickUpSO(e.pickUp.GetPickUpSO());

                            Debug.Log(item.transform.name);
                            item.gameObject.SetActive(true);

                            slots[i].itemsOnSlotCount++;
                            break;
                        }
                    }
                }
                break;
            }
        }
        */
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

        i = 0;
    }
}
