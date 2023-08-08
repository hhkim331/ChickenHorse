using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KHHGameManager : MonoBehaviour
{
    public static KHHGameManager instance;
    ScoreManager scoreMgr;
    public ScoreManager ScoreMgr { get { return scoreMgr; } }

    public PartyBox partyBox;
    public UserCursor cursor;

    Vector3 startPos;

    public int myCharacterIndex;
    public GameObject[] playerPrefab;

    //활성화 플레이어
    public GameObject myPlayer;
    List<GameObject> playerList = new List<GameObject>();


    public enum GameState
    {
        None,
        Select,
        Place,
        Play,
        Score,
        End,
    }
    public GameState state;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        partyBox.Init();

        //자신의 커서 생성
        //커서 비활성화
        cursor.Deactive();

        startPos = new Vector3(MapManager.instance.startBlock.transform.position.x, MapManager.instance.startBlock.transform.position.y, 0);

        ChangeState(GameState.Select);
        CreatePlayer();

        scoreMgr = GetComponent<ScoreManager>();
        scoreMgr.playerTest = myPlayer;
        scoreMgr.Init();
    }

    void CreatePlayer()
    {
        //플레이어 생성
        myPlayer = Instantiate(playerPrefab[myCharacterIndex]);
        myPlayer.SetActive(false);
        playerList.Add(myPlayer);
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.Select:
                if (cursor.IsSelect)
                    ChangeState(GameState.Place);
                break;
            case GameState.Place:
                if (cursor.IsPlace)
                    ChangeState(GameState.Play);
                break;
            case GameState.Play:
                UpdatePlay();
                break;
            case GameState.Score:
                break;
            case GameState.End:
                break;
        }
    }

    void UpdatePlay()
    {
        //모든 플레이어가 준비가 되었는지 확인
        if (!myPlayer.activeSelf) return;

        //플레이어 움직임 활성화

        //플레이어 상태 확인
        bool allActive = false;
        foreach (var player in playerList)
            if (player.GetComponent<KHHPlayerTest>().isActive)
            {
                allActive = true;
                break;
            }

        if (!allActive)
        {
            //모든 플레이어가 못움직일때
            ChangeState(GameState.Score);
        }
    }

    public void ChangeState(GameState state)
    {
        this.state = state;
        switch (state)
        {
            case GameState.Select:
                cursor.Set();
                partyBox.SetBox();
                partyBox.Open();
                break;
            case GameState.Place:
                partyBox.Close();
                cursor.Active();
                break;
            case GameState.Play:
                ResetPlayer();
                break;
            case GameState.Score:
                scoreMgr.ScoreCalc();
                break;
            case GameState.End:
                break;
        }
    }

    //플레이어 초기화
    void ResetPlayer()
    {
        myPlayer.SetActive(true);
        myPlayer.transform.position = startPos;
        myPlayer.GetComponent<KHHPlayerTest>().ResetPlayer();

        myPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}