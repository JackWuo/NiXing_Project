using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour
{
    public int Eqslotid;
    public bool isEquip;
    public Image EqslotImage;
    public item EquipSlotItem;

    public void setEqslot(item mitem)
    {
        if (mitem != null)
        {
            isEquip = true;
            EqslotImage.sprite = mitem.itemimg;
            EquipSlotItem = mitem;
        }
    }

}
