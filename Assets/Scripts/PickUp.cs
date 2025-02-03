using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PickUp : MonoBehaviour
{
    public GameObject PickUpText;
    private void Start()
    {
        transform.GetComponent<NetworkObject>().Spawn(true);
        PickUpText.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            
            PickUpText.SetActive(true);
            
            
            if(Input.GetKey(KeyCode.E))
            {
                this.gameObject.SetActive(false);

                Debug.Log("Picked Up");
                
                PickUpText.SetActive(false);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
       PickUpText.SetActive(false); 
    }
    
}  
    