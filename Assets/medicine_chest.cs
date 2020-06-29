using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class medicine_chest : MonoBehaviour
{
    public item thisitem;
    public Inventory Equipbag;
    public Inventory Goodsbag;
    public Inventory StaticBag;
    public int tempid = 2;

    private void OnEnable()
    {
        
        thisitem = StaticBag.itemlist[tempid];
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = StaticBag.itemlist[tempid].itemimg;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (thisitem.iscoin)
            {
                thisitem.itemHeld += 5;
            }
            else
            {
                AddItem();
            }
            Destroy(gameObject);
        }
    }

    public void AddItem()
    {
        if (thisitem.isequip)
        {
            InventoryMG.MGAddToBag(thisitem, 0);
        }
        else
        {
            InventoryMG.MGAddToBag(thisitem, 1);
        }
    }
}
