using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemonWorld : MonoBehaviour
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
            tempid = rd.Next(0, 2);
        }
        else if (tempid < 80)
        {
            tempid = rd.Next(3, 5);
        }
        else {
            tempid = rd.Next(6, 8);
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
                thisitem.itemHeld += 1;
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
