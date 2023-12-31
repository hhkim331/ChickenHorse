﻿using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    Vector3 endPos;

    public int myCharacterIndex;
    //public GameObject[] playerPrefab;

    //텍스트
    [SerializeField] GameObject readyTextObject;

    //활성화 플레이어
    Dictionary<int, KHHPlayerMain> players;
    List<(int, string)> actors;
    KHHPlayerMain myPlayer;
    List<StageObject> stageObjects = new List<StageObject>();

    //게임플레이
    bool isPlay = false;
    bool anyPlayerDie = false;
    float dieCameraTime = 0;
    float dieCameraWait = 2;
    Coroutine dieCamCoroutine = null;

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

        SoundManager.Instance.PlayBGM("Stage");

        Init();
        partyBox.Init();
        startPos = new Vector3(MapManager.instance.startBlock.transform.position.x, MapManager.instance.startBlock.transform.position.y, 0);
        endPos = new Vector3(MapManager.instance.endBlock.transform.position.x, MapManager.instance.endBlock.transform.position.y, 0);

        followCamera.State = FollowCamera.CameraState.FullScreen;
        followCamera.Init(mapMgr.mapSize);

        yield return new WaitForSeconds(1f);

        cursors = new List<UserCursor>(FindObjectsOfType<UserCursor>());
        players = new Dictionary<int, KHHPlayerMain>();
        actors = new List<(int, string)>();
        KHHPlayerMain[] kHHPlayerMains = FindObjectsOfType<KHHPlayerMain>();
        foreach (var player in kHHPlayerMains)
        {
            players.Add(player.photonView.Owner.ActorNumber, player);
            actors.Add((player.photonView.Owner.ActorNumber, player.photonView.Owner.NickName));
        }

        actors.Sort();
        scoreMgr = GetComponent<ScoreManager>();
        scoreMgr.Init(actors);
        followCamera.Set(players, actors, cursors);

        ChangeState(GameState.Select);
    }

    void Init()
    {
        Character myCharacter = PlayerData.instance.PlayerCharacterDic[PhotonNetwork.LocalPlayer.ActorNumber];

        //자신의 커서 생성
        myCursor = PhotonNetwork.Instantiate("UserCursor" + myCharacter.characterType, Vector3.zero, Quaternion.identity).GetComponent<UserCursor>();
        myCursor.Init(mainCamera, partyBoxCamera, cursorCamera);

        //나의 Player 생성
        myPlayer = PhotonNetwork.Instantiate("Player" + myCharacter.characterType, Vector3.zero, Quaternion.identity).GetComponent<KHHPlayerMain>();
        myPlayer.Active(false, startPos);
    }

    //void CreatePlayer()
    //{
    //    //플레이어 생성
    //    myPlayer = Instantiate(playerPrefab[myCharacterIndex]);
    //}

    private void Update()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient) return;

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
                    ChangeState(GameState.Place);
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
                    ChangeState(GameState.Play);
                break;
            case GameState.Play:
                if (isPlay)
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
            if (player.Value.IsActive)
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

    public void AnyPlayerDie()
    {
        anyPlayerDie = true;
        followCamera.State = FollowCamera.CameraState.FullScreen;
        dieCameraTime = 0;
        if (dieCamCoroutine != null)
            StopCoroutine(dieCamCoroutine);
        dieCamCoroutine = StartCoroutine(DieCamera());
    }

    IEnumerator DieCamera()
    {
        while (dieCameraTime < dieCameraWait)
        {
            dieCameraTime += Time.deltaTime;
            yield return null;
        }

        if (state == GameState.Play)
        {
            followCamera.State = FollowCamera.CameraState.Play;
            anyPlayerDie = false;
        }
    }

    [PunRPC]
    void CurPlayEnd()
    {
        isPlay = false;
        anyPlayerDie = false;
        bool isGoal = false;
        List<GameObject> goalPlayers = new List<GameObject>();
        foreach (var player in players)
        {
            if (player.Value.IsGoal)
            {
                goalPlayers.Add(player.Value.gameObject);
                isGoal = true;
            }
        }

        if (isGoal)
            followCamera.SetGoal(goalPlayers);
        else
            followCamera.SetNoGoal();

        ChangeState(GameState.Score);
    }


    public void ChangeState(GameState state)
    {
        if (this.state == state) return;
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            photonView.RPC(nameof(CSRPC), RpcTarget.All, state);
    }

    [PunRPC]
    void CSRPC(GameState state) //ChangeStateRPC
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
                followCamera.State = FollowCamera.CameraState.FullScreen;
                foreach (var sprite in graphSprites)
                    sprite.DOFade(0, 0.5f);
                foreach (var sprite in graphFadeSprites)
                    sprite.DOFade(0, 0.5f);
                ResetPlayer();
                PlayStageObject();
                PlayStart();
                break;
            case GameState.Score:
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
        //myPlayer.gameObject.SetActive(true);
        //myPlayer.transform.position = startPos;
        myPlayer.Active(true, startPos);
    }

    void PlayStart()
    {
        readyTextObject.SetActive(true);
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            StartCoroutine(PlayStartrCoroutine());
    }

    IEnumerator PlayStartrCoroutine()
    {
        yield return new WaitForSeconds(1f);
        photonView.RPC(nameof(PSRPC), RpcTarget.All);
    }

    [PunRPC]
    void PSRPC() //PlayStartRPC
    {
        followCamera.State = FollowCamera.CameraState.Play;
        isPlay = true;
        myPlayer.ActiveMove();
        readyTextObject.SetActive(false);
    }

    void End()
    {
        players[scoreMgr.winner].Active(true, endPos);
        followCamera.SetEnd(endPos);
        StartCoroutine(EndWait());
    }

    IEnumerator EndWait()
    {
        yield return new WaitForSeconds(5f);
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            PhotonNetwork.LoadLevel("BMJ_Alpha_Test");
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

    public void PlayerInactive()
    {
        myPlayer.Active(false, startPos);
    }

    ////새로운 인원이 방에 들어왔을때 호출되는 함수
    //public override void OnPlayerEnteredRoom(Player newPlayer)
    //{
    //    base.OnPlayerEnteredRoom(newPlayer);

    //    print(newPlayer.NickName + "님이 들어왔습니다!");
    //}

    private void OnApplicationFocus(bool focus)
    {
        Cursor.visible = !focus;
    }
}