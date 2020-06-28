using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAttack : MonoBehaviour
{
    // Start is called before the first frame update
    enemyAI e;
    public enemyAI E { get => e; set => e = value; }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.LogFormat("collider name:{0}", collision.collider.name);
        //isAttack = false;
        //if (atk) Destroy(atk);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.LogFormat("enemyAttack collider name:{0}", collider.name);
        if (collider.gameObject.tag == "enemy") return;
        e.isAttack = false;

        //isAttack = false;
        //if (atk) Destroy(atk);
    }
}
