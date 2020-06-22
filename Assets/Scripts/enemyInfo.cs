using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class enemyInfo : MonoBehaviour
{
    public static enemyInfo instance;
    public enum Type { warrier, witcher };
    public Type type = Type.warrier;
    public float pushBackScale = 10.0f;

    //怪物的攻击力和血量
    private int warrierAttackPower = 5;
    private int witcherAttackPower = 10;
    private int bossAttackPower = 15;
    private int bossSkill = 30;
    private int warrierBlood = 30;
    private int witcherBlood = 40;
    private int bossBlood = 200;
    //通过怪物类型获得攻击力或者血量
    public int enemyAttackPower(int enemyType)
    {
        if (enemyType == 0) return warrierAttackPower;
        if (enemyType == 1) return witcherAttackPower;
        if (enemyType == 2) return bossAttackPower;
        return 0;
    }
    public int enemyBlood(int enemyType)
    {
        if (enemyType == 0) return warrierBlood;
        if (enemyType == 1) return witcherBlood;
        if (enemyType == 2) return bossBlood;
        return 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
