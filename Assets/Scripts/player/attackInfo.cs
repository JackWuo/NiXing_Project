using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackInfo : MonoBehaviour
{
    // Start is called before the first frame weaponUpdate

    public static float skillSpeed = 8f;
    //武器的初始位置以及朝向，之后会重写的所以不必在意这块硬编码= =
    public struct WeaponInfo
    {
        public Vector3 pos;
        public Vector3 rotPos;
        public Quaternion qua;
        public float speed;
        public int sortInLayer;
        public WeaponInfo(Vector3 p, Vector3 r, Quaternion q, float sp, int s)
        {
            pos = p;
            rotPos = r;
            qua = q;
            speed = sp;
            sortInLayer = s;
        }
    };
    private WeaponInfo weaponUp, weaponDown, weaponLeft, weaponRight;

    public struct SkillInfo
    {
        public Vector3 pos;
        public Quaternion qua;
        public float speed;
        public SkillInfo(Vector3 p, Quaternion q)
        {
            pos = p;
            qua = q;
            speed = skillSpeed;
        }
    }
    private SkillInfo skillUp, skillDown, skillLeft, skillRight;

    void Start()
    {
        weaponUp = new WeaponInfo(new Vector3(-0.4f, 0.8f, 0), new Vector3(0, 0.5f, 0), new Quaternion(0, 0, 0, 1), -4, 0);
        weaponDown = new WeaponInfo(new Vector3(-0.4f, -0.8f, 0), new Vector3(0, -0.5f, 0), new Quaternion(0, 0, 0.707f, 0.707f), 4, 0);
        weaponLeft = new WeaponInfo(new Vector3(-0.8f, 0.4f, 0), new Vector3(-0.5f, 0, 0), new Quaternion(0, 0, 0, 1), 4, 0);
        weaponRight = new WeaponInfo(new Vector3(0.8f, 0.5f, 0), new Vector3(0.5f, 0, 0), new Quaternion(0, 0, -0.707f, 0.707f), -4, 0);

        skillUp = new SkillInfo(new Vector3(0, -1, 0), new Quaternion(0, 0, 0.707f, 0.707f));
        skillDown = new SkillInfo(new Vector3(0, 1, 0), new Quaternion(0, 0, -0.707f, 0.707f));
        skillLeft = new SkillInfo(new Vector3(-0.8f, 0, 0), new Quaternion(0, 0, 1, 0));
        skillRight = new SkillInfo(new Vector3(0.8f, 0, 0), new Quaternion(0, 0, 0, 1));

    }

    public WeaponInfo GetWeaponInfo(int name)
    {
        
        switch (name)
        {
            case 0:
                return weaponUp;
            case 1:
                return weaponDown;
            case 2:
                return weaponLeft;
            case 3:
                return weaponRight;
            default:
                return new WeaponInfo();
        }
    }

    public SkillInfo GetSkillInfo(int name)
    {
        switch (name)
        {
            case 0:
                return skillUp;
            case 1:
                return skillDown;
            case 2:
                return skillLeft;
            case 3:
                return skillRight;
            default:
                return new SkillInfo();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
