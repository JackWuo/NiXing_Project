using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public Text price;
    public Image goodsimg;
    public GameObject selecticon;
    private string goodsinfo;
    public int shopslotid;
    public bool ismybagItem;
    public bool isshowselect;


    public void onClickShopGoods()
    {
        ShopMG.reflashGoodsinfo(goodsinfo, ismybagItem, shopslotid);

        selecticon.SetActive(isshowselect);
    }

    public void setShopSlot(item thisitem)
    {
        if (thisitem != null)
        {
            goodsinfo = thisitem.iteminfo;
            goodsimg.sprite = thisitem.itemimg;
            if (ismybagItem)
                price.text = string.Join("", thisitem.itemHeld);
            else
                price.text = string.Join("", thisitem.itemprice);
            isshowselect = true;
        }
        else
        {
            isshowselect = false;
        }

    }
}
