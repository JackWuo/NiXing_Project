using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plsyer : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed;
    public float attackRestTime;
    float lastAttackTime;
    bool isCollision;
    Transform tranform;
    Animator animator;
    Rigidbody2D rigidbody2d;
    Collider2D barrier;

    void Start()
    {
        moveSpeed = 3.0f;
        attackRestTime = 0.4f;
        lastAttackTime = 0.0f;
        tranform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        barrier = null;

    }

    void checkMove()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (barrier != null)
        {
            //isCollision = true;
            Vector2 dis = -barrier.Distance(GetComponent<Collider2D>()).normal;
            
            if (dis.x == horizontalInput) horizontalInput = 0;
            if (dis.y == verticalInput) verticalInput = 0;
        }
        //tranform.Translate(new Vector3(verticalInput, horizontalInput, 0) * moveSpeed * Time.deltaTime);
        rigidbody2d.velocity = new Vector2(horizontalInput, verticalInput) * moveSpeed;


        animator.SetFloat("walk_x", horizontalInput);
        animator.SetFloat("walk_y", verticalInput);
        animator.SetBool("is_move", (verticalInput != 0) | (horizontalInput != 0));
        
        if(animator.GetBool("is_move"))
        {
            animator.SetFloat("last_walk_x", horizontalInput);
            animator.SetFloat("last_walk_y", verticalInput);
        }


    }
    // Update is called once per frame
    void Update()
    {
        checkMove();

        if (Time.time - lastAttackTime > attackRestTime | Time.time < attackRestTime)
        {
            if (Input.GetKey(KeyCode.J))
            {
                animator.SetBool("is_attack", true);
                lastAttackTime = Time.time;
            }
            else animator.SetBool("is_attack", false);
        }
        else { }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("hit detected");
        Debug.Log(collider.Distance(GetComponent<Collider2D>()).normal);
        barrier = collider;
        //checkMove(collider);
    }
    //private void OnTriggerStay2D(Collider2D collider)
    //{
    //    Debug.Log("hit stayed");
    //    //checkMove(collider);
    //}
    private void OnTriggerExit2D(Collider2D collider)
    {
        Debug.Log("hit exit");
        barrier = null;
    }
}
