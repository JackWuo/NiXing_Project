using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public int slotid;
    public item slotitem;
    public Image slotImage;
    public Text slotnum;

    public GameObject iteminslot;

    public string iteminfo;
    public void itemOnClick()
    {
        InventoryMG.updateiteminfo(iteminfo);
    }

    public int setslot(item mitem)
    {
        if (mitem == null || mitem.itemHeld==0)
        {
            slotitem = null;
            iteminslot.SetActive(false);
            return 0;
        }
        slotitem = mitem;
        slotImage.sprite = mitem.itemimg;
        slotnum.text = mitem.itemHeld.ToString();
        iteminfo = mitem.iteminfo;
        return 1;
    }
}
