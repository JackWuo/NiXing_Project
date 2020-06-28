using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class enemyInfo : MonoBehaviour
{
    public static enemyInfo instance;
    public enum Type { warrier, witcher, boss};

    public struct Info
    {
        Type type;
        int attackPower;
        float attackPushScale;
        bool isAttack;

        int maxBlood;
        int currBlood;
        bool isDead;

        int skillPower;
        float skillPushScale;

        public Info(Type t, int ap, float aps, int mb, int cb, int sp, float sps)
        {
            type = t;
            attackPower = ap;
            attackPushScale = aps;
            isAttack = false;
            maxBlood = mb;
            currBlood = cb;
            isDead = false;
            skillPower = sp;
            skillPushScale = sps;
        }

        public Type Type { get => type; set => type = value; }
        public int AttackPower { get => attackPower; set => attackPower = value; }
        public float AttackPushScale { get => attackPushScale; set => attackPushScale = value; }
        public bool IsAttack { get => isAttack; set => isAttack = value; }
        public int MaxBlood { get => maxBlood; set => maxBlood = value; }
        public int CurrBlood { get => currBlood; set => currBlood = value; }
        public bool IsDead { get => isDead; set => isDead = value; }
        public int SkillPower { get => skillPower; set => skillPower = value; }
        public float SkillPushScale { get => skillPushScale; set => skillPushScale = value; }
    }

    //怪物的攻击力和血量
    Info warrierInfo = new Info(Type.warrier, 5, 10.0f, 30, 30, 0, 0);
    Info witcherInfo = new Info(Type.witcher, 10, 10.0f, 40, 40, 0, 0);
    Info bossInfo = new Info(Type.boss, 15, 10.0f, 200, 200, 30, 10.0f);
    public Info thisInfo;

    //通过怪物类型获得攻击力或者血量

    public void UpdateBlood(int change)
    {
        thisInfo.CurrBlood += change;
        if (thisInfo.CurrBlood <= 0) {
            thisInfo.CurrBlood = 0;
            thisInfo.IsDead = true;
        } 
        else if (thisInfo.CurrBlood >= thisInfo.MaxBlood) thisInfo.CurrBlood = thisInfo.MaxBlood;
        Debug.LogFormat("enenmy blood update to {0}", thisInfo.CurrBlood);
    }
    public void changeEnemyType(Type t)
    {
        switch (t)
        {
            case Type.warrier:
                thisInfo = warrierInfo;
                break;
            case Type.witcher:
                thisInfo = witcherInfo;
                break;
            case Type.boss:
                thisInfo = bossInfo;
                break;
            default:
                Debug.LogFormat("unexpected enemy type {0}, please check your code", t);
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        thisInfo = warrierInfo;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
