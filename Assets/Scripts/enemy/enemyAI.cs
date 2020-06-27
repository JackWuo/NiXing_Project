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

    GameObject atk;
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
        if(seeker.IsDone()) seeker.StartPath(rigidbody2d.position, target.position, OnPathComplete);
    }
    void Start()
    {
        seeker = GetComponentInParent<Seeker>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        lastAttackedTime = 0.0f;
        InvokeRepeating("UpdatePath", 0f, .5f);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //寻路
        if (path == null) return;
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

        if (Time.time - lastAttackTime > attackHoldTime) isAttack = false;
        if (Time.time - lastAttackTime > attackRestTime && !isAttack)
        {
            atk = Instantiate(Resources.Load("enemy/enemyAttack", typeof(GameObject))) as GameObject;
            Vector2 atkPos = collider.ClosestPoint((Vector2)(target.position));
            atk.GetComponent<Transform>().position = new Vector3(atkPos.x, atkPos.y, 0);
            atkDir = (target.position - atk.GetComponentInParent<Transform>().position).normalized;
            isAttack = true;
            lastAttackTime = Time.time;
        }

        if (isAttack)
        {
            atk.GetComponent<Rigidbody2D>().velocity = atkMoveSpeed * atkDir;
        }
        else
        {
            if (atk) Destroy(atk);
        }
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
        Debug.LogFormat("enemyTigger object name:{0}", collider.name);
        //如果是武器则表现受击硬直
        if (collider.tag != "enemy" && Time.time - lastAttackedTime > stiffTime)
        {
            lastAttackedTime = Time.time;
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = Color.red;
            rigidbody2d.velocity = (GetComponent<Transform>().position - collider.GetComponentInParent<Transform>().position) * moveSpeed;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.color = Color.white;
    }
}
