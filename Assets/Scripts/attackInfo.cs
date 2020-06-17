using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackInfo : MonoBehaviour
{
    // Start is called before the first frame update
    public float pushBackScale = 1.0f;
    public float atk = 1.0f;
    //武器的初始位置以及朝向，之后会重写的所以不必在意这块硬编码= =
    public struct weaponInfo
    {
        public Vector3 pos;
        public Vector3 rotPos;
        public Quaternion qua;
        public float speed;
        public int sortInLayer;
        public weaponInfo(Vector3 p, Vector3 r, Quaternion q, float sp, int s)
        {
            pos = p;
            rotPos = r;
            qua = q;
            speed = sp;
            sortInLayer = s;
        }
    };
    private weaponInfo up, down, left, right;
    private weaponInfo face;
    void Start()
    {
        up = new weaponInfo(new Vector3(-0.4f, 0.8f, 0), new Vector3(0, 0.5f, 0), new Quaternion(0, 0, 0, 1), -4, 1);
        down = new weaponInfo(new Vector3(-0.4f, -0.8f, 0), new Vector3(0, -0.5f, 0), new Quaternion(0, 0, 1.414f, 1.414f), 4, 1);
        left = new weaponInfo(new Vector3(-0.8f, 0.4f, 0), new Vector3(-0.5f, 0, 0), new Quaternion(0, 0, 0, 1), 4, 1);
        right = new weaponInfo(new Vector3(0.8f, 0.5f, 0), new Vector3(0.5f, 0, 0), new Quaternion(0, 0, -1.414f, 1.414f), -4, 1);

    }

    public weaponInfo GetWeaponInfo(int name)
    {
        
        switch (name)
        {
            case 0:
                face = up;
                break;
            case 1:
                face = down;
                break;
            case 2:
                face = left;
                break;
            case 3:
                face = right;
                break;
        }
        return face;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
