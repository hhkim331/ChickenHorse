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

    //나 자시자신을 끄는 것을 확인하고 싶다.
    public RPlayer rplayer;

    private void Awake()
    {
        instance = this;
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
        //플레이어의 이동 컴포넌트를 가져온다.
        rplayer = playerPhoton.GetComponent<RPlayer>();

        SetActivePlayer(true);
    }

    public void SetActivePlayer(bool isEnable)
    {
        //내가 취소한 플레이어의 움직임을 끈다.
        rplayer.enabled = isEnable;
        //플레이어가 가지고 있는 포톤에서 플레이어가 체크 되어있으면 모든 컴퓨터에 끄는 것을 동기화 한다.
        rplayer.GetComponent<OwnershipTransfer>().HasPlayer(isEnable, cursorPhotonView.Owner.ActorNumber);
    }
}