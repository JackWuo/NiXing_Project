using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class item : ScriptableObject
{
    public string name;
    public Sprite itemimg;
    public int itemHeld = 1;
    public bool isequip;
    public bool iscoin;
    public int itemprice;

    public int sellprice;
    /***
     * itemid = enum{非装备物品，头盔，战甲，利刃，手套，斗篷，靴....}
     ***/
    public int attributeid = 0;
    public int itemid;

    public int itemattribute;


    [TextArea]
    public string iteminfo;
}
