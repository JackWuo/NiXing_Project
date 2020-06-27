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

    /* 持续时间指的是角色处于某种状态的时间长度，通常被设置为与相应动画的持续时间相等，以保证视觉上的一致性
     * 冷却时间指的是角色进行某个动作后，到能再次进行该动作的时间长度，通常要比持续时间长
     * 处于这两个时间时系统不响应输入指令
     */
    //普通攻击信息
    bool isAttack;
    public float attackRestTime = 0.4f;
    public float attackHoldTime = 0.4f;
    float lastAttackTime;

    //技能信息
    public bool isSkill;
    public float skillRestTime = 0.4f;
    public float skillHoldTime = 0.4f;
    public float skillSpeed = 3f;
    Vector3 skillDir;
    float lastSkillTime;

    //被攻击信息
    bool isAttacked; //硬直状态
    float lastAttackedTime;
    public float stiffTime = 0.4f;

    //冲刺信息
    bool isDash;
    float lastDashTime;
    public float dashRestTime = 1.0f;
    public float dashSpeed = 10.0f;
    public float dashHoldTime = 0.2f;
    Vector2 lastDir;

    //移动信息
    bool isMove;
    public float moveSpeed = 3.0f;
    //角色朝向
    public enum Face { up, down, left, right};
    Face face;

    public bool isDead;

    Transform tranform;
    Animator animator;
    Rigidbody2D rigidbody2d;
    GameObject weapon;
    GameObject fireball;

    attackInfo attack;
    attackInfo.WeaponInfo weaponInfo;
    attackInfo.SkillInfo skillInfo;

    void Start()
    {
        lastAttackTime = 0.0f;
        lastAttackedTime = 0.0f;
        lastDashTime = 0.0f;
        skillDir = new Vector3(0, 0, 0);
        lastDir = new Vector2(1, 0);
        isSkill = false;
        isAttacked = false;
        isAttack = false;
        isMove = false;
        isDash = false;
        isDead = false;

        tranform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();

        weapon = GameObject.Find("weapon");
        attack = GetComponent<attackInfo>();
        weaponInfo = attack.GetWeaponInfo((int)face);
        skillInfo = attack.GetSkillInfo((int)face);
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

    void CheckMove()
    {
        //处于受击硬直状态时无法移动
        if (isAttacked | isDead) return;

        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector2 moveDir = new Vector2(horizontalInput, verticalInput);
        if (moveDir != new Vector2(0, 0)) lastDir = moveDir;
        //冷却时间判断
        if (Time.time - lastDashTime > dashRestTime | Time.time < dashRestTime)
        {
            if (Input.GetButton("Jump") && !isDash)
            {
                isDash = true;
                lastDashTime = Time.time;
            }
            else
            {
                isDash = false;
            }
        }

        if (isDash) rigidbody2d.velocity = lastDir * dashSpeed;
        else rigidbody2d.velocity = moveDir * moveSpeed;
        checkFace(moveDir);

        //设置animator变量
        animator.SetFloat("face", (int)face);
        isMove = (verticalInput != 0) | (horizontalInput != 0);
        animator.SetBool("is_move", isMove);

    }
    void CheckAttack()
    {
        if (isAttacked | isSkill | isDead) return;
        if (Time.time - lastAttackTime > attackRestTime | Time.time < attackRestTime)
        {
            weaponInfo = attack.GetWeaponInfo((int)face);
            if (Input.GetButton("Fire1") && !isAttack)
            {
                isAttack = true;
                lastAttackTime = Time.time;
                weapon.SetActive(true);
                weapon.GetComponent<Transform>().position = tranform.position + weaponInfo.pos;
                weapon.GetComponent<Transform>().rotation = weaponInfo.qua;
                weapon.GetComponent<SpriteRenderer>().sortingOrder = weaponInfo.sortInLayer;
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
            weapon.GetComponent<Transform>().RotateAround(tranform.position + weaponInfo.rotPos, new Vector3(0, 0, 1), weaponInfo.speed);
        }
        else
        {
            weapon.GetComponent<Transform>().rotation = weaponInfo.qua;
        }
    }

    void CheckSkill()
    {
        if (isAttacked | isAttack | isDead) return; 
        if (Time.time - lastSkillTime > skillRestTime | Time.time < skillRestTime)
        {
            skillInfo = attack.GetSkillInfo((int)face);
            skillDir = lastDir;
            if (Input.GetButton("Fire2") && !isSkill)
            {
                isSkill = true;
                lastSkillTime = Time.time;
                fireball = Instantiate(Resources.Load("player/playerSkill", typeof(GameObject))) as GameObject;
                //Vector2 skillPos = collider.ClosestPoint((Vector2)(target.position));
                fireball.GetComponent<Transform>().position = tranform.position + skillInfo.pos;
                fireball.GetComponent<Transform>().rotation = skillInfo.qua;
            }
            else
            {
                isSkill = false;
            }
        }


        //进行攻击动作
        animator.SetBool("is_attack", isSkill);
        if (isSkill)
        {
            fireball.GetComponent<Rigidbody2D>().velocity = skillInfo.speed * skillDir;
        }
        else
        {
            if (fireball) Destroy(fireball);
        }
    }

    void CheckDead()
    {
        //isDead = true;
        if (isDead) rigidbody2d.velocity = new Vector2(0, 0);
        animator.SetBool("is_dead", isDead);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //检查有持续时间的动作是否进行中
        if (Time.time - lastAttackedTime > stiffTime | Time.time < stiffTime) isAttacked = false;
        if (Time.time - lastDashTime > dashHoldTime | Time.time < dashHoldTime) isDash = false;
        if (Time.time - lastAttackTime > attackHoldTime | Time.time < attackHoldTime) isAttack = false;
        if (Time.time - lastSkillTime > skillHoldTime | Time.time < skillHoldTime) isSkill = false;

        CheckMove();
        CheckAttack();
        CheckSkill();
        CheckDead();
    }

    //角色受到碰撞时触发回调函数
    private void OnCollisionEnter2D(Collision2D collision)
    {
        enemyInfo enemy = collision.gameObject.GetComponent<enemyInfo>();
        //如果碰撞体为敌人则表现受击硬直
        if(enemy != null && Time.time - lastAttackedTime > stiffTime)
        {
            isAttacked = true;
            isDead = true;
            lastAttackedTime = Time.time;
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = Color.red;
            rigidbody2d.velocity = collision.GetContact(0).normal * moveSpeed;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemyAttack atkInfo = collision.GetComponentInParent<enemyAttack>();
        //如果是敌人的攻击则表现受击硬直
        if (atkInfo != null && Time.time - lastAttackedTime > stiffTime)
        {
            isAttacked = true;
            isDead = true;

            lastAttackedTime = Time.time;
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = Color.red;
            rigidbody2d.velocity = (tranform.position - collision.GetComponentInParent<Transform>().position) * moveSpeed;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.color = Color.white;
    }
}
