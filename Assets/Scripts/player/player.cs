using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    /* 玩家行为所需信息
     * 攻击、技能等
     * 持续时间 *holdTime 
     * 冷却时间 *restTime
     * 状态 is* 表明是否处于某种状态
     */

    //普通攻击信息
    bool isAttack;
    public float attackRestTime = 0.4f;
    public float attackHoldTime = 0.4f;
    float lastAttackTime;

    //被攻击信息
    float lastAttackedTime;
    public float stiffTime = 0.4f;

    //冲刺信息
    bool isDash;
    float lastDashTime;
    public float dashRestTime = 1.0f;
    public float dashSpeed = 10.0f;
    public float dashHoldTime = 0.2f;

    //移动信息
    bool isMove;
    public float moveSpeed = 3.0f;
    //角色朝向
    enum Face { up, down, left, right };
    Face face;


    Transform tranform;
    Animator animator;
    Rigidbody2D rigidbody2d;
    GameObject weapon;

    attackInfo attack;
    attackInfo.weaponInfo info;

    void Start()
    {
        lastAttackTime = 0.0f;
        lastAttackedTime = 0.0f;
        lastDashTime = 0.0f;
        isAttack = false;
        isMove = false;
        isDash = false;

        tranform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();

        weapon = GameObject.Find("weapon");
        attack = weapon.GetComponent<attackInfo>();
        info = attack.GetWeaponInfo((int)face);
        weapon.SetActive(false);
    }

    void checkFace(Vector2 f)
    {
        if (f == Vector2.up) face = Face.up;
        else if (f == Vector2.down) face = Face.down;
        else if (f == Vector2.left) face = Face.left;
        else if (f == Vector2.right) face = Face.right;
        else { }
    }

    void checkMove()
    {
        //处于受击硬直状态时无法移动
        if (Time.time - lastAttackedTime < stiffTime) return;


        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector2 moveDir = new Vector2(horizontalInput, verticalInput);

        /* 持续时间指的是角色处于某种状态的时间长度，通常被设置为与相应动画的持续时间相等，以保证视觉上的一致性
         * 冷却时间指的是角色进行某个动作后，到能再次进行该动作的时间长度，通常要比持续时间长
         * 处于这两个时间时系统不响应输入指令
         */
        bool canModify = false;
        //持续时间判断
        if (Time.time - lastDashTime > dashHoldTime | Time.time < dashHoldTime)
        {
            canModify = true;
            isDash = false;
        }
        else canModify = false;
        //冷却时间判断
        if (Time.time - lastDashTime > dashRestTime | Time.time < dashRestTime)
        {
            canModify = true;
        }
        else canModify = false;
        if (canModify)
        {
            if (Input.GetButton("Jump"))
            {
                isDash = true;
                lastDashTime = Time.time;
            }
            else
            {
                isDash = false;
            }
        }

        if (isDash) rigidbody2d.velocity = moveDir * dashSpeed;
        else rigidbody2d.velocity = moveDir * moveSpeed;
        checkFace(moveDir);

        //设置animator变量
        animator.SetFloat("face", (int)face);
        isMove = (verticalInput != 0) | (horizontalInput != 0);
        animator.SetBool("is_move", isMove);

    }
    void checkAttack()
    {
        bool canModify = false;
        //持续时间判断
        if (Time.time - lastAttackTime > attackHoldTime | Time.time < attackHoldTime)
        {
            canModify = true;
            isAttack = false;
        }
        else canModify = false;
        //冷却时间判断
        if (Time.time - lastAttackTime > attackRestTime | Time.time < attackRestTime)
        {
            canModify = true;
        }
        else canModify = false;

        if (canModify)
        {
            info = attack.GetWeaponInfo((int)face);
            if (Input.GetButton("Fire1"))
            {
                isAttack = true;
                lastAttackTime = Time.time;
                weapon.SetActive(true);
                weapon.GetComponent<Transform>().position = tranform.position + info.pos;
                weapon.GetComponent<Transform>().rotation = info.qua;
                weapon.GetComponent<SpriteRenderer>().sortingOrder = info.sortInLayer;
            }
            else
            {
                isAttack = false;
                weapon.SetActive(false);
            }
        }


        //进行攻击动作
        animator.SetBool("is_attack", isAttack);
        if (isAttack)
        {
            weapon.GetComponent<Transform>().RotateAround(tranform.position + info.rotPos, new Vector3(0, 0, 1), info.speed);
        }
        else
        {
            weapon.GetComponent<Transform>().rotation = info.qua;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        checkMove();
        checkAttack();
    }

    //角色受到碰撞时触发回调函数
    private void OnCollisionEnter2D(Collision2D collision)
    {
        enemyInfo enemy = collision.gameObject.GetComponent<enemyInfo>();
        //如果碰撞体为敌人则表现受击硬直
        if (enemy != null && Time.time - lastAttackedTime > stiffTime)
        {
            lastAttackedTime = Time.time;
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = Color.red;
            rigidbody2d.velocity = collision.GetContact(0).normal * moveSpeed;
            //玩家被攻击后扣血，暂定是a类怪
            playerStatus.instance.downBlood(enemyInfo.instance.enemyAttackPower(0));
            int curBlood = playerStatus.instance.getBlood();
            if (curBlood <= 0)
            {
                Debug.Log("dead");
                //死亡
                return;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        enemyInfo enemy = collision.gameObject.GetComponent<enemyInfo>();
        if (enemy != null)
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = Color.white;

        }
    }
}
