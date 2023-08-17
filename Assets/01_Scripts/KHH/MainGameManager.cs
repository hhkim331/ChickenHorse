using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviourPunCallbacks
{
    public static MainGameManager instance;

    ScoreManager scoreMgr;
    public ScoreManager ScoreMgr { get { return scoreMgr; } }

    [SerializeField] FollowCamera followCamera;
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera partyBoxCamera;
    [SerializeField] Camera cursorCamera;
    [SerializeField] MapManager mapMgr;

    //좌표격자
    public SpriteRenderer[] graphSprites;
    public SpriteRenderer[] graphFadeSprites;
    public PartyBox partyBox;

    List<UserCursor> cursors;
    UserCursor myCursor;
    public UserCursor MyCursor { get { return myCursor; } }

    Vector3 startPos;

    public int myCharacterIndex;
    //public GameObject[] playerPrefab;

    //텍스트
    [SerializeField] GameObject readyTextObject;

    //활성화 플레이어
    List<KHHPlayerMain> players;
    KHHPlayerMain myPlayer;
    List<StageObject> stageObjects = new List<StageObject>();

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

        DOTween.SetTweensCapacity(500, 50);
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        Init();
        partyBox.Init();
        startPos = new Vector3(MapManager.instance.startBlock.transform.position.x, MapManager.instance.startBlock.transform.position.y, 0);

        followCamera.State = FollowCamera.CameraState.FullScreen;
        followCamera.Init(mapMgr.mapSize);

        scoreMgr = GetComponent<ScoreManager>();
        scoreMgr.playerTest = myPlayer.gameObject;
        scoreMgr.Init();

        yield return new WaitForSeconds(1f);

        cursors = new List<UserCursor>(FindObjectsOfType<UserCursor>());
        players = new List<KHHPlayerMain>(FindObjectsOfType<KHHPlayerMain>());

        followCamera.Set(players, cursors);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            photonView.RPC(nameof(ChangeState), RpcTarget.All, GameState.Select);
    }

    void Init()
    {
        Character myCharacter = PlayerData.instance.PlayerCharacterDic[photonView.Owner.ActorNumber];

        //자신의 커서 생성
        myCursor = PhotonNetwork.Instantiate("UserCursor" + myCharacter.characterType, Vector3.zero, Quaternion.identity).GetComponent<UserCursor>();
        myCursor.Init(mainCamera, partyBoxCamera, cursorCamera);
        myCursor.Active(false);

        //나의 Player 생성
        myPlayer = PhotonNetwork.Instantiate(myCharacter.prefabDirectory, Vector3.zero, Quaternion.identity).GetComponent<KHHPlayerMain>();
        myPlayer.gameObject.SetActive(false);
    }

    //void CreatePlayer()
    //{
    //    //플레이어 생성
    //    myPlayer = Instantiate(playerPrefab[myCharacterIndex]);
    //}

    private void Update()
    {
        if (!photonView.Owner.IsMasterClient) return;

        switch (state)
        {
            case GameState.Select:
                bool allSelect = true;
                foreach (var cursor in cursors)
                    if (!cursor.IsSelect)
                    {
                        allSelect = false;
                        break;
                    }
                if (allSelect)
                    photonView.RPC(nameof(ChangeState), RpcTarget.All, GameState.Place);
                break;
            case GameState.Place:
                bool allPlace = true;
                foreach (var cursor in cursors)
                    if (!cursor.IsPlace)
                    {
                        allPlace = false;
                        break;
                    }
                if (allPlace)
                    photonView.RPC(nameof(ChangeState), RpcTarget.All, GameState.Play);
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
        ////모든 플레이어가 준비가 되었는지 확인
        //if (!myPlayer.gameObject.activeSelf) return;

        //플레이어 움직임 활성화

        //플레이어 상태 확인
        bool isActivePlayer = false;
        foreach (var player in players)
            if (player.IsActive)
            {
                isActivePlayer = true;
                break;
            }

        //모든 플레이어가 못움직일때
        if (!isActivePlayer)
        {
            photonView.RPC(nameof(CurPlayEnd), RpcTarget.All);
        }
    }

    [PunRPC]
    void CurPlayEnd()
    {
        bool isGoal = false;
        foreach (var player in players)
        {
            if (player.isGoal)
            {
                isGoal = true;
                break;
            }
        }

        if (isGoal)
            followCamera.SetGoal(new List<GameObject>() { myPlayer.gameObject });

        ChangeState(GameState.Score);
    }


    [PunRPC]
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
                myCursor.Set();
                partyBox.SetBox();
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
                PlayStageObject();
                break;
            case GameState.Score:
                //myPlayer.SetActive(false);  //플레이어 비활성화
                scoreMgr.ScoreCalc();
                StopStageObject();
                break;
            case GameState.End:
                End();
                break;
        }
    }

    //플레이어 초기화
    void ResetPlayer()
    {
        myPlayer.gameObject.SetActive(true);
        myPlayer.transform.position = startPos;
        myPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;
        myPlayer.GetComponent<KHHPlayerMain>().ResetPlayer();
        StartCoroutine(ResetPlayerCoroutine());
    }

    IEnumerator ResetPlayerCoroutine()
    {
        readyTextObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        myPlayer.GetComponent<KHHPlayerMain>().ActiveMove();
        readyTextObject.SetActive(false);
    }

    void End()
    {
        myPlayer.gameObject.SetActive(true);  //우승 플레이어만 활성화
        followCamera.SetEnd(myPlayer.transform.position);
    }

    public void AddStageObject(StageObject stageObject)
    {
        stageObjects.Add(stageObject);
    }

    public void RemoveStageObject(StageObject stageObject)
    {
        stageObjects.Remove(stageObject);
    }

    void PlayStageObject()
    {
        foreach (var stageObject in stageObjects)
            stageObject.Play();
    }

    void StopStageObject()
    {
        foreach (var stageObject in stageObjects)
            stageObject.Stop();
    }

    //새로운 인원이 방에 들어왔을때 호출되는 함수
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        print(newPlayer.NickName + "님이 들어왔습니다!");
    }
}