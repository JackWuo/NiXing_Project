using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{

    Transform tranform;
    Animator animator;
    Rigidbody2D rigidbody2d;
    GameObject weapon;
    playerAttack fireball;

    //InventoryMG inventoryMG;
    playerStatus status;
    attackInfo attack;
    attackInfo.WeaponInfo weaponInfo;
    attackInfo.SkillInfo skillInfo;

    AudioSource audio;
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip skill1;
    public AudioClip skill2;
    public AudioClip dead;
    public AudioClip attacked;

    bool lastInvetoryKeyState = false;
    bool lastUseBloodRecoveryKeyState = false;
    bool lastUseBlueRecoveryKeyState = false;

    void Start()
    {

        tranform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        status = GetComponent<playerStatus>();
        //inventoryMG = FindObjectOfType<InventoryMG>();
        weapon = GameObject.Find("weapon");
        attack = GetComponent<attackInfo>();
        weaponInfo = attack.GetWeaponInfo((int)status.FaceTo);
        skillInfo = attack.GetSkillInfo((int)status.FaceTo);
        weapon.SetActive(false);
    }

    void CheckAttacked()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (status.IsAttacked)
        {
            sprite.color = Color.red;

        }
        else sprite.color = Color.white;
    }
    void CheckMove()
    {
        //处于受击硬直状态时无法移动
        if (status.IsAttacked | status.IsDead) return;

        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector2 moveDir = new Vector2(horizontalInput, verticalInput);
        if (moveDir != new Vector2(0, 0)) status.LastDir = moveDir;
        //冷却时间判断
        if (Time.time - status.LastDashTime > status.dashRestTime | Time.time < status.dashRestTime)
        {
            if (Input.GetButton("Jump"))
            {
                status.IsDash = true;
                status.LastDashTime = Time.time;
            }
            else
            {
                status.IsDash = false;
            }
        }

        if (status.IsDash) rigidbody2d.velocity = status.LastDir * status.dashSpeed;
        else rigidbody2d.velocity = moveDir * status.moveSpeed;
        status.CheckFace(moveDir);
        if (status.FaceTo == playerStatus.Face.left) tranform.rotation = new Quaternion(0, 1, 0, 0);
        else if(status.FaceTo == playerStatus.Face.right) tranform.rotation = new Quaternion(0, 0, 0, 1);
        //设置animator变量
        //animator.SetFloat("face", (int)status.FaceTo);
        status.IsMove = (verticalInput != 0) | (horizontalInput != 0);
        //animator.SetBool("is_move", status.IsMove);

    }
    void CheckAttack()
    {
        if (status.IsAttacked | status.IsSkill | status.IsDead) return;
        if (Time.time - status.LastAttackTime > status.attackRestTime | Time.time < status.attackRestTime)
        {
            weaponInfo = attack.GetWeaponInfo((int)status.FaceTo);
            if (Input.GetButton("Fire1") && !status.IsAttack)
            {
                status.IsAttack = true;
                status.LastAttackTime = Time.time;
                weapon.SetActive(true);
                weapon.GetComponent<Transform>().position = tranform.position + weaponInfo.pos;
                weapon.GetComponent<Transform>().rotation = weaponInfo.qua;
                weapon.GetComponent<SpriteRenderer>().sortingOrder = weaponInfo.sortInLayer;
            }
            else
            {
                status.IsAttack = false;
                weapon.SetActive(false);
            }
        }


        //进行攻击动作
        animator.SetBool("is_attack", status.IsAttack);
        if (status.IsAttack)
        {
            weapon.GetComponent<Transform>().RotateAround(tranform.position + weaponInfo.rotPos, new Vector3(0, 0, 1), weaponInfo.speed);
            if(!audio.isPlaying) audio.PlayOneShot((int)Time.time % 2  == 0 ? attack1 : attack2);
        }
        else
        {
            weapon.GetComponent<Transform>().rotation = weaponInfo.qua;
        }
    }

    void CheckSkill()
    {
        if (status.IsAttacked | status.IsAttack | status.IsDead) return;
        if (Time.time - status.LastSkillTime > status.skillRestTime | Time.time < status.skillRestTime)
        {
            skillInfo = attack.GetSkillInfo((int)status.FaceTo);
            status.SkillDir = status.LastDir;
            if (Input.GetButton("Fire2") && !status.IsSkill && status.CurBlue > status.SkillBlue)
            {
                status.IsSkill = true;
                status.LastSkillTime = Time.time;
                fireball = Instantiate(Resources.Load("player/playerSkill", typeof(playerAttack))) as playerAttack;
                fireball.Pstatus = status;
                //fireball.transform.SetParent(this.transform);
                //Vector2 skillPos = collider.ClosestPoint((Vector2)(target.position));
                fireball.GetComponent<Transform>().position = tranform.position + skillInfo.pos;
                fireball.GetComponent<Transform>().rotation = skillInfo.qua;
                Destroy(fireball.gameObject, status.skillHoldTime);

                status.UpdateBlue(-status.SkillBlue);
            }
            else
            {
                status.IsSkill = false;
            }
        }


        //进行攻击动作
        animator.SetBool("is_skill", status.IsSkill);
        if (status.IsSkill)
        {
            //rigidbody2d.velocity + 
            fireball.GetComponent<Rigidbody2D>().velocity = skillInfo.speed * (Vector2)status.SkillDir;
            if (!audio.isPlaying) audio.PlayOneShot((int)Time.time % 2 == 0 ? skill1 : skill2);
            //Debug.LogFormat("fireball velocity {0}", fireball.GetComponent<Rigidbody2D>().velocity);
        }
        else
        {
            if (fireball) Destroy(fireball.gameObject, 0.3f);
        }
    }

    void CheckDead()
    {
        //status.IsDead = true;
        if (status.IsDead)
        {
            rigidbody2d.velocity = new Vector2(0, 0);
            if (!audio.isPlaying) audio.PlayOneShot(dead);
            animator.SetBool("is_dead", status.IsDead);
            Destroy(this.gameObject, 1.0f);
            GameManager.Instance.Dead();
        }

    }

   

    void enquire(GameObject collect)
    {
        //捡起物品
    }
    void drop()
    {
        //丢弃物品
    }

    void CheckBagOpen()
    {

        if(!lastInvetoryKeyState && Input.GetKey(KeyCode.Tab))
        {
            if(status.IsBagOpen) { status.IsBagOpen = false; InventoryMG.ShutBag(); }
            else { status.IsBagOpen = true; InventoryMG.OpenBag(); }
        }
        lastInvetoryKeyState = Input.GetKey(KeyCode.Tab);
    }

    void CheckItemUse()
    {
        if (!lastUseBloodRecoveryKeyState && Input.GetKey(KeyCode.Q))
        {
            InventoryMG.GetGoods(0);
        }
        lastUseBloodRecoveryKeyState = Input.GetKey(KeyCode.Q);

        if (!lastInvetoryKeyState && Input.GetKey(KeyCode.E))
        {
            InventoryMG.GetGoods(1);
        }
        lastInvetoryKeyState = Input.GetKey(KeyCode.E);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //检查有持续时间的动作是否进行中
        if (Time.time - status.LastAttackedTime > status.stiffTime | Time.time < status.stiffTime) status.IsAttacked = false;
        if (Time.time - status.LastDashTime > status.dashHoldTime | Time.time < status.dashHoldTime) status.IsDash = false;
        if (Time.time - status.LastAttackTime > status.attackHoldTime | Time.time < status.attackHoldTime) status.IsAttack = false;
        if (Time.time - status.LastSkillTime > status.skillHoldTime | Time.time < status.skillHoldTime) status.IsSkill = false;

        CheckBagOpen();
        CheckAttacked();
        CheckMove();
        CheckAttack();
        CheckSkill();
        
        CheckDead();
    }

    //角色受到碰撞时触发回调函数
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //如果碰撞体为敌人则表现受击硬直
        if(collision.gameObject.tag == "enemy" && Time.time - status.LastAttackedTime > status.stiffTime)
        {
            status.IsAttacked = true;
            enemyInfo einfo = collision.gameObject.GetComponent<enemyInfo>();
            status.UpdateBlood(-status.calculateDamage(einfo.thisInfo.AttackPower));
            status.LastAttackedTime = Time.time;
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = Color.red;
            rigidbody2d.velocity = collision.GetContact(0).normal * status.moveSpeed;
            if (!audio.isPlaying) audio.PlayOneShot(attacked);

        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        enemyInfo enemy = collision.gameObject.GetComponent<enemyInfo>();
        if (collision.gameObject.tag == "enemy")
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = Color.white;

        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //enemyAttack atkInfo = collider.GetComponentInParent<enemyAttack>();
        //如果是敌人的攻击则表现受击硬直
        if (collider.gameObject.tag == "enemy" && Time.time - status.LastAttackedTime > status.stiffTime)
        {
            status.IsAttacked = true;
            //enemyAttack atkInfo = collider.GetComponentInParent<enemyAttack>();
            enemyInfo einfo = collider.gameObject.GetComponent<enemyAttack>().E.GetComponent<enemyInfo>();
            
            status.UpdateBlood(-status.calculateDamage(einfo.thisInfo.AttackPower));

            if (!audio.isPlaying) audio.PlayOneShot(attacked);

            status.LastAttackedTime = Time.time;

            rigidbody2d.velocity = (tranform.position - collider.GetComponentInParent<Transform>().position) * status.moveSpeed;
        }

        if(collider.gameObject.tag == "collect" && Input.GetKeyDown(KeyCode.E))
        {
            enquire(collider.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {

    }
}
