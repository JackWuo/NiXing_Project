using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    public float speed;
    Vector2 movement;
    int xdirectid;
    int ydirectid;
    int speedid;

    public GameObject mybag;
    bool isopenbag;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        speedid = Animator.StringToHash("speed");

    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        openbag();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        Switchanim();
    }

    void Switchanim()
    {
        anim.SetFloat(speedid, movement.magnitude);
    }

    void openbag()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            isopenbag = !isopenbag;
            mybag.SetActive(isopenbag);
            InventoryMG.reflashbag(0);
        }
        isopenbag = mybag.activeSelf;
    }
}
