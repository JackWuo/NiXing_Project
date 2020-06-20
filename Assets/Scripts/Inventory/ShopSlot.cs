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


    public void onClickShopGoods()
    {
        ShopMG.reflashGoodsinfo(goodsinfo, ismybagItem);
        selecticon.SetActive(true);
    }

    public void setShopSlot(item thisitem)
    {
        if (thisitem != null)
        {
            goodsinfo = thisitem.iteminfo;
            goodsimg.sprite = thisitem.itemimg;
            price.text = string.Join("", thisitem.itemprice);
        }

    }
}
