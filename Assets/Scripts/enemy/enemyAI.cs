using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class enemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float moveSpeed = 1f;
    public float nextWayPointDistance = 2f;

    public float stiffTime = 0.3f;
    float lastAttackedTime;
    Path path;
    int currentWayPoint = 0;
    bool reachEndofPath = false;
    Seeker seeker;
    Rigidbody2D rigidbody2d;

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
        lastAttackedTime = 0.0f;
        InvokeRepeating("UpdatePath", 0f, .5f);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        attackInfo atkInfo = collision.GetComponentInParent<attackInfo>();
        if (atkInfo != null && Time.time - lastAttackedTime > stiffTime)
        {
            lastAttackedTime = Time.time;
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = Color.red;
            rigidbody2d.velocity = (GetComponent<Transform>().position - collision.GetComponentInParent<Transform>().position) * moveSpeed;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.color = Color.white;
    }
}
