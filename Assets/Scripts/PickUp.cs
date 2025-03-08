using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PickUp : MonoBehaviour
{
    [SerializeField] private string tempName;

    [SerializeField] private GameObject pickUpText;
    [SerializeField] private PickUp pickupable;
    [SerializeField] private Player player;

    private bool isPlayerInTrigger;

    private void Start()
    {
        pickUpText.SetActive(false);
        isPlayerInTrigger = false;

        Player.OnItemPickedUp += Player_OnItemPickedUp;
    }
    private void Player_OnItemPickedUp(object sender, System.EventArgs e)
    { 
        if (isPlayerInTrigger)
        {
            player.SetPickUp(pickupable);

            pickUpText.SetActive(false);
            gameObject.SetActive(false);
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            pickUpText.SetActive(true);
            isPlayerInTrigger = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        pickUpText.SetActive(false);
        isPlayerInTrigger = false;
    }
}  
    