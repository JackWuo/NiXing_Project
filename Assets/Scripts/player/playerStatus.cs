using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class playerStatus : MonoBehaviour
{
    public static playerStatus instance;

    //血条与蓝条系统
    private int curBlood = 100;
    private int curBlue = 0;
    private int maxBlood = 100;
    private int maxBlue = 100;

    public Slider bloodSlider;
    public Slider blueSlider;


    //受到攻击
    public void downBlood(int enemyAttack)
    {
        curBlood -= enemyAttack;
        if (curBlood <= 0) curBlood = 0;
    }
    //血药
    public void upBlood()
    {
        curBlood += 20;
        if (curBlood >= 100) curBlood = 100;
    }
    //蓝药
    public void upBlue()
    {
        curBlue += 20;
        if (curBlue >= 100) curBlue = 100;
    }
    //释放技能
    public bool skill()
    {
        if (curBlue >= 25) { curBlue -= 25; return true; }
        else return false;
    }
    public int getBlood() { return curBlood; }
    public int getBlue() { return curBlue; }

    //攻击力系统（装备）
    int playerAttack = 10;
    int playerSkill = 30;
    public int getAttack() { return playerAttack; }
    public void upAttackPower(int power)
    {
        playerAttack += power;
    }


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        bloodSlider.value = curBlood;
        blueSlider.value = curBlue;
    }
    // Update is called once per frame
    void Update()
    {
        bloodSlider.value = curBlood;
        blueSlider.value = curBlue;
        if (Input.GetKeyDown(KeyCode.Q))
        {   //如果有剩余血药剂
            //if (InventoryMG.GetGoods(0)) { 
            upBlood();
            //}
            Debug.Log("您按下了Q键");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {   //如果有剩余血药剂
            //if (InventoryMG.GetGoods(1)) { 
            upBlue();
            //}
            Debug.Log("您按下了E键");
        }
    }
}