using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MapManager : MonoBehaviour
{ 
    public List<GameObject> wall = new List<GameObject>();
    public GameObject floor;
    public GameObject barrier;
    public List<GameObject> enemy = new List<GameObject>();
    public GameObject player;
    public GameObject door;
    public GameObject medicalChest;      // 医疗箱
    public GameObject medicalRobot;     // 医疗机器人
    public GameObject portal;                 // 传送门


    public int minBarrierCount;
    public int maxBarrierCount;
    public int minenemyCount;
    public int maxenemyCount;
    public int minChestCount;
    public int maxChestCount;
    private int level;

    public Transform mapholder;
    private List<Vector2> positionList = new List<Vector2>();
    private List<GameObject> spawnWall = new List<GameObject>();
    private List<Vector2> allDoorPos = new List<Vector2>();
    public int xLength = 40;
    public int yLength = 40;
    private bool stopSpawn = false;


    //AI系统
    public AstarPath astarPath;
    // Update is called once per frame
    void Update()
    {

    }
    private void Start()
    {
        initMap();
    }

    public void DestroyMap()
    {
        Destroy(mapholder.gameObject);

    }

    public void initMap()
    {
        //关卡阶段
        level = GameManager.Instance.sceneLevel / 5;
        if (GameManager.Instance.sceneLevel == 5)
            AudioManager.Instance.StopPlayBGM();
        //创建地板和围墙
        mapholder = new GameObject("Map").transform;
        for (int i = 0; i < xLength; i++)
            for (int j = 0; j < yLength; j++)
                if (i == 0 || j == 0 || i == xLength - 1 || j == yLength - 1)
                {

                    GameObject go = GameObject.Instantiate(wall[1], new Vector3(i, j, 0), Quaternion.identity);
                    go.transform.SetParent(mapholder);
                }
                else
                {
                    GameObject go = GameObject.Instantiate(floor, new Vector3(i, j, 0), Quaternion.identity);
                    go.transform.SetParent(mapholder);
                }

        positionList.Clear();
        for (int x = 1; x < xLength - 1; x++)
        {
            for (int y = 1; y < yLength - 1; y++)
            {
                positionList.Add(new Vector2(x, y));
            }
        }

        for (int x = 1; x < 4; x++)
        {
            for (int y = 1; y < 4; y++)
            {
                positionList.Remove(new Vector2(x, y));
            }
        }

        //创建主角
        if (GameManager.Instance.isFirstStart)
        {
            GameManager.Instance.isFirstStart = false;
            GameObject p = GameObject.Instantiate(player, new Vector2(1, 1), Quaternion.identity);
            p.transform.SetParent(mapholder);
            // player.GetComponent<playInfo>().RoleType = (RoleType)PlayerPrefs.GetInt("roleType", 0);
        }


        //创建围墙

        SpawnWall(new Vector2(Random.Range(xLength / 5, xLength / 2), 0), 0);
        SpawnWall(new Vector2(Random.Range(xLength / 5, xLength / 2), yLength - 1), 0);
        SpawnWall(new Vector2(0, Random.Range(yLength / 5, yLength / 2)), 0);
        SpawnWall(new Vector2(xLength - 1, Random.Range(yLength / 5, yLength / 2)), 0);
        SpawnWall(new Vector2(Random.Range(xLength / 2, xLength - 1), yLength - 1), 0);
        SpawnWall(new Vector2(Random.Range(xLength / 2, xLength - 1), 0), 0);







        //创建障碍物
        int barrierCount = Random.Range(minBarrierCount, maxBarrierCount + 1);
        for (int i = 0; i < barrierCount; i++)
        {

            CreateItemWithoutDoor(barrier, minBarrierCount, maxBarrierCount);

        }
        //创建宝箱
        int chestCount = Random.Range(minChestCount, maxChestCount + 1);
        for (int i = 0; i < chestCount; i++)
        {
            CreateItemWithoutDoor(medicalChest, minChestCount, maxChestCount);
        }

        //创建传送门
        Vector2 portalPos = new Vector2(xLength - 2, yLength - 2);
        GameObject port = GameObject.Instantiate(portal, portalPos, Quaternion.identity);
        port.transform.SetParent(mapholder);
        positionList.Remove(portalPos);


        //创建医疗机器人
        if (GameManager.Instance.sceneLevel % 5 == 3)
        {
            CreateItemWithoutDoor(medicalRobot, 1, 2);
        }

        //创建敌人
        int enemyCount = Random.Range(minenemyCount, maxenemyCount + 1);
        for (int i = 0; i < enemyCount; i++)
        {
            int enemy_index = Random.Range(0, 2);
            CreateItem(enemy[enemy_index], minenemyCount, maxenemyCount);
        }
        if (GameManager.Instance.sceneLevel % 5 == 1)
        {
            CreateBigItem(enemy[2], 1, 2);
        }
        astarPath = GetComponent<AstarPath>();
        if (astarPath)
        {
            astarPath.Scan();
        }
    }

    private void CreateItem(GameObject item, int minIndex, int maxIndex)
    {
        int positionIndex = Random.Range(0, positionList.Count);
        Vector2 pos = positionList[positionIndex];
        positionList.RemoveAt(positionIndex);
        GameObject go = GameObject.Instantiate(item, pos, Quaternion.identity);
        go.GetComponent<enemyAI>().target = FindObjectOfType<player>().transform;
        if (go.tag == "player")
        {
        }
        else
            go.transform.SetParent(mapholder);
    }
    private void CreateBigItem(GameObject item, int minIndex, int maxIndex)
    {
        int positionIndex = Random.Range(0, positionList.Count);
        bool canSpawn = false;
        Vector2 SpawnPos = positionList[positionIndex] + new Vector2(0.55f, 0.5f);
        if (positionList.Contains(SpawnPos - new Vector2(0.55f, 0.5f)) && positionList.Contains(SpawnPos - new Vector2(0.55f, -0.5f)) && positionList.Contains(SpawnPos - new Vector2(-0.45f, 0.5f))
            && positionList.Contains(SpawnPos - new Vector2(-0.45f, -0.5f)))
        {
            canSpawn = true;
        }
        if (canSpawn)
        {
            positionList.Remove(SpawnPos - new Vector2(0.55f, 0.5f));
            positionList.Remove(SpawnPos - new Vector2(0.55f, -0.5f));
            positionList.Remove(SpawnPos - new Vector2(-0.45f, 0.5f));
            positionList.Remove(SpawnPos - new Vector2(-0.45f, -0.5f));
            GameObject go = GameObject.Instantiate(item, SpawnPos, Quaternion.identity);
            go.transform.SetParent(mapholder);
        }
        else
        {
            CreateBigItem(item, minIndex, maxIndex);
        }

    }
    private void CreateItemWithoutDoor(GameObject item, int minIndex, int maxIndex)
    {
        int positionIndex = Random.Range(0, positionList.Count);
        Vector2 targetPosition = positionList[positionIndex];
        if (allDoorPos.Contains(targetPosition + new Vector2(0, 1)) || allDoorPos.Contains(targetPosition + new Vector2(0, -1)) ||
                    allDoorPos.Contains(targetPosition + new Vector2(-1, 0)) || allDoorPos.Contains(targetPosition + new Vector2(1, 0)))

        {
            CreateItemWithoutDoor(item, minIndex, maxIndex);
        }
        else
        {
            positionList.RemoveAt(positionIndex);
            GameObject go = GameObject.Instantiate(item, targetPosition, Quaternion.identity);
            go.transform.SetParent(mapholder);
        }

    }

    private void InitWallFromThis(Vector2 spawnPosition, int index)
    {
        List<Vector2> canSpawnPos = new List<Vector2>();

        Vector2 leftPos = spawnPosition + new Vector2(-1, 0);
        if (positionList.Contains(leftPos))
            canSpawnPos.Add(leftPos);
        Vector2 rightPos = spawnPosition + new Vector2(1, 0);
        if (positionList.Contains(rightPos))
            canSpawnPos.Add(rightPos);
        Vector2 upPos = spawnPosition + new Vector2(0, 1);
        if (positionList.Contains(upPos))
            canSpawnPos.Add(upPos);
        Vector2 downPos = spawnPosition + new Vector2(0, -1);
        if (positionList.Contains(downPos))
            canSpawnPos.Add(downPos);

        if (canSpawnPos.Count != 0)
        {
            int randomIndex = Random.Range(0, canSpawnPos.Count);
            int spawnCount = Random.Range(yLength / 3, yLength / 2);
            Vector2 targetPosition = canSpawnPos[randomIndex];
            Vector2 lerpValue = targetPosition - spawnPosition;
            for (int i = 0; i < spawnCount; i++)
            {
                if (!positionList.Contains(targetPosition))
                {
                    stopSpawn = true;
                    break;
                }
                if (targetPosition == new Vector2(1, 1))
                {
                    stopSpawn = true;
                    break;
                }
                else if ((allDoorPos.Contains(targetPosition + new Vector2(0, 1)) || allDoorPos.Contains(targetPosition + new Vector2(0, -1)) ||
                    allDoorPos.Contains(targetPosition + new Vector2(-1, 0)) || allDoorPos.Contains(targetPosition + new Vector2(1, 0)))
                    && !allDoorPos.Contains(targetPosition - lerpValue)
                    )
                {
                    GameObject doorPre = GameObject.Instantiate(door, targetPosition, Quaternion.identity);
                    doorPre.transform.SetParent(mapholder);
                    allDoorPos.Add(doorPre.transform.position);
                }
                else if (allDoorPos.Contains(targetPosition + lerpValue) && !positionList.Contains(targetPosition + 2 * lerpValue))
                {
                    GameObject go = GameObject.Instantiate(wall[index], targetPosition, Quaternion.identity);
                    go.transform.SetParent(mapholder);
                    spawnWall.Add(go);
                    positionList.Remove(targetPosition);
                }
                else
                {
                    GameObject go = GameObject.Instantiate(wall[index], targetPosition, Quaternion.identity);
                    go.transform.SetParent(mapholder);
                    spawnWall.Add(go);
                    positionList.Remove(targetPosition);
                }
                targetPosition += lerpValue;
            }
            if (spawnWall.Count != 0)
            {
                SpawnDoorPre();

            }

            if (!stopSpawn)
            {
                targetPosition -= lerpValue;
                InitWallFromThis(targetPosition, index);
            }
            else
            {
                stopSpawn = false;

            }

        }

    }

    private void SpawnWall(Vector2 spawnPos, int index)
    {
        InitWallFromThis(spawnPos, index);


    }
    private void SpawnDoorPre()
    {
        if (spawnWall.Count < 3)
            return;
        int doorIndex = Random.Range(1, spawnWall.Count - 1);
        SpawnDoor(doorIndex, doorIndex - 1);

    }

    private void SpawnDoor(int doorIndex, int startIndex)
    {
        if (startIndex == doorIndex)
            return;
        if (doorIndex == 1 || doorIndex == spawnWall.Count - 1)
        {
            SpawnDoor((doorIndex + 1) % spawnWall.Count, startIndex);
            return;
        }

        int cantCount = 0;
        Vector2 doorPos = spawnWall[doorIndex].transform.position;
        if (!positionList.Contains(doorPos + new Vector2(0, 1)))
            cantCount++;
        if (!positionList.Contains(doorPos + new Vector2(0, -1)))
            cantCount++;
        if (!positionList.Contains(doorPos + new Vector2(1, 0)))
            cantCount++;
        if (!positionList.Contains(doorPos + new Vector2(-1, 0)))
            cantCount++;
        if (cantCount >= 3)
            SpawnDoor((doorIndex + 1) % spawnWall.Count, startIndex);
        else if (!positionList.Contains(doorPos + new Vector2(0, 1)) && !positionList.Contains(doorPos + new Vector2(-1, 0)))
            SpawnDoor((doorIndex + 1) % spawnWall.Count, startIndex);
        else if (!positionList.Contains(doorPos + new Vector2(0, 1)) && !positionList.Contains(doorPos + new Vector2(1, 0)))
            SpawnDoor((doorIndex + 1) % spawnWall.Count, startIndex);
        else if (!positionList.Contains(doorPos + new Vector2(0, -1)) && !positionList.Contains(doorPos + new Vector2(-1, 0)))
            SpawnDoor((doorIndex + 1) % spawnWall.Count, startIndex);
        else if (!positionList.Contains(doorPos + new Vector2(0, -1)) && !positionList.Contains(doorPos + new Vector2(1, 0)))
            SpawnDoor((doorIndex + 1) % spawnWall.Count, startIndex);
        else
        {
            Destroy(spawnWall[doorIndex]);
            GameObject doorPre = GameObject.Instantiate(door, doorPos, Quaternion.identity);
            doorPre.transform.SetParent(mapholder);
            allDoorPos.Add(doorPre.transform.position);
            spawnWall.Clear();
        }

    }
}