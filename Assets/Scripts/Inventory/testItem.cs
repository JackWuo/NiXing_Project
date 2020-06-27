using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testItem : MonoBehaviour
{
    /// <summary>
    /// 掉落装备，DropItemSlot作为一个规范，需要在打死怪兽之后随机生成装备
    /// </summary>
    public GameObject DropItemSlot;
    public Inventory StaticBag;
    private GameObject DropItem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DropItem = Instantiate(DropItemSlot, gameObject.transform);
            System.Random rd = new System.Random(Guid.NewGuid().GetHashCode());
            int tempid = rd.Next(StaticBag.itemlist.Count);
            DropItem.GetComponent<ItemonWorld>().thisitem = StaticBag.itemlist[tempid];
            DropItem.GetComponentInChildren<SpriteRenderer>().sprite = StaticBag.itemlist[tempid].itemimg;
            //Destroy(gameObject);
        }
    }
}
