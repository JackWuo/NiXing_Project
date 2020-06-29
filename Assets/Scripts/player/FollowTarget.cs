using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FollowTarget : MonoBehaviour {

    private GameObject player;
    public Vector3 targetPosion;
    public float speed = 0.2f;
    private void Start()
    {
    }

    private void Update()
    {
        if(player==null)
            player = GameObject.FindGameObjectWithTag("Player");

        //   targetPosion = new Vector2( Mathf.Max( 6.63f,Mathf.Min(37, player.transform.position.x)-0.37f), Mathf.Max(3.48f,Mathf.Min(25.41f, player.transform.position.y- 1.52f)));
        targetPosion = new Vector3( player.transform.position.x,  player.transform.position.y ,-3);
        transform.DOMove(targetPosion,speed);
       // if(transform.position.x)
    }
}
