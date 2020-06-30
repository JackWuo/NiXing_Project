using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SellManControl : MonoBehaviour
{
    public Inventory ShopBag;
    public GameObject Shop;


    private void Awake()
    {
        Shop = GameObject.Find("SellManBag");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OpenShop();
            ShopMG.chooseBagReflash(0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CloseShop();
            //ShopMG.chooseBagReflash(0);
        }
    }

    private void OpenShop()
    {
        Shop.SetActive(true);
    }

    private void CloseShop()
    {
        Shop.SetActive(false);
    }
}
