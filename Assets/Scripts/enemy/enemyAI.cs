using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class enemyAI : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 1f;
    public float nextWayPointDistance = 2f;

    public float stiffTime = 0.3f;
    float lastAttackedTime;

    public float atkMoveSpeed = 4.0f;
    public float attackRestTime = 2.0f;
    public float attackHoldTime = 1.5f;
    float lastAttackTime;
    public bool isAttack;
    Vector3 atkDir;


    Path path;
    int currentWayPoint = 0;
    bool reachEndofPath = false;
    Seeker seeker;
    Rigidbody2D rigidbody2d;
    Collider2D collider;
    enemyInfo einfo;

    Animator animator;
    //死亡掉落物体
    public GameObject collectObject;
    enemyAttack atk;
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }
    void UpdatePath()
    {
        if (target == null) return;
        if(seeker.IsDone()) seeker.StartPath(rigidbody2d.position, target.position, OnPathComplete);
    }
    void Start()
    {
        isAttack = false;
        lastAttackTime = 0.0f;
        seeker = GetComponent<Seeker>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        einfo = GetComponent<enemyInfo>();
        //生成怪物时需在这里把类型替换成想要的类型
        //einfo.changeEnemyType(enemyInfo.Type.witcher);
        lastAttackedTime = 0.0f;
        InvokeRepeating("UpdatePath", 0f, .5f);
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (einfo.thisInfo.IsDead)
        {
            InventoryMG.instance.Goodsbag.coin.itemHeld += 10;
            if (collectObject != null)
            {
                GameObject collect = Instantiate(collectObject, transform.position, new Quaternion(0, 0, 0, 1));
                collectObject = null;
            }
            Destroy(this.gameObject, 1.0f);

        }
        //寻路
        if (path == null | target == null) return;
        float dis = Vector2.Distance(target.position, rigidbody2d.position);

        if (currentWayPoint >= path.vectorPath.Count || dis <= nextWayPointDistance)
        {
            reachEndofPath = true;
            return;
        }
        else reachEndofPath = false;
        if (reachEndofPath) return;

        Vector2 dir = ((Vector2)(path.vectorPath[currentWayPoint]) - rigidbody2d.position).normalized;
        rigidbody2d.velocity = dir * moveSpeed;

        dis = Vector2.Distance(path.vectorPath[currentWayPoint], rigidbody2d.position);
        if (dis <= nextWayPointDistance)
        {
            currentWayPoint++;
            //Debug.LogFormat("current way point {0} distance {1}", currentWayPoint, dis);
        }


        if (einfo.thisInfo.Type != enemyInfo.Type.boss) return;
        if (Time.time - lastAttackTime > attackHoldTime) isAttack = false;
        if (Time.time - lastAttackTime > attackRestTime && !isAttack)
        {
            atk = Instantiate(Resources.Load("enemy/enemyAttack", typeof(enemyAttack))) as enemyAttack;
            atk.E = this;
            Vector2 atkPos = collider.ClosestPoint((Vector2)(target.position));
            atk.GetComponent<Transform>().position = new Vector3(atkPos.x, atkPos.y, 0);
            atkDir = (target.position - this.transform.position).normalized;
            isAttack = true;
            lastAttackTime = Time.time;
            Destroy(atk.gameObject, attackHoldTime);
        }
        if (isAttack)
        {
            atk.GetComponent<Rigidbody2D>().velocity = rigidbody2d.velocity + atkMoveSpeed * (Vector2)atkDir;
                //Debug.LogFormat("enemy attack velocity {0}", atk.GetComponent<Rigidbody2D>().velocity);
        }
        else if (atk)
            Destroy(atk.gameObject, 0.3f);
        animator.SetBool("is_attack", isAttack);
        animator.SetBool("is_dead", einfo.thisInfo.IsDead);


        //攻击
        //atk.Attack(target);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //attackInfo atkInfo = collision.gameObject.GetComponent<attackInfo>();
        //if (atkInfo != null && Time.time - lastAttackedTime > stiffTime)
        //{
        //    lastAttackedTime = Time.time;
        //    SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        //    sprite.color = Color.red;
        //    rigidbody2d.velocity = collision.GetContact(0).normal * moveSpeed;
        //}
    }
    //遭受trigger触发器的碰撞体碰撞时的回调函数
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.LogFormat("enemyTiggerEnter, collider name:{0}", collider.name);
        //如果是伤害则表现受击硬直
        if (collider.tag != "enemy" && Time.time - lastAttackedTime > stiffTime)
        {
            lastAttackedTime = Time.time;
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = Color.red;
            rigidbody2d.velocity = (GetComponent<Transform>().position - collider.GetComponentInParent<Transform>().position) * moveSpeed;

            int change = 0;
            switch(collider.gameObject.name)
            {
                case "weapon":
                    change = collider.gameObject.GetComponentInParent<playerStatus>().AttackPower;
                    break;
                case "playerSkill(Clone)":
                    change = collider.gameObject.GetComponent<playerAttack>().Pstatus.skillPower;
                    break;
                default:
                    break;
            }
            einfo.UpdateBlood(-change);
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        Debug.LogFormat("enemyTiggerExit, collider name:{0}", collider.name);

        if (collider.tag != "enemy")
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = Color.white;
        }
    }
}
