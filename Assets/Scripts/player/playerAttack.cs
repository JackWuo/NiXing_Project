using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.LogFormat("collider name:{0}", collider.name);
        if (collider.name == "player" | collider.name == "wall") return;
        player p = FindObjectOfType<player>();
        if (p != null) p.isSkill = false;

    }
}
