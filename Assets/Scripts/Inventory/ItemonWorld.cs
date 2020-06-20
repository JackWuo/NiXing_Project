using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemonWorld : MonoBehaviour
{
    public item thisitem;
    public Inventory Equipbag;
    public Inventory Goodsbag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AddItem();
            Destroy(gameObject);
        }
    }

    public void AddItem()
    {
        if (thisitem.isequip)
        {
            AddToBag(Equipbag, 0);
        }
        else
        {
            AddToBag(Goodsbag, 1);
        }
    }

    private void AddToBag(Inventory bag, int sign)
    {
        if (thisitem.Equals(bag.coin))
        {
            bag.coin.itemHeld += 1;
        }
        else if (!bag.itemlist.Contains(thisitem))
        {
            //mybag.itemlist.Add(thisitem);
            //InventoryMG.CreateItem(thisitem);
            for (int i = 0; i < bag.itemlist.Count; i++)
            {
                if (bag.itemlist[i] == null)
                {
                    thisitem.itemHeld = 1;
                    bag.itemlist[i] = thisitem;
                    break;
                }
            }
        }
        else if(bag.itemlist.Contains(thisitem))
        {
            thisitem.itemHeld += 1;
        }
        InventoryMG.reflashbag(sign);
    }
}
