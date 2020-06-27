using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ShopMG : MonoBehaviour
{
    static ShopMG instance;

    public Inventory staticBag;
    public Inventory ShopBag;
    public Inventory playEqBag;
    public Inventory playGoodsBag;

    public GameObject shopslot;
    public GameObject shopgird;
    public GameObject MiniBagGrid;
    public GameObject WarningPanel;

    public Text goodsinfo;
    public Text MinibagitemInfo;
    public Text CoinCount;

    public int choseid;
    public bool ismybaggoods;

    public Image sellgoodsicon;
    public Text sellcoin;
    public GameObject sellPanel;

    public int playerwhichbag;

    public List<GameObject> goods = new List<GameObject>();
    public List<GameObject> minibagGoods = new List<GameObject>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    public void OnEnable()
    {
        GenGoods();
        chooseBagReflash(0);
        chooseBagReflash(2);
        instance.goodsinfo.text = "";
        instance.MinibagitemInfo.text = "";
    }

    public static void reflashGoodsinfo(string newgoodsinfo, bool ismyitem, int thisid)
    {
        if (ismyitem)
            instance.MinibagitemInfo.text = newgoodsinfo;
        else
            instance.goodsinfo.text = newgoodsinfo;
        for (int i = 0; i < instance.goods.Count; i++)
        {
            instance.goods[i].GetComponent<ShopSlot>().selecticon.SetActive(false);
        }
        for (int i = 0; i < instance.minibagGoods.Count; i++)
        {
            instance.minibagGoods[i].GetComponent<ShopSlot>().selecticon.SetActive(false);
        }
        instance.choseid = thisid;
        instance.ismybaggoods = ismyitem;
    }

    private static void GenGoods()
    {
        for (int i = 0; i < instance.ShopBag.itemlist.Count; i++)
        {
            System.Random rd = new System.Random(Guid.NewGuid().GetHashCode());
            int tempindex = rd.Next(instance.staticBag.itemlist.Count);
            instance.ShopBag.itemlist[i] = instance.staticBag.itemlist[tempindex];
        }
    }


    private static void reflashShop(Inventory thisbag, GameObject grid, List<GameObject>thisitemlists, bool judgebag)
    {
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            if (grid.transform.childCount == 0)
                break;
            Destroy(grid.transform.GetChild(i).gameObject);
        }
        thisitemlists.Clear();
        for (int i = 0; i < thisbag.itemlist.Count; i++)
        {
            thisitemlists.Add(Instantiate(instance.shopslot, grid.transform));

            thisitemlists[i].GetComponent<ShopSlot>().shopslotid = i;
            thisitemlists[i].GetComponent<ShopSlot>().ismybagItem = judgebag;
            thisitemlists[i].GetComponent<ShopSlot>().setShopSlot(thisbag.itemlist[i]);
        }
    }

    public static void chooseBagReflash(int sign)
    {
        ///0- 刷新商店，1-刷新迷你包的物品，2-刷新迷你包的装备
        switch (sign)
        {
            case 0:
                {
                    reflashShop(instance.ShopBag, instance.shopgird, instance.goods, false);
                    instance.CoinCount.text = string.Join("", instance.playEqBag.coin.itemHeld);
                    break;
                }
            case 1:
                {
                    reflashShop(instance.playGoodsBag, instance.MiniBagGrid, instance.minibagGoods, true);
                    instance.MinibagitemInfo.text = "";
                    instance.playerwhichbag = 1;
                    break;
                }
            case 2:
                {
                    reflashShop(instance.playEqBag, instance.MiniBagGrid, instance.minibagGoods, true);
                    instance.MinibagitemInfo.text = "";
                    instance.playerwhichbag = 2;
                    break;
                }
        }
    }

    public void BuyGoods()
    {
        if (!instance.ismybaggoods && instance.ShopBag.itemlist[instance.choseid]!=null)
        {
            int tempprice = instance.ShopBag.itemlist[instance.choseid].itemprice;
            if (tempprice <= instance.playEqBag.coin.itemHeld)
            {

                instance.playEqBag.coin.itemHeld -= tempprice;
                if (instance.ShopBag.itemlist[instance.choseid].isequip)
                {
                    InventoryMG.MGAddToBag(instance.ShopBag.itemlist[instance.choseid], 0);
                    chooseBagReflash(2);
                }
                else
                {
                    InventoryMG.MGAddToBag(instance.ShopBag.itemlist[instance.choseid], 1);
                    chooseBagReflash(1);
                }
                instance.goodsinfo.text = "";
                instance.ShopBag.itemlist[instance.choseid] = null;
                chooseBagReflash(0);
            }
            else
            {
                instance.WarningPanel.SetActive(true);
            }
        }
    }

    public void SellGoods(Inventory thisbag)
    {
        if (instance.ismybaggoods)
        {
            if (thisbag.itemlist[instance.choseid] != null)
            {
                instance.sellgoodsicon.sprite = thisbag.itemlist[instance.choseid].itemimg;
                instance.sellcoin.text = string.Join("", thisbag.itemlist[instance.choseid].sellprice);
                instance.sellPanel.SetActive(true);
            }
        }
    }

    public void ChoseSellBag()
    {
        if (instance.playerwhichbag == 2)
        {
            SellGoods(instance.playEqBag);
        }else if (instance.playerwhichbag == 1)
        {
            SellGoods(instance.playGoodsBag);
        }
    }

    public void RemoveMyItem()
    {
        if (instance.playerwhichbag == 2)
        {
            instance.playEqBag.coin.itemHeld += instance.playEqBag.itemlist[instance.choseid].sellprice;
            if (instance.playEqBag.itemlist[instance.choseid].itemHeld > 1)
                instance.playEqBag.itemlist[instance.choseid].itemHeld -= 1;
            else if (instance.playEqBag.itemlist[instance.choseid].itemHeld == 1)
                //instance.playEqBag.itemlist[instance.choseid] = null;
                instance.playEqBag.itemlist.Remove(instance.playEqBag.itemlist[instance.choseid]);
            chooseBagReflash(2);
            instance.CoinCount.text = string.Join("", instance.playEqBag.coin.itemHeld);
        }
        else if (instance.playerwhichbag == 1)
        {
            instance.playGoodsBag.coin.itemHeld += instance.playGoodsBag.itemlist[instance.choseid].sellprice;
            if (instance.playGoodsBag.itemlist[instance.choseid].itemHeld > 1)
                instance.playGoodsBag.itemlist[instance.choseid].itemHeld -= 1;
            else if (instance.playGoodsBag.itemlist[instance.choseid].itemHeld == 1)
                instance.playGoodsBag.itemlist[instance.choseid] = null;
            chooseBagReflash(1);
            instance.CoinCount.text = string.Join("", instance.playGoodsBag.coin.itemHeld);
        }
    }
}
