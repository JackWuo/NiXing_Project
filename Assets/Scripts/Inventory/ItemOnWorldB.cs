using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemOnWorldB : MonoBehaviour
{
    public item thisitem;
    public Inventory Equipbag;
    public Inventory Goodsbag;
    public Inventory StaticBag;

    private void OnEnable()
    {
        System.Random rd = new System.Random(Guid.NewGuid().GetHashCode());
        int tempid = rd.Next(1, 100);
        if (tempid < 50)
        {
            tempid = rd.Next(1, 2);
        }
        else if (tempid < 65)
        {
            tempid = 5;
        }
        else
        {
            tempid = 0;
        }
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
