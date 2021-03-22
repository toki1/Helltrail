using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemButton;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(2.0f);
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
    }

    //private void OnTriggerEnter2D(Collider2D other)
    public void addItem()
    {
        
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if(inventory.isFull[i] == false)
                {
                    
                    inventory.isFull[i] = true;
                    inventory.currentTotal += 1;
                   
                    if (inventory.currentTotal == 1 && inventory.currentSlot == 0)
                    {
                        inventory.SelectedItemIcon.enabled = true;
                    }
                    Destroy(gameObject);
                    break;
                }
            }
      
    }
}
