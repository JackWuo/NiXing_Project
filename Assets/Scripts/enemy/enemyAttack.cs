using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAttack : MonoBehaviour
{
    // Start is called before the first frame update

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
        Debug.LogFormat("collider name:{0}", collider.name);
        if (collider.tag == "enemy") return;
        enemyAI ai = FindObjectOfType<enemyAI>();
        if(ai != null) ai.isAttack = false;

        //isAttack = false;
        //if (atk) Destroy(atk);
    }
}
