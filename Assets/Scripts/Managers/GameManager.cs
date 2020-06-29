using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    public int sceneLevel = 1;
    private static GameManager _instance;
    private MapManager mapmanager;
    public GameObject player;
    private bool sleepStep;
    //UI部分
    public Button bag_Button;
    public GameObject skillPanel;

    //游戏逻辑部分
    public bool isFirstStart = true;
    public bool isPause = false;
    private int SaveIndex;
    public bool canInput = true;
    public bool isBag = false;
    public bool isSkill = false;


    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject Player
    {
        get
        {
            return player;
        }

        set
        {
            player = value;
        }
    }


    // Use this for initialization

    private void Awake()
    {
        if (GameManager.Instance != null)
            Destroy(gameObject);
    }
    void Start()
    {
        _instance = this;
        //存档功能，待完善
        //if (PlayerPrefs.GetInt("isContinue",0) == 1)
        //{
        //    GameObject go = Resources.Load("Prefebs/Player/player1") as GameObject;
        //    Player = GameObject.Instantiate(go, new Vector2(1, 1), Quaternion.identity);
        //    SaveIndex = PlayerPrefs.GetInt("saveIndex");
        //    Read(SaveIndex);
        //}
        //else
        //{
        //    SaveIndex= PlayerPrefs.GetInt("saveIndex");
        //}
        /*
        PlayInfoManager.Instance.playerATK += PlayerPrefs.GetInt("ATK");
        PlayInfoManager.Instance.playerHP += PlayerPrefs.GetInt("HP");
        PlayInfoManager.Instance.UpdatePlayInfo();
        PlayInfoManager.Instance.currentHP = PlayInfoManager.Instance.maxHP;
        */
        initGame();
        mapmanager.initMap();
        // SceneManager.sceneLoaded += OnSceneLoaded;//场景加载时调用
    }

    void initGame()
    {
        //AudioManager.Instance.PlayEffectSound(AudioManager.Instance.bgmAudioSource);
        mapmanager = GetComponent<MapManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void reStart()
    {
        GameManager.Instance.sceneLevel++;
        mapmanager.DestroyMap();
        mapmanager.initMap();
        Instance.Player.transform.position = new Vector2(1, 1);
        //   SaveGame(SaveIndex);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);

    }
    static private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Instance.initGame();
        Instance.Player.transform.position = new Vector2(1, 1);
        Debug.Log("load");
    }

}
