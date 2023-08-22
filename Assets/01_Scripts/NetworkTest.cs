using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class NetworkTest : MonoBehaviourPunCallbacks
{
    public static NetworkTest instance;

    bool isStart = false;

    [Header("Title")]
    public GameObject startTextObj;

    [Header("Board")]
    public RectTransform rtBoard;

    [Header("NickName")]
    public GameObject nickNameObj;
    public TMP_InputField nickNameInputField;
    public TextMeshProUGUI warnText;
    public Button searchButton;
    public Button nickNameBackButton;

    [Header("Room")]
    public GameObject roomObj;
    public Button createButton;
    public Button roomBackButton;
    public RectTransform rtContent;
    public GameObject roomItemFactory;

    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        startTextObj.transform.DOScale(1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        SoundManager.Instance.PlayBGM("Title");

        searchButton.onClick.AddListener(searchButtonEvent);
        nickNameBackButton.onClick.AddListener(NickNameBackButtonEvent);
        createButton.onClick.AddListener(CreateButtonEvent);
        roomBackButton.onClick.AddListener(RoomBackButtonEvent);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isStart)
        {
            isStart = true;
            //로비진입
            //Photon 환경설정을 기반으로 접속을 시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //마스터 서버 접속 완료
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        rtBoard.DOLocalMoveY(0, 0.5f).From(-1200).SetEase(Ease.Linear);
    }

    //로비진입
    void searchButtonEvent()
    {
        string nickName = nickNameInputField.text;
        if (string.IsNullOrEmpty(nickName))
        {
            warnText.color = Color.white;
            warnText.DOFade(0, 0.5f).SetDelay(0.5f).SetEase(Ease.Linear);
            return;
        }

        nickNameObj.SetActive(false);
        //닉네임 설정
        PhotonNetwork.NickName = nickName;
        //기본 Lobby 입장
        PhotonNetwork.JoinLobby();
    }

    //로비진입 완료
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        roomObj.SetActive(true);
    }

    void NickNameBackButtonEvent()
    {
        PhotonNetwork.Disconnect();
        rtBoard.DOLocalMoveY(-1200, 0.5f).SetEase(Ease.Linear);
        isStart = false;
    }

    //방 생성
    void CreateButtonEvent()
    {
        //방 생성 or 참여
        RoomOptions roomOptioin = new RoomOptions();
        roomOptioin.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(PhotonNetwork.NickName, roomOptioin, TypedLobby.Default);
    }

    //방 생성 완료시 호출 되는 함수
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(nameof(OnCreatedRoom));
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        //방 생성 실패 원인을 보여주는 팝업 띄워줘야 겠죠?
    }

    void RoomBackButtonEvent()
    {
        PhotonNetwork.LeaveLobby();
        roomObj.SetActive(false);
        nickNameObj.SetActive(true);
    }

    //방 참여
    public void JoinedRoom(string name)
    {
        PhotonNetwork.JoinRoom(name);
    }

    //방 참여 성공시 호출되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print(nameof(OnJoinedRoom));
        //Game Scene 으로 이동
        PhotonNetwork.LoadLevel("BMJ_Alpha_Test");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        RemoveRoomList();
        UpdateRoomList(roomList);
        CreateRoomList();
    }

    void RemoveRoomList()
    {
        for (int i = 0; i < rtContent.childCount; i++)
            Destroy(rtContent.GetChild(i).gameObject);

        //foreach(Transform child in rtContent)
        //    Destroy(child.gameObject);
    }

    void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (roomCache.ContainsKey(info.Name))
            {
                if (info.RemovedFromList)
                {
                    roomCache.Remove(info.Name);
                    continue;
                }
            }

            roomCache[info.Name] = info;
        }
    }

    void CreateRoomList()
    {
        int roomNum = 1;
        foreach (RoomInfo info in roomCache.Values)
        {
            RoomItem item = Instantiate(roomItemFactory, rtContent).GetComponent<RoomItem>();
            item.SetInfo(roomNum, info.PlayerCount, "로비", info.Name, JoinedRoom);
            roomNum++;
        }
    }
}
