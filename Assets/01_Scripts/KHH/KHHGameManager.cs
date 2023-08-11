using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KHHGameManager : MonoBehaviour
{
    public static KHHGameManager instance;

    ScoreManager scoreMgr;
    public ScoreManager ScoreMgr { get { return scoreMgr; } }

    [SerializeField] FollowCamera followCamera;
    [SerializeField] MapManager mapMgr;

    //좌표격자
    public SpriteRenderer[] graphSprites;
    public SpriteRenderer[] graphFadeSprites;
    public PartyBox partyBox;
    public List<UserCursor> cursors;

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
        cursors[0].Deactive();

        startPos = new Vector3(MapManager.instance.startBlock.transform.position.x, MapManager.instance.startBlock.transform.position.y, 0);

        ChangeState(GameState.Select);
        CreatePlayer();

        followCamera.Init(mapMgr.mapSize);
        followCamera.Set(playerList, cursors);

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
                if (cursors[0].IsSelect)
                    ChangeState(GameState.Place);
                break;
            case GameState.Place:
                if (cursors[0].IsPlace)
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

        //모든 플레이어가 못움직일때
        if (!allActive)
        {
            if (myPlayer.GetComponent<KHHPlayerTest>().isGoal)
                followCamera.SetGoal(new List<GameObject>() { myPlayer });


            ChangeState(GameState.Score);
        }
    }

    public void ChangeState(GameState state)
    {
        this.state = state;
        switch (state)
        {
            case GameState.Select:
                followCamera.State = FollowCamera.CameraState.FullScreen;
                foreach (var sprite in graphSprites)
                    sprite.DOFade(1, 0.5f);
                foreach (var sprite in graphFadeSprites)
                    sprite.DOFade(0.2f, 0.5f);
                cursors[0].Set();
                partyBox.SetBox();
                partyBox.Open();
                break;
            case GameState.Place:
                followCamera.State = FollowCamera.CameraState.Place;
                partyBox.Close();
                break;
            case GameState.Play:
                followCamera.State = FollowCamera.CameraState.Play;
                foreach (var sprite in graphSprites)
                    sprite.DOFade(0, 0.5f);
                foreach (var sprite in graphFadeSprites)
                    sprite.DOFade(0, 0.5f);
                ResetPlayer();
                break;
            case GameState.Score:
                //myPlayer.SetActive(false);  //플레이어 비활성화
                scoreMgr.ScoreCalc();
                break;
            case GameState.End:
                End();
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

    void End()
    {
        myPlayer.SetActive(true);  //우승 플레이어만 활성화
        followCamera.SetEnd(myPlayer.transform.position);
    }
}