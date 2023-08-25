using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    //나의 커서 photonView 담을 변수
    public PhotonView cursorPhotonView;

    public CinemachineVirtualCamera cinemachineVirtualCamera;

    public RPlayer rPlayer;

    //컬러 색상을 바꾸고 싶다.
    public Color[] nickNameColors;

    public Queue<int> colorsIndex = new Queue<int>();

    public int myColorIndex;

    private void Awake()
    {
        instance = this;

        //colorIndex에  nickNameColors의 길이만큼의 숫자를 넣는다.
        for (int i = 0; i < nickNameColors.Length; i++)
        {
            colorsIndex.Enqueue(i);
        }
        //컬러 범위를 byte로 random 값을 준다.
        SoundManager.Instance.PlayBGM("Room");
    }

    //해당 플레이어 선택
    public void SelectPlayer(PhotonView playerPhoton)
    {
        //선택한 플레이어의 포톤을 커서 포톤으로 넘긴다.
        playerPhoton.TransferOwnership(cursorPhotonView.Owner);
        //플레이어가 선택되면 커서 비활성화를 동기화한다.
        cursorPhotonView.RPC("CursorDisable", RpcTarget.All, false);
        //카메라 타깃 변경
        cinemachineVirtualCamera.Follow = playerPhoton.transform;
        rPlayer = playerPhoton.GetComponent<RPlayer>();

        SetActivePlayer(true);
    }

    public void SetActivePlayer(bool isEnabled)
    {
        //플레이어를 가졌다라는 것을 알린다.
        rPlayer.GetComponent<OwnershipTransfer>().HasPlayer(isEnabled, cursorPhotonView.Owner.ActorNumber);
    }
}