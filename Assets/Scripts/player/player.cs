using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    bool isAttack;
    public float attackRestTime = 0.4f;
    public float attackHoldTime = 0.4f;
    float lastAttackTime;

    float lastAttackedTime;
    public float stiffTime = 0.4f;

    bool isDash;
    float lastDashTime;
    public float dashRestTime = 1.0f;
    public float dashSpeed = 10.0f;
    public float dashHoldTime = 0.2f;

    bool isMove;
    public float moveSpeed = 3.0f;

    enum Face { up, down, left, right};
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
        if (Time.time - lastAttackedTime < stiffTime) return;
        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector2 moveDir = new Vector2(horizontalInput, verticalInput);
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
            
        if(isDash) rigidbody2d.velocity = moveDir * dashSpeed;
        else rigidbody2d.velocity = moveDir * moveSpeed;
        checkFace(moveDir);

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        enemyInfo enemy = collision.gameObject.GetComponent<enemyInfo>();
        
        if(enemy != null && Time.time - lastAttackedTime > stiffTime)
        {
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
}
