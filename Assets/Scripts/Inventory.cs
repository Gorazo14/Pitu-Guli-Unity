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
    [SerializeField] private GameObject[] items;
    [SerializeField] private Transform itemSpawnPoint;

    private void Start()
    {
        inventoryBackground.SetActive(false);
        player.enabled = true;
        gameInput.OnInventoryOpenClose += GameInput_OnInventoryOpenClose;
        player.OnItemPickedUp += Player_OnItemPickedUp;
    }

    private void Player_OnItemPickedUp(object sender, System.EventArgs e)
    {
        foreach(GameObject item in items)
        {
            if (!item.activeSelf)
            {
                item.SetActive(true);
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
            player.enabled = true;
        }else
        {
            inventoryBackground.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            player.enabled = false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Instantiate(medkitPrefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
        eventData.pointerDrag.gameObject.SetActive(false);

        eventData.pointerDrag.gameObject.GetComponent<DragDrop>().GetCanvasGroup().alpha = 1f;
        eventData.pointerDrag.gameObject.GetComponent<DragDrop>().GetCanvasGroup().blocksRaycasts = true;
        eventData.pointerDrag.gameObject.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.gameObject.GetComponent<DragDrop>().GetRectTransform().anchoredPosition;
    }
}
