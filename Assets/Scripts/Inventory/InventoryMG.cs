using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMG : MonoBehaviour
{
    public static InventoryMG instance;

    public Inventory Equipbag;
    public Inventory Goodsbag;
    private int whichbag = 0;

    public GameObject slotgird;
    //public Slot slotpreb;
    public GameObject emptyslot;
    public Text iteminfo;

    public Text CoinCount;

    public Text HPcount;
    public Text MPcount;

    public item MP;
    public item HP;
    public item MedicineBox;

    public GameObject bag;

    public List<GameObject> slots = new List<GameObject>();
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    private void OnEnable()
    {
        reflashItem(instance.Equipbag);
        instance.iteminfo.text = "";
        reflashHMPcount();
    }

    public static void updateiteminfo(string itemdescription)
    {
        instance.iteminfo.text = itemdescription;
    }

    /*public static void CreateItem(item mitem)
    {
        Slot newitem = Instantiate(instance.slotpreb, instance.slotgird.transform.position, Quaternion.identity);
        newitem.gameObject.transform.SetParent(instance.slotgird.transform);
        newitem.slotitem = mitem;
        newitem.slotImage.sprite = mitem.itemimg;
        newitem.slotnum.text = mitem.itemHeld.ToString();
    } */

    public static void reflashItem(Inventory tempinventory)
    {
        for (int i = 0; i < instance.slotgird.transform.childCount; i++)
        {
            if (instance.slotgird.transform.childCount == 0)
            {
                break;
            }
            Destroy(instance.slotgird.transform.GetChild(i).gameObject);
        }
        instance.slots.Clear();

        for (int i = 0; i < tempinventory.itemlist.Count; i++)
        {
            //CreateItem(instance.mybag.itemlist[i]);
            instance.slots.Add(Instantiate(instance.emptyslot));
            instance.slots[i].transform.SetParent(instance.slotgird.transform);

            instance.slots[i].GetComponent<Slot>().slotid = i;
            int tempjudge = instance.slots[i].GetComponent<Slot>().setslot(tempinventory.itemlist[i]);
            if (tempjudge == 0)
            {
                tempinventory.itemlist[i] = null;
            }
        }


        ///金币数量显示刷新
        instance.CoinCount.text = string.Join("", tempinventory.coin.itemHeld);
    }

    private static void chooseBagreflash()
    {
        if (instance.whichbag == 0)
        {
            reflashItem(instance.Equipbag);
            instance.iteminfo.text = "";
        }
        else if (instance.whichbag == 1)
        {
            reflashItem(instance.Goodsbag);
            instance.iteminfo.text = "";
        }
    }

    public static void reflashbag(int sign)
    {
        instance.whichbag = sign;
        chooseBagreflash();
    }

    public void reflashEquipBag()
    {
        whichbag = 0;
        chooseBagreflash();
    }

    public void reflashGoodsBag()
    {
        whichbag = 1;
        chooseBagreflash();
    }


    public static void MGAddToBag(item thisitem, int sign)
    {
        if (sign == 0)
        {
            if (!instance.Equipbag.itemlist.Contains(thisitem))
            {
                for (int i = 0; i < instance.Equipbag.itemlist.Count; i++)
                {
                    if (instance.Equipbag.itemlist[i] == null)
                    {
                        thisitem.itemHeld = 1;
                        instance.Equipbag.itemlist[i] = thisitem;
                        break;
                    }
                }
            }
            else
            {
                thisitem.itemHeld += 1;
            }
        }
        else if (sign == 1)
        {
            if (!instance.Goodsbag.itemlist.Contains(thisitem))
            {
                for (int i = 0; i < instance.Goodsbag.itemlist.Count; i++)
                {
                    if (instance.Goodsbag.itemlist[i] == null)
                    {
                        thisitem.itemHeld = 1;
                        instance.Goodsbag.itemlist[i] = thisitem;
                        Debug.Log(i);
                        break;
                    }
                }
            }
            else
            {
                thisitem.itemHeld += 1;
            }
        }
        reflashbag(sign);
        reflashHMPcount();
    }

    private static int judgeitemInBag(Inventory thisbag, item thisitem)
    {
        for (int i = 0; i < thisbag.itemlist.Count; i++)
        {
            if (thisbag.itemlist[i] == null)
                continue;
            if (thisbag.itemlist[i].itemid == thisitem.itemid)
                return i;
        }
        return -1;
    }

    public static void OpenBag()     ////打开背包接口，调用----InventoryMG.OpenBag()
    {
        instance.bag.SetActive(true);
        reflashbag(0);
    }

    public static void ShutBag()     ////关闭背包接口，调用----InventoryMG.ShutBag()
    {
        instance.bag.SetActive(false);
    }

    public static bool GetGoods(int sign)   //使用药品接口，调用----InventoryMG.GetGoods(sign), sign-0 使用血瓶， sign-1使用蓝瓶，sign-2使用医疗箱,
    {                                      //如果背包存在对应的物品就返回true,没有或者sign不是以上三个值就返回false
        if (sign == 0)
        {
            if (instance.Goodsbag.itemlist.Contains(instance.HP)&& instance.HP.itemHeld >0)
            {
                
                if (instance.HP.itemHeld == 1)
                {
                    instance.HP.itemHeld = 0;
                    instance.Goodsbag.itemlist.Remove(instance.HP);
                }else
                    instance.HP.itemHeld -= 1;
                reflashHMPcount();
                return true;
            }
            return false;
        }else if (sign == 1)
        {
            if (instance.Goodsbag.itemlist.Contains(instance.MP)&& instance.MP.itemHeld >0)
            {
                
                if (instance.MP.itemHeld == 1)
                {
                    instance.MP.itemHeld = 0;
                    instance.Goodsbag.itemlist.Remove(instance.MP);
                }else
                    instance.MP.itemHeld -= 1;
                reflashHMPcount();
                return true;
            }
        }
        else if (sign == 2)
        {
            if (instance.Goodsbag.itemlist.Contains(instance.MedicineBox)&& instance.MedicineBox.itemHeld >0)
            {
                
                if (instance.MedicineBox.itemHeld == 1)
                {
                    instance.MedicineBox.itemHeld = 0;
                    instance.Goodsbag.itemlist.Remove(instance.MedicineBox);
                }else
                    instance.MedicineBox.itemHeld -= 1;
                reflashHMPcount();
                return true;
            }
        }
        return false;
    }

    public static void reflashHMPcount()
    {
        instance.HPcount.text = string.Join("",instance.HP.itemHeld);
        instance.MPcount.text = string.Join("", instance.MP.itemHeld);
    }
}
