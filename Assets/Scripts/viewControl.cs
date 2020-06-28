using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class viewControl : MonoBehaviour
{
    Transform viewTransform;
    player p;
    // Start is called before the first frame update
    void Start()
    {
        viewTransform = GetComponent<Transform>();
        p = FindObjectOfType<player>();

    }

    // Update is called once per frame
    void Update()
    {
        if (p == null) return;
        Vector3 pos = p.transform.position;
        viewTransform.position = new Vector3(pos.x, pos.y, pos.z - 1.0f);

    }

}
