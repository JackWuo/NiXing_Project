using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] outWallArray;
    public GameObject[] floorArray;

    public int mapHeight = 30;
    public int mapWidth = 30;

    private Transform mapHolder;
    // Start is called before the first frame update
    void Start()
    {
        InitMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitMap()
    {
        mapHolder = new GameObject("Map").transform;
        for(int x=0; x < mapWidth; x++)
        {
            for(int y=0; y< mapWidth; y++)
            {
                // 生成外墙
                if(x == 0 || y == 0 || x == mapWidth-1 || y == mapHeight-1)
                {
                    int wallIndex = Random.Range(0, outWallArray.Length);
                    GameObject outWall = GameObject.Instantiate(outWallArray[wallIndex], new Vector3(x, y, 0), Quaternion.identity);
                    outWall.transform.SetParent(mapHolder);
                }
                // 生成地板
                else
                {
                    int floorIndex = Random.Range(0, floorArray.Length);
                   GameObject floor = GameObject.Instantiate(floorArray[floorIndex], new Vector3(x, y, 0), Quaternion.identity);
                    floor.transform.SetParent(mapHolder);
                }
            }
        }
    }
}
