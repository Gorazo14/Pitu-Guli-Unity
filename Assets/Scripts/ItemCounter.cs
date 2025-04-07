using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemCounter : MonoBehaviour
{
    private TextMeshProUGUI counterText;

    private void Awake()
    {
        counterText = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        counterText.text = transform.parent.GetComponent<ItemSlot>().itemsOnSlotCount.ToString();
    }
}
