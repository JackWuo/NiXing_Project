using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeSlot : MonoBehaviour
{
    /// <summary>
    /// attributeslotid = enum{攻击，防御，速度，法力，闪避}
    /// </summary>

    private string []attributebaseinfo = {
        "攻击：10",
        "防御：10",
        "速度：1",
        "法力：5",
        "闪避：5"
    };
    public int attributeslotid;
    public Text baseAttribute;
    public Text additionAttribute;
    public Inventory mEquipPanel;

    public void initBasePanel()
    {
        baseAttribute.text = attributebaseinfo[attributeslotid];
    }

    /// <summary>
    /// sign作为一个标记位，0时表示清除属性，1时表示设置属性
    /// </summary>
    /// <param name="attributevalue"></param>
    /// <param name="sign"></param>
    public void setAttributeSlot(int attributevalue, int sign)
    {
        if (sign == 1)
        {
             additionAttribute.text = "(+" + string.Join("", attributevalue) + ")";
        }else if (sign == 0)
        {
            additionAttribute.text = "";
        }
    }
}
