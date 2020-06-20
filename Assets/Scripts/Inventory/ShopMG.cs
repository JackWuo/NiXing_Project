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

    public Text goodsinfo;
    public Text MinibagitemInfo;
    public Text CoinCount;

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
        chooseBagReflash(0);
        chooseBagReflash(2);
        instance.goodsinfo.text = "";
        MinibagitemInfo.text = "";
    }

    public static void reflashGoodsinfo(string newgoodsinfo, bool ismyitem)
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

            thisitemlists[i].GetComponent<ShopSlot>().setShopSlot(thisbag.itemlist[i]);
            thisitemlists[i].GetComponent<ShopSlot>().shopslotid = i;

            thisitemlists[i].GetComponent<ShopSlot>().ismybagItem = judgebag;
        }
    }

    public static void chooseBagReflash(int sign)
    {
        ///0- 刷新商店，1-刷新迷你包的物品，2-刷新迷你包的装备
        switch (sign)
        {
            case 0:
                {
                    GenGoods();
                    reflashShop(instance.ShopBag, instance.shopgird, instance.goods, false);
                    instance.CoinCount.text = string.Join("", instance.playEqBag.coin.itemHeld);
                    break;
                }
            case 1:
                {
                    reflashShop(instance.playGoodsBag, instance.MiniBagGrid, instance.minibagGoods, true);
                    instance.MinibagitemInfo.text = "";
                    break;
                }
            case 2:
                {
                    reflashShop(instance.playEqBag, instance.MiniBagGrid, instance.minibagGoods, true);
                    instance.MinibagitemInfo.text = "";
                    break;
                }
        }
    }

}
