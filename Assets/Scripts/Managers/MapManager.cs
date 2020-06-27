using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager: MonoBehaviour
{
    public const int wallCount = 6;
    public List<GameObject> wall = new List<GameObject>();
    public GameObject floor;
    public GameObject food;
    public GameObject barrier;
    public List<GameObject> enemy = new List<GameObject>();
    public GameObject player;
    public GameObject door;
    public GameObject chest;
    public GameObject portal;
    public GameObject businessman;
    public GameObject forgeman;


    public int minBarrierCount;
    public int maxBarrierCount;
    public int minenemyCount;
    public int maxenemyCount;
    public int minChestCount;
    public int maxChestCount;
    public int stage;

    public Transform mapholder;
    private List<Vector2> positionList = new List<Vector2>();
    private List<GameObject> spawnWall = new List<GameObject>();
    private List<Vector2> allDoorPos = new List<Vector2>();
    public int xLength = 45;
    public int yLength = 30;
    private bool stopSpawn = false;

    public GameObject[] MapParts;

    //AI系统
    public AstarPath astarPath;
    void Awake()
    {
        initMap();
    }


    public void DestroyMap()
    {
        Destroy(mapholder.gameObject);
    }

    public void initMap()
    {

        mapholder = new GameObject("Map").transform;
        GameObject go = Instantiate(MapParts[0], Vector3.zero, Quaternion.identity);
        GameObject go1 = Instantiate(MapParts[0], new Vector3(23, 0, 0), Quaternion.identity);
        GameObject go2 = Instantiate(MapParts[0], new Vector3(23, -14, 0), Quaternion.identity);
        GameObject go3 = Instantiate(MapParts[0], new Vector3(0, -14, 0), Quaternion.identity);

        ////创建主角
        //if(Gamemanager.Instance.isFirstStart)
        //{
        //    Gamemanager.Instance.isFirstStart = false;
        //    GameObject.Instantiate(player, new Vector2(1, 1), Quaternion.identity);
        //    player.GetComponent<playInfo>().RoleType = (RoleType)PlayerPrefs.GetInt("roleType",0);
        //}



        //astarPath.Scan();


    }

    private void CreateItem(GameObject item, int minIndex, int maxIndex)
    {
        int positionIndex = Random.Range(0, positionList.Count);
        Vector2 pos = positionList[positionIndex];
        positionList.RemoveAt(positionIndex);
        GameObject go = GameObject.Instantiate(item, pos, Quaternion.identity);
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
