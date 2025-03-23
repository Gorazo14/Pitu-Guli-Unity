using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour, IDropHandler
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private GameObject inventoryBackground;
    [SerializeField] private Player player;
    [SerializeField] private GameObject medkitPrefab;
    [SerializeField] private Transform itemSpawnPoint;

    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private Gun gun;

    [SerializeField] private Item[] items;
    [SerializeField] private ItemSlot[] itemSlots;

    private void Start()
    {
        inventoryBackground.SetActive(false);
        player.enabled = true;
        gameInput.OnInventoryOpenClose += GameInput_OnInventoryOpenClose;
        player.OnItemPickedUp += Player_OnItemPickedUp;
    }

    private void Player_OnItemPickedUp(object sender, System.EventArgs e)
    {
        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (!itemSlot.HasItem())
            {
                foreach (Item item in items)
                {
                    if (!item.HasParentItemSlot())
                    {
                        item.gameObject.SetActive(true);

                        itemSlot.SetItem(item.GetComponent<RectTransform>());
                        item.SetParentItemSlot(itemSlot.GetComponent<RectTransform>());

                        break;
                    }
                }
                break;
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
        eventData.pointerDrag.gameObject.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.gameObject.GetComponent<Item>().GetParentItemSlot().anchoredPosition;
    }
}
